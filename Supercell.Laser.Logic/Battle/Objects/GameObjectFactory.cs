namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public static class GameObjectFactory//+32 type 64 encode 48IsAlive
    {
        public static Character CreateGameObjectByData(CharacterData characterData)
        {
            return new Character(characterData);
        }
        public static Projectile CreateGameObjectByData(ProjectileData characterData)
        {
            return new Projectile(characterData);
        }
        public static Item CreateGameObjectByData(ItemData characterData)
        {
            return new Item(characterData);
        }
        public static AreaEffect CreateGameObjectByData(AreaEffectData characterData)
        {
            return new AreaEffect(characterData);
        }
    }
}
