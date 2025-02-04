namespace Supercell.Laser.Logic.Message.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;

    public class OwnHomeDataMessage : GameMessage
    {
        public OwnHomeDataMessage() : base()
        {
            ;
        }

        public ClientHome Home;
        public ClientAvatar Avatar;

        public override void Encode()
        {
        //Stream.WriteVInt(2021122);
        //Stream.WriteVInt(75735);  
        //Stream.WriteVInt(5000);
        //Stream.WriteVInt(5000); 
        
        //Stream.WriteVInt(100);
        //Stream.WriteVInt(97); 
        //Stream.WriteVInt(5000);

        //    //Stream.WriteDataReference(28, 0);  
        //    //Stream.WriteDataReference(43, 0);  

        //    Stream.WriteVInt(28);
        //    Stream.WriteVInt(0);
        //    Stream.WriteVInt(43);
        //    Stream.WriteVInt(0);

        //Stream.WriteVInt(0);   
        //Stream.WriteVInt(0);   
        //Stream.WriteVInt(0);   
        
        //Stream.WriteVInt(0);

        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteByte(0);

        //Stream.WriteVInt(1000);
        //Stream.WriteVInt(10);
        //Stream.WriteVInt(20);
        //Stream.WriteVInt(30);

        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);   
        
        //Stream.WriteByte(0); 
        
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);


        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(200);
        //Stream.WriteVInt(0);


        //Stream.WriteVInt(0);   
        
        //Stream.WriteVInt(99999);
        //Stream.WriteVInt(0);


        //    Stream.WriteVInt(16);
        //    Stream.WriteVInt(0);

        //Stream.WriteString("RU");
        //Stream.WriteString("ServerBSvvv"); 
        
        //Stream.WriteVInt(1);    
        //Stream.WriteInt(0);
        //Stream.WriteInt(0);


        //Stream.WriteVInt(1);    
        //Stream.WriteVInt(0);
        //    Stream.WriteVInt(16);
        //    Stream.WriteVInt(0);
        //Stream.WriteVInt(0);


        //Stream.WriteVInt(1);    
        
        //    Stream.WriteVInt(4);
        //    Stream.WriteVInt(0);
        //    Stream.WriteByte(0);
        //    Stream.WriteVInt(1);
        //    Stream.WriteByte(0);


        //Stream.WriteVInt(0);   
        
        //Stream.WriteByte(1);
        //Stream.WriteVInt(0);


        //Stream.WriteByte(1);
        //Stream.WriteVInt(0);


        //Stream.WriteVInt(0);  
        
        //Stream.WriteVInt(0);


        //Stream.WriteVInt(1);
        //    //Stream.WriteVInt(Home.Events.Length);
        //    //for (int i = 0; i < Home.Events.Length; i++)
        //    //{
        //    //    Home.Events[i].Encode(Stream);
        //    //}
        //    //Stream.WriteVInt(0);
        //    Stream.WriteVInt(0);
        //Stream.WriteVInt(1);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(75992);
        //Stream.WriteVInt(10);
        //    Stream.WriteVInt(15);
        //    Stream.WriteVInt(7);
        //Stream.WriteVInt(3);
        //Stream.WriteVInt(3);
        //Stream.WriteString("TID_WEEKEND_EVENT");
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);  
        //Stream.WriteVInt(0);
        //Stream.WriteVInt(0);
        //Stream.WriteByte(0);  
        //Stream.WriteVInt(0);
        
        //Stream.WriteVInt(0); 
        
        //Stream.WriteVInt(0);   
        //Stream.WriteVInt(0);   
        //Stream.WriteVInt(0);   
        //Stream.WriteVInt(0);   
        
        //Stream.WriteByte(0);


        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(0);    
        
        //Stream.WriteByte(0);  
        
        //Stream.WriteInt(0);
        //Stream.WriteInt(1);


        //Stream.WriteVInt(0); 
        //Stream.WriteVInt(0);  
        
        //Stream.WriteByte(0);  
        //Stream.WriteVInt(0);    
        
        //Stream.WriteVInt(0);

        Home.Encode(Stream);
        Avatar.Encode(Stream);



        Stream.WriteVInt(0);
        }

        public override int GetMessageType()
        {
            return 24101;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
