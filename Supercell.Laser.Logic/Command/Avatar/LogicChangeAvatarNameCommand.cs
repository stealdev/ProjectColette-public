﻿namespace Supercell.Laser.Logic.Command.Avatar
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicChangeAvatarNameCommand : Command
    {
        public string Name;
        public int ChangeNameCost;

        public override void Encode(ByteStream stream)
        {
            stream.WriteString(Name);
            stream.WriteVInt(ChangeNameCost);
        }

        public override int Execute(HomeMode homeMode)
        {
            homeMode.Avatar.Name = Name;
            homeMode.Avatar.NameSetByUser = true;
            if (homeMode.Avatar.Friends.Count > 0)
            {
                foreach(var friend in homeMode.Avatar.Friends)
                {
                    if (friend.Avatar.Friends.Find(x => x.AccountId == homeMode.Avatar.AccountId) == null) continue;
                    friend.Avatar.Friends.Find(x => x.AccountId == homeMode.Avatar.AccountId).DisplayData = new Logic.Avatar.Structures.PlayerDisplayData(homeMode.Home.ThumbnailId, homeMode.Home.NameColorId, homeMode.Avatar.Name);
                }
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 201;
        }
    }
}
