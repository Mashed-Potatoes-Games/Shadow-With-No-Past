using ShadowWithNoPast.Entities;
using System;
using UnityEngine;

[Serializable]
public struct WorldPos
{
    public World World;
    public Vector2Int Vector;

    public WorldPos(World world, Vector2Int pos)
    {
        World = world;
        Vector = pos;
    }

    public Vector3 WorldCoordinates => World.WorldFromCell(Vector);

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