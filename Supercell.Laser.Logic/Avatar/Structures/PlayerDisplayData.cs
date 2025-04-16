namespace Supercell.Laser.Logic.Avatar.Structures
{
    using Supercell.Laser.Titan.DataStream;

    public class PlayerDisplayData
    {
        public int ThumbnailId;
        public int NameColorId;
        public string Name;

        public PlayerDisplayData()
        {
            ;
        }

        public PlayerDisplayData(int thumbnail, int namecolor, string name)
        {
            ThumbnailId = thumbnail;
            NameColorId = namecolor;
            Name = name;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteString(Name);
            stream.WriteVInt(5000);
            stream.WriteVInt(ThumbnailId);
            stream.WriteVInt(NameColorId);
            stream.WriteVInt(-1);

        }
    }
}
