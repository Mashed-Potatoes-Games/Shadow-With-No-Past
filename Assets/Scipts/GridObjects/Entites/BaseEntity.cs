using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowWithNoPast.GridObjects
{
    [ExecuteAlways]
    public abstract class BaseEntity : GridObject
    {

        //Direction, the entity can face or attack.
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        

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

        //Distance the entity is allowed to move in 1 turn.
        public int MoveDistance = 1;

        
        protected GridManagement GridInfo;

        protected override void Awake()
        {
            base.Awake();
        }

        //ToDo: Don't work with EntitiesGrid, and work with GridInfo instead!!!
        protected override void Start()
        {
            base.Start();
        }

        //Usual enemies don't need to do anything at each frame, so it's blank.
        //It's virtual in case you will need to override it in innerhited entities(animations or so)
        protected override void Update()
        {
            base.Update();
        }

        //check for an available space before moving
        public void TryMoveTo(Vector2Int targetPos)
        {
            if (CanMoveTo(targetPos))
            {
                MoveTo(targetPos);
            }
        }

        //According to direction tries to move to calculated coordinate.
        public virtual void TryMoveTo(Direction direction)
        {
            Vector2Int MovementVector = GetPosFromDirection(direction);
            
            Vector2Int TargetPosition = CurrentPos + MovementVector;
            TryMoveTo(TargetPosition);
        } 

        //Get's movement vector from position, that can be added to current coordinates to find the target position.
        private Vector2Int GetPosFromDirection(Direction direction)
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
                    throw new Exception();
            }
        }

        public bool CanMoveTo(Vector2Int targetPos)
        {
            //TODO: move to GridInfo class!
            bool isSpaceAvailable = GlobalParentGrid.GetCellStatus(targetPos) == GridManagement.CellStatus.Free;
            bool canReachTo;
            int xDiff = Mathf.Abs(CurrentPos.x - targetPos.x);
            int yDiff = Mathf.Abs(CurrentPos.y - targetPos.y);

            canReachTo = MoveDistance >= xDiff + yDiff;
            //TODO: make pathfinding minding obstacles!

            return isSpaceAvailable && canReachTo;
        }

        public override void MoveTo(Vector2Int targetPos)
        {
            Vector2Int direction = targetPos - CurrentPos;

            if(direction.x > 0)
            {
                FaceTo(Direction.Right);
            } 
            else if (direction.x < 0)
            {
                FaceTo(Direction.Left);
            }

            base.MoveTo(targetPos);
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
                    throw new System.NotImplementedException();
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

        //This function should be used from controlling class to let the entity make it's move. 
        //It should always be implemented except of player.
        public abstract void MakeTurn();
    }
}