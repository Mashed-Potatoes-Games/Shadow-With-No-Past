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
                Direction.Up => new Vector2Int(0, 1),
                Direction.Right => new Vector2Int(1, 0),
                Direction.Down => new Vector2Int(0, -1),
                Direction.Left => new Vector2Int(-1, 0),
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
                if(vector.x > 0)
                {
                    return Direction.Left;
                }
            }
            return null;
        }

        public static Vector2Int RotateFromUpDirectionTo(Vector2Int pos, Direction direction)
        {
            switch (direction) {
                case Direction.Down:
                    return new Vector2Int(-pos.x, -pos.y);
                case Direction.Right:
                    return new Vector2Int(pos.y, -pos.x);
                case Direction.Left:
                    return new Vector2Int(-pos.y, pos.x);
                default:
                    return pos;
            }
        }
    }
}