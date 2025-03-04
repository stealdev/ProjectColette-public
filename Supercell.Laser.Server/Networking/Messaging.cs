﻿using Supercell.Laser.Server.Networking.Security;
using Supercell.Laser.Titan.Library.Blake;

namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Server.Message;
    using Supercell.Laser.Titan.Cryptography;
    using Supercell.Laser.Titan.Library;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.Math;
    using System.Linq;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Logic.Message.Account.Auth;

    public class Messaging
    {
        public byte[] SessionToken { get; }

        private Connection Connection;
        private int PepperState;

        private StreamEncrypter Encrypter;
        private StreamEncrypter Decrypter;

        private MessageFactory MessageFactory;
        private byte[] server_private_key = { 158, 217, 110, 5, 87, 249, 222, 234, 204, 121, 177, 228, 59, 79, 93, 217, 25, 33, 113, 185, 119, 171, 205, 246, 11, 185, 185, 22, 140, 152, 107, 20 };
        private byte[] s;
        private byte[] server_public_key;

        private byte[] RNonce;
        private byte[] SNonce;
        private byte[] SecretKey;

        public int Seed { get; set; }

        public bool DisableCrypto;

        public Messaging(Connection connection)
        {
            Connection = connection;
            MessageFactory = MessageFactory.Instance;

            PepperState = 2;

            SessionToken = new byte[24];
            SNonce = new byte[24];
            SecretKey = new byte[32];

            TweetNaCl.RandomBytes(SessionToken);
            TweetNaCl.RandomBytes(SNonce);
            TweetNaCl.RandomBytes(SecretKey);

            server_public_key = TweetNaCl.CryptoScalarmultBase(server_private_key);

            DisableCrypto = false;
        }

        public void Send(GameMessage message)
        {
            Debugger.Print(message.GetType() + " sent!");
            if (message.GetMessageType() == 20100)
            {
                EncryptAndWrite(message);
            }
            else
            {
                Processor.Send(Connection, message);
            }
        }

        public void EncryptAndWrite(GameMessage message)
        {
            if (message.GetEncodingLength() == 0) message.Encode();

            byte[] payload = new byte[message.GetEncodingLength()];
            Buffer.BlockCopy(message.GetMessageBytes(), 0, payload, 0, payload.Length);

            int messageType = message.GetMessageType();
            int version = message.GetVersion();

            if (!DisableCrypto)
                switch (PepperState)
                {
                    case 4:
                        payload = SendPepperLoginResponse(payload);
                        break;
                    case 5:
                        byte[] encrypted = new byte[payload.Length + Encrypter.GetEncryptionOverhead()];
                        Encrypter.Encrypt(payload, encrypted, payload.Length);
                        payload = encrypted;
                        break;
                }

            byte[] stream = new byte[payload.Length + 7];

            int length = payload.Length;

            stream[0] = (byte)(messageType >> 8);
            stream[1] = (byte)(messageType);
            stream[2] = (byte)(length >> 16);
            stream[3] = (byte)(length >> 8);
            stream[4] = (byte)(length);
            stream[5] = (byte)(version >> 8);
            stream[6] = (byte)(version);

            Buffer.BlockCopy(payload, 0, stream, 7, payload.Length);
            Connection.Write(stream);
        }

        public int OnReceive()
        {
            long position = Connection.Memory.Position;
            Connection.Memory.Position = 0;

            byte[] headerBuffer = new byte[7];
            Connection.Memory.Read(headerBuffer, 0, 7);

            // Messaging::readHeader inling? yes.
            int type = headerBuffer[0] << 8 | headerBuffer[1];
            int length = headerBuffer[2] << 16 | headerBuffer[3] << 8 | headerBuffer[4];
            int version = headerBuffer[5] << 8 | headerBuffer[6];

            byte[] payload = new byte[length];
            if (Connection.Memory.Read(payload, 0, length) < length)
            { // Packet still not received
                Connection.Memory.Position = position;
                return 0;
            }

            if (this.ReadNewMessage(type, length, version, payload) != 0)
            {
                return -1;
            }

            byte[] all = Connection.Memory.ToArray();
            byte[] buffer = all.Skip(length + 7).ToArray();

            Connection.Memory = new MemoryStream();
            Connection.Memory.Write(buffer, 0, buffer.Length);

            if (buffer.Length >= 7)
            {
                OnReceive();

            }
            return 0;
        }

        private int ReadNewMessage(int type, int length, int version, byte[] payload)
        {
            if (type != 10504) Debugger.Print(type + " received!" + "(version " + version + ")");
            if (PepperState == 2 && type == 10101) DisableCrypto = true;
            if (!DisableCrypto)
                switch (PepperState)
                {
                    case 2:
                        if (type == 10100) PepperState = 3;
                        else
                        {
                            //AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
                            //loginFailed.ErrorCode = 1;
                            //loginFailed.Message = "请更新最新版本客户端。";
                            //Connection.Send(loginFailed);
                            Connection.Send(new AuthenticationFailedMessage()
                            {
                                ErrorCode = 8,
                                UpdateUrl = "https://github.com/allbrawl/ProjectColette-public"
                            });
                            return -1;
                        }
                        break;
                    case 3:
                        if (type != 10101) return -1;
                        payload = HandlePepperLogin(payload);
                        if (payload == null) return -1;
                        break;
                    case 5:
                        byte[] decrypted = new byte[length - Decrypter.GetEncryptionOverhead()];
                        int result = Decrypter.Decrypt(payload, decrypted, length);
                        payload = decrypted;
                        if (result != 0) return -1;
                        break;
                }
            //AuthenticationFailedMessage loginFailed = new AuthenticationFailedMessage();
            //            loginFailed.ErrorCode = 1;
            //loginFailed.Message = "请更新最新版本客户端。";
            //Connection.Send(loginFailed);
            //return 0;
            GameMessage message = MessageFactory.CreateMessageByType(type);
            if (message != null)
            {
                message.GetByteStream().SetByteArray(payload, payload.Length);
                message.Decode();
                if (message.GetMessageType() == 10100)
                {
                    Connection.MessageManager.ReceiveMessage(message);
                }
                else
                {

                    Processor.Receive(Connection, message);

                }

            }
            else
            {
                //Logger.Print("Ignoring message of unknown type " + type);
            }

            return 0;
        }

        private byte[] client_pk;
        private byte[] client_sk;

        private byte[] HandlePepperLogin(byte[] payload)
        {
            try
            {
                //payload =new byte[]{ 24, 186, 147, 54, 48, 219, 55, 15, 51, 58, 53, 184, 103, 129, 217, 114, 109, 89, 60, 153, 201, 21, 139, 86, 227, 52, 98, 169, 123, 94, 83, 81, 211, 87, 8, 241, 11, 86, 25, 157, 2, 49, 242, 79, 72, 213, 247, 169, 217, 18, 44, 17, 184, 42, 3, 242, 176, 197, 152, 255, 189, 17, 182, 102, 158, 54, 107, 214, 51, 79, 219, 4, 68, 179, 94, 11, 120, 168, 79, 5, 138, 129, 246, 57, 88, 43, 81, 6, 44, 163, 41, 3, 98, 71, 192, 190, 112, 19, 244, 137, 175, 49, 181, 197, 159, 205, 230, 134, 115, 195, 227, 41, 48, 88, 67, 53, 191, 68, 6, 114, 130, 66, 219, 137, 206, 249, 41, 23, 168, 53, 103, 54, 230, 188, 56, 233, 122, 16, 158, 254, 104, 108, 236, 91, 237, 29, 211, 123, 255, 139, 134, 63, 221, 240, 150, 220, 38, 154, 47, 33, 45, 96, 162, 113, 188, 255, 20, 37, 153, 114, 179, 65, 146, 4, 66, 126, 57, 175, 76, 214, 232, 184, 249, 238, 146, 40, 59, 41, 27, 84, 84, 142, 226, 91, 110, 230, 231, 99, 10, 80, 137, 164, 122, 16, 177, 177, 157, 252, 148, 3, 89, 193, 87, 61, 74, 27, 164, 251, 88, 22, 125, 253, 192, 93, 56, 127, 207, 4, 192, 206, 215, 194, 132, 211, 207, 53, 203, 171, 46, 167, 89, 185, 21, 240, 75, 216, 106, 245, 248, 96, 229, 68, 63, 105, 115, 131, 191, 174, 168, 114, 24, 42, 82, 98, 34, 135, 46, 247, 67, 48, 42, 179, 139, 179, 98, 2, 37, 240, 226, 236, 150, 89, 167, 76, 117, 150, 176, 244, 189, 71, 13, 244, 166, 222, 67, 210, 65, 203 };
                client_pk = payload.Take(32).ToArray();
                Blake2BHasher hasher = new Blake2BHasher();
                hasher.Update(client_pk);
                hasher.Update(server_public_key);
                byte[] nonce = hasher.Finish();
                s = TweetNaCl.CryptoBoxBeforenm(client_pk, server_private_key);
                //byte[] decrypted = TweetNaCl.CryptoBoxOpen(payload.Skip(32).ToArray(), nonce, client_pk, );
                byte[] decrypted = TweetNaCl.CryptoBoxOpenAfternm(payload.Skip(32).ToArray(), nonce, s);
                //byte[] decrypted = { 253, 250, 163, 134, 177, 86, 151, 110, 74, 117, 241, 24, 148, 226, 6, 30, 76, 224, 120, 222, 223, 91, 125, 174, 212, 71, 82, 180, 173, 9, 135, 249, 238, 121, 73, 81, 70, 196, 9, 227, 37, 166, 128, 119, 44, 117, 15, 204, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 0, 48, 0, 0, 0, 1, 0, 0, 1, 70, 0, 0, 0, 40, 98, 102, 56, 101, 100, 97, 53, 56, 97, 54, 99, 49, 52, 98, 100, 48, 55, 51, 100, 97, 100, 53, 54, 100, 55, 101, 50, 101, 52, 54, 48, 97, 101, 101, 55, 51, 52, 100, 50, 52, 0, 0, 0, 6, 73, 78, 50, 48, 50, 53, 1, 1, 0, 0, 0, 5, 122, 104, 45, 67, 78, 0, 0, 0, 2, 49, 50, 1, 0, 0, 0, 0, 0, 0, 0, 16, 49, 49, 50, 53, 54, 98, 52, 98, 57, 48, 57, 98, 100, 51, 52, 55, 0, 0, 0, 0, 0, 0, 0, 5, 180, 2, 0, 0, 0, 6, 52, 56, 46, 51, 50, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 120, 156, 3, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                PepperState = 4;

                RNonce = decrypted.Skip(24).Take(24).ToArray();

                return decrypted.Skip(48).ToArray();
            }
            catch (TweetNaCl.InvalidCipherTextException)
            {
                Console.WriteLine("Failed to decrypt 10101");
                return null;
            }
        }

        private byte[] SendPepperLoginResponse(byte[] payload)
        {
            byte[] packet = new byte[payload.Length + 32 + 24];

            Buffer.BlockCopy(SNonce, 0, packet, 0, 24);
            Buffer.BlockCopy(SecretKey, 0, packet, 24, 32);
            Buffer.BlockCopy(payload, 0, packet, 24 + 32, payload.Length);

            Blake2BHasher hasher = new Blake2BHasher();
            hasher.Update(RNonce);
            hasher.Update(client_pk);
            hasher.Update(server_public_key);
            byte[] nonce = hasher.Finish();
            byte[] encrypted = TweetNaCl.CryptoBoxAfternm(packet, nonce, s);

            PepperState = 5;

            Decrypter = new PepperEncrypter(SecretKey, RNonce);
            Encrypter = new PepperEncrypter(SecretKey, SNonce);

            return encrypted;
        }
    }
}
