using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ShadowWithNoPast.GridObjects
{
    [ExecuteAlways]
    public abstract class BaseEntity : GridObject
    {
        //Get: returns the direction this obect is facing to.
        //Set: changes this value AND flips the sprite to this direction
        public Direction Facing
        {
            get
            {
                return facing;
            }
            set
            {
                switch (value)
                {
                    case Direction.Right:
                        facing = value;
                        break;
                    case Direction.Left:
                        facing = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        protected Direction facing;


        public virtual TurnPriority Priority => TurnPriority.Normal;

        //Distance the entity is allowed to move in 1 turn.
        public virtual int MoveDistance => 1;
        public virtual float DelayInSecBetweenCellsMove => 0.1f;

        public Queue<Vector2Int> MovementQueue = new Queue<Vector2Int>();

        protected Queue<Vector2Int> GetPath(Vector2Int targetPos, bool isSearchStrict = true)
        {
            if (!WorldGrid.CanCellBePassable(targetPos))
            {
                return null;
            }

            Queue<Vector2Int> pathQueue = WorldGrid.FindClearPath(currentPos, targetPos);

            if (pathQueue is null)
            {
                if (isSearchStrict)
                {
                    return null;
                }
                else
                {
                    pathQueue = WorldGrid.FindPathThroughEntities(currentPos, targetPos);
                }
            }
            if (pathQueue != null && pathQueue.Peek() == CurrentPos)
            {
                pathQueue.Dequeue();
            }

            return pathQueue;
        }

        public IEnumerator MoveWithDelay(Queue<Vector2Int> path)
        {
            int movesLeft = MoveDistance;
            while(path.Count > 0 && movesLeft > 0)
            {
                Vector2Int pos = path.Dequeue();
                if(!TryInstantMoveTo(pos))
                {
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(DelayInSecBetweenCellsMove);
                    movesLeft--;
                }
            }
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

        //According to direction tries to move to calculated coordinate.
        public virtual bool TryInstantMoveTo(Direction direction)
        {
            Vector2Int MovementVector = GetVectorFromDirection(direction);
            
            Vector2Int TargetPosition = CurrentPos + MovementVector;
            return TryInstantMoveTo(TargetPosition);
        } 

        //Get's movement vector from position, that can be added to current coordinates to find the target position.
        private Vector2Int GetVectorFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Vector2Int(0, 1);
                case Direction.Right:
                    return new Vector2Int(1, 0);
                case Direction.Down:
                    return new Vector2Int(0, -1);
                case Direction.Left:
                    return new Vector2Int(-1, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        public bool CanMoveTo(Vector2Int targetPos)
        {
            bool isSpaceAvailable = WorldGrid.GetCellStatus(targetPos) == CellStatus.Free;
            bool canReachTo;
            int xDiff = Mathf.Abs(CurrentPos.x - targetPos.x);
            int yDiff = Mathf.Abs(CurrentPos.y - targetPos.y);

            canReachTo = MoveDistance >= xDiff + yDiff;
            //TODO: make pathfinding minding obstacles!

            return isSpaceAvailable && canReachTo;
        }

        public override void InstantMoveTo(Vector2Int targetPos)
        {
            //Changes direction, object is facing.
            FaceTo(targetPos);

            base.InstantMoveTo(targetPos);
        }

        protected void FaceTo(GridObject obj)
        {
            FaceTo(obj.CurrentPos);
        }

        protected void FaceTo(Vector2Int targetPos)
        {
            Vector2Int direction = targetPos - CurrentPos;

            if (direction.x > 0)
            {
                FaceTo(Direction.Right);
            }
            else if (direction.x < 0)
            {
                FaceTo(Direction.Left);
            }
        }

        //Change facing direction after a move or other interractions
        protected void FaceTo(Direction direction)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            switch (direction)
            {
                case Direction.Right:
                    spriteRenderer.flipX = false;
                    break;
                case Direction.Left:
                    spriteRenderer.flipX = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        

        //Those default functions should be overrided in child classes.

        //Also trying to call them from other places other than child class is a bad idea,
        //because you can't be sure those will for sure be implemented.
        protected virtual void Attack(Direction direction)
        {
            throw new NotImplementedException();
        }

        protected virtual void Defend()
        {
            throw new NotImplementedException();
        }


        //This functions should be used from controlling class to let the entity make it's move. 
        //It should always be implemented except of player.

        public abstract IEnumerator PrepareAndTelegraphMove();
        public abstract IEnumerator ExecuteMove();
    }
    public enum TurnPriority
    {
        First,
        High,
        Normal,
        Low,
        Last,
        Player
    }

    //Direction, the entity can face or attack.
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}