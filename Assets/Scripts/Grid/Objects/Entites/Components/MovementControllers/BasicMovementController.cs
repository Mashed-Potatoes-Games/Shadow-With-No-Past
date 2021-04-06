using ShadowWithNoPast.Algorithms;
using ShadowWithNoPast.Utils;
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

        private float delayInSecBetweenCellsMove = 0.1f;

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

            Queue<Vector2Int> pathQueue = FindClearPath(entity.Pos, targetPos);

            if (pathQueue is null)
            {
                if (isSearchStrict)
                {
                    return null;
                }

                pathQueue = FindPathThroughEntities(entity.Pos, targetPos);
            }

            if (pathQueue != null && pathQueue.Peek() == entity.Pos)
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
            Vector2Int MovementVector = CoordinateUtils.GetVectorFromDirection(direction);

            Vector2Int TargetPosition = entity.Pos + MovementVector;
            return TryInstantMoveTo(TargetPosition);
        }


        private void InstantMoveTo(Vector2Int targetPos)
        {
            entity.FaceTo(targetPos);
            world.MoveInstantTo(entity, targetPos);
            world.EventManager.EntityMoved.Invoke(entity);
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

        public IEnumerator MoveWithDelay(Queue<Vector2Int> path)
        {
            int movesLeft = entity.MoveDistance;
            while (path.Count > 0 && movesLeft > 0)
            {
                Vector2Int pos = path.Dequeue();
                if (!TryInstantMoveTo(pos))
                {
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(delayInSecBetweenCellsMove);
                    movesLeft--;
                }
            }
        }
    }
}