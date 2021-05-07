using ShadowWithNoPast.Entities;
using System;
using UnityEngine;

namespace ShadowWithNoPast.Utils
{
    public static class CoordinateUtils
    {
        public static Vector2Int GetVectorFromDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Right => Vector2Int.right,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                _ => throw new NotImplementedException(),
            };
        }

        public static Direction? GetDirectionFromVector(Vector2Int vector)
        {
            
            if(vector.x == 0)
            {
                if(vector.y > 0)
                {
                    return Direction.Up;
                }
                if(vector.y < 0)
                {
                    return Direction.Down;
                }
            }

            if(vector.y == 0)
            {
                if(vector.x > 0)
                {
                    return Direction.Right;
                }
                if(vector.x < 0)
                {
                    return Direction.Left;
                }
            }
            return null;
        }

        public static Vector2Int RotateFromUpDirectionTo(Vector2Int pos, Direction direction)
        {
            return direction switch
            {
                Direction.Down => new Vector2Int(-pos.x, -pos.y),
                Direction.Right => new Vector2Int(pos.y, -pos.x),
                Direction.Left => new Vector2Int(-pos.y, pos.x),
                _ => pos,
            };
        }

        public static Direction[] AllDirections()
        {
            return (Direction[])Enum.GetValues(typeof(Direction));
        }
    }
}