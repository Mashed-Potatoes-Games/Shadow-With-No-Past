using ShadowWithNoPast.Entities;
using UnityEngine;

public struct WorldPos
{
    public WorldManagement World;
    public Vector2Int Vector;

    public WorldPos(WorldManagement world, Vector2Int pos)
    {
        World = world;
        Vector = pos;
    }

    public CellStatus GetStatus() => World.GetCellStatus(Vector);
    public GridEntity GetEntity()
    {
        return World.GetEntityAt(Vector);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator  ==(WorldPos a, WorldPos b) 
    {
        return a.World == b.World && a.Vector == b.Vector;
    }

    public static bool operator !=(WorldPos a, WorldPos b)
    {
        return !(a == b);
    }

    public static WorldPos operator +(WorldPos a, Vector2Int b)
    {
        return new WorldPos(a.World, a.Vector += b);
    }
}