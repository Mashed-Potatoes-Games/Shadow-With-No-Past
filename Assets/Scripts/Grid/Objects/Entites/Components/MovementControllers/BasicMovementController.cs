using ShadowWithNoPast.Algorithms;
using ShadowWithNoPast.Entities.Abilities;
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
            world = entity.World;
        }

        public Queue<WorldPos> GetPath(WorldPos targetPos, bool isSearchStrict = true)
        {
            if (!CanCellBePassable(targetPos))
            {
                return null;
            }

            Queue<WorldPos> pathQueue = FindClearPath(entity.GetGlobalPos(), targetPos);

            if (pathQueue is null)
            {
                if (isSearchStrict)
                {
                    return null;
                }

                pathQueue = FindPathThroughEntities(entity.GetGlobalPos(), targetPos);
            }

            if (pathQueue != null && pathQueue.Peek().Equals(entity.GetGlobalPos()))
            {
                pathQueue.Dequeue();
            }

            return pathQueue;
        }

        private bool CanMoveTo(WorldPos targetPos)
        {
            if (entity.World != targetPos.World)
            {
                Debug.LogError("This movement controller doesn't allow moving between worlds!");
                return false;
            }
            return targetPos.GetStatus() == CellStatus.Free;
        }

        //According to direction tries to move to calculated coordinate.
        public virtual bool TryInstantMoveTo(Direction direction)
        {
            Vector2Int MovementVector = CoordinateUtils.GetVectorFromDirection(direction);

            WorldPos TargetPosition = new WorldPos(entity.World, entity.Pos + MovementVector);
            return TryInstantMoveTo(TargetPosition);
        }

        //check for an available space before moving
        public bool TryInstantMoveTo(WorldPos targetPos)
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


        private void InstantMoveTo(WorldPos targetPos)
        {
            entity.FaceTo(targetPos.Vector);
            world.MoveInstantTo(entity, targetPos);
            world.EventManager.EntityMoved.Invoke(entity);
        }

        public Queue<WorldPos> FindClearPath(WorldPos start, WorldPos end)
        {
            if(start.World != end.World)
            {
                throw new NotImplementedException();
            }
            var posQueue = BreadthFirstSearch.FindPath(start, end, IsCellFree);
            return posQueue;
        }

        public bool IsCellFree(WorldPos pos)
        {
            return pos.GetStatus() == CellStatus.Free;
        }

        public bool IsCellFree(Vector2Int pos)
        {
            return world.GetCellStatus(pos) == CellStatus.Free;
        }

        public Queue<WorldPos> FindPathThroughEntities(WorldPos start, WorldPos end)
        {
            return BreadthFirstSearch.FindPath(start, end, CanCellBePassable);
        }

        public bool CanCellBePassable(WorldPos pos)
        {
            CellStatus status = pos.GetStatus();
            bool CanPass = status != CellStatus.NoGround &&
                           status != CellStatus.Obstacle;
            return CanPass;
        }

        public IEnumerator MoveWithDelay(Queue<WorldPos> path)
        {
            int movesLeft = entity.MoveDistance;
            while (path.Count > 0 && movesLeft > 0)
            {
                WorldPos pos = path.Dequeue();
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

        public List<WorldPos> GetAvailableMoves()
        {
            return BreadthFirstSearch.GetAvailableMoves(entity.GetGlobalPos(), entity.MoveDistance, CanMoveTo);
        }
    }
}