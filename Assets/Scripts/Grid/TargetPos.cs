using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    public struct TargetPos
    {
        public WorldManagement World;
        public Vector2Int Pos;

        public TargetPos(WorldManagement world, Vector2Int pos)
        {
            World = world;
            Pos = pos;
        }
        public GridEntity GetEntity()
        {
            return World.GetEntityAt(Pos);
        }
    }
}