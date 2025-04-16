﻿global using Supercell.Laser.Logic.Data.Helper;
global using Supercell.Laser.Logic.Data.Reader;

using System.Linq;

namespace Supercell.Laser.Logic.Data
{


    public enum DataType
    {
        Projectile = 6,
        AllianceBadge = 8,
        Location = 15,
        Character = 16,
        AreaEffect = 17,
        Item = 18,
        Map = 19,
        Skill = 20,
        Card = 23,
        Tile = 27,
        PlayerThumbnail = 28,
        Skin = 29,
        Milestone = 39,
        NameColor = 43,

        SkinConf = 44,
        Accessory = 50,
        Emote = 52,
        Gear = 62,
        Titul = 76,

        Mastery = 75,
        MasteryVanity = 74,
        Spray = 68,
    }

    public partial class DataTables
    {
        public static Dictionary<DataType, Type> DataTypes = new Dictionary<DataType, Type>();
        public static Dictionary<Type, DataType> Types;

        static DataTables()
        {
            DataTypes.Add(DataType.Projectile, typeof(ProjectileData));
            DataTypes.Add(DataType.AllianceBadge, typeof(AllianceBadgeData));
            DataTypes.Add(DataType.Location, typeof(LocationData));
            DataTypes.Add(DataType.Character, typeof(CharacterData));
            DataTypes.Add(DataType.AreaEffect, typeof(AreaEffectData));
            DataTypes.Add(DataType.Item, typeof(ItemData));
            DataTypes.Add(DataType.Map, typeof(MapData));
            DataTypes.Add(DataType.Skill, typeof(SkillData));
            DataTypes.Add(DataType.Card, typeof(CardData));
            DataTypes.Add(DataType.Tile, typeof(TileData));
            DataTypes.Add(DataType.PlayerThumbnail, typeof(PlayerThumbnailData));
            DataTypes.Add(DataType.Skin, typeof(SkinData));
            DataTypes.Add(DataType.Milestone, typeof(MilestoneData));
            DataTypes.Add(DataType.SkinConf, typeof(SkinConfData));
            DataTypes.Add(DataType.Accessory, typeof(AccessoryData));
            DataTypes.Add(DataType.Emote, typeof(EmoteData));
            DataTypes.Add(DataType.Gear, typeof(GearData));
            DataTypes.Add(DataType.Titul, typeof(TitlesData));
            DataTypes.Add(DataType.Mastery, typeof(MasteryData));
            DataTypes.Add(DataType.MasteryVanity, typeof(MasteryVanityData));
            DataTypes.Add(DataType.Spray, typeof(SprayData));

            Types = DataTypes.ToDictionary(x => x.Value, x => x.Key);
        }

        public static LogicData Create(DataType file, Row row, DataTable dataTable)
        {
            if (DataTypes.ContainsKey(file)) return Activator.CreateInstance(DataTypes[file], row, dataTable) as LogicData;

            return null;
        }
    }
}