using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class BaseEntity : MonoBehaviour
    {
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        public Vector2Int CurrentPos
        {
            get
            {
                if (currentPos.x != Mathf.RoundToInt(transform.position.x - XOffset))
                {
                    currentPos.x = Mathf.RoundToInt(transform.position.x - XOffset);
                }

                if (currentPos.y != Mathf.RoundToInt(transform.position.y - YOffset))
                {
                    currentPos.y = Mathf.RoundToInt(transform.position.y - YOffset);
                }

                return currentPos;
            }
        }
        protected Vector2Int currentPos;

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
                        throw new System.NotImplementedException();
                }
            }
        }
        protected Direction facing;

        public int MoveDistance = 1;

        protected Camera mainCamera;
        protected GridInformation GridInfo;

        protected virtual float XOffset => 0.5f;
        protected virtual float YOffset => 1.1f;


        public virtual void Start()
        {
            currentPos = new Vector2Int(
                (int)Mathf.RoundToInt(transform.position.x - XOffset),
                (int)Mathf.RoundToInt(transform.position.y - YOffset));

            mainCamera = Camera.main;

            GameObject grid = GameObject.Find("Main grid");
            GridInfo = grid.gameObject.GetComponent<GridInformation>();
        }

        void Update()
        {

        }

        public void TryMoveTo(Vector2Int targetPos)
        {
            if (CanMoveTo(targetPos))
            {
                MoveTo(targetPos);
            }
        }

        //In fact directional one uses coordinates one on the top ^^^
        public void TryMoveTo(Direction direction)
        {
            Vector2Int MovementVector = new Vector2Int(0, 0);
            switch (direction)
            {
                case Direction.Up:
                    MovementVector = new Vector2Int(0, 1);
                    break;
                case Direction.Right:
                    MovementVector = new Vector2Int(1, 0);
                    break;
                case Direction.Down:
                    MovementVector = new Vector2Int(0, -1);
                    break;
                case Direction.Left:
                    MovementVector = new Vector2Int(-1, 0);
                    break;
            }
            Vector2Int TargetPosition = CurrentPos + MovementVector;
            TryMoveTo(TargetPosition);
        } 

        public bool CanMoveTo(Vector2Int targetPos)
        {
            //TODO: move to GridInfo class!
            bool isSpaceAvailable = GridInfo.IsGround(targetPos) && !GridInfo.IsObstacle(targetPos);
            bool canReachTo;
            int xDiff = Mathf.Abs(CurrentPos.x - targetPos.x);
            int yDiff = Mathf.Abs(CurrentPos.y - targetPos.y);

            canReachTo = MoveDistance >= xDiff + yDiff;
            //TODO: make pathfinding through obstacles!

            return isSpaceAvailable && canReachTo;
        }

        public void MoveTo(Vector2Int targetPos)
        {
            Vector2Int direction = targetPos - CurrentPos;
            if(direction.x > 0)
            {
                FaceTo(Direction.Right);
            } else if (direction.x < 0)
            {
                FaceTo(Direction.Left);
            }
            transform.position = new Vector3(targetPos.x + XOffset, targetPos.y + YOffset);
        }

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
            }
        }
    }
}