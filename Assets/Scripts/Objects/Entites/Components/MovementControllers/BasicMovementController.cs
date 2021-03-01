using ShadowWithNoPast.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class BasicMovementController : MonoBehaviour, IMovementController
    {
        private GridEntity entity;
        private WorldManagement world;

        void Start()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
        }

        public Queue<Vector2Int> GetPath(Vector2Int targetPos, bool isSearchStrict = true)
        {
            if (!CanCellBePassable(targetPos))
            {
                return null;
            }

            Queue<Vector2Int> pathQueue = FindClearPath(entity.CurrentPos, targetPos);

            if (pathQueue is null)
            {
                if (isSearchStrict)
                {
                    return null;
                }

                pathQueue = FindPathThroughEntities(entity.CurrentPos, targetPos);
            }

            if (pathQueue != null && pathQueue.Peek() == entity.CurrentPos)
            {
                pathQueue.Dequeue();
            }

            return pathQueue;
        }

        //check for an available space before moving
        public bool TryInstantMoveTo(Vector2Int targetPos)
        {
            if (CanMoveTo(targetPos))
            {
                InstantMoveTo(targetPos);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanMoveTo(Vector2Int targetPos)
        {
            return world.GetCellStatus(targetPos) == CellStatus.Free;
        }

        //According to direction tries to move to calculated coordinate.
        public virtual bool TryInstantMoveTo(Direction direction)
        {
            Vector2Int MovementVector = GetVectorFromDirection(direction);

            Vector2Int TargetPosition = entity.CurrentPos + MovementVector;
            return TryInstantMoveTo(TargetPosition);
        }

        //Get's movement vector from position, that can be added to current coordinates to find the target position.
        private Vector2Int GetVectorFromDirection(Direction direction)
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

        private void InstantMoveTo(Vector2Int targetPos)
        {
            entity.FaceTo(targetPos);
            world.MoveInstantTo(entity, targetPos);
        }

        public Queue<Vector2Int> FindClearPath(Vector2Int start, Vector2Int end)
        {
            return BreadthFirstSearch.FindPath(start, end, IsCellFree);
        }

        public bool IsCellFree(Vector2Int pos)
        {
            return world.GetCellStatus(pos) == CellStatus.Free;
        }

        public Queue<Vector2Int> FindPathThroughEntities(Vector2Int start, Vector2Int end)
        {
            return BreadthFirstSearch.FindPath(start, end, CanCellBePassable);
        }

        public bool CanCellBePassable(Vector2Int pos)
        {
            CellStatus status = world.GetCellStatus(pos);
            bool CanPass = status != CellStatus.NoGround &&
                           status != CellStatus.Obstacle;
            return CanPass;
        }
    }
}