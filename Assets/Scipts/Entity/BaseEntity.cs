using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public Vector2Int CurrentPos
        {
            get
            {
                if (currentPos.x != (int)Mathf.Floor(transform.position.x))
                {
                    currentPos.x = (int)Mathf.Floor(transform.position.x);
                }

                if (currentPos.y != (int)Mathf.Floor(transform.position.y))
                {
                    currentPos.y = (int)Mathf.Floor(transform.position.y);
                }

                return currentPos;
            }
        }

        protected Vector2Int currentPos;

        public int MoveDistance = 1;

        protected Camera mainCamera;
        protected GridInformation GridInfo;

        protected const float XOffset = 0.5f;
        protected const float YOffset = 1.1f;


        // Start is called before the first frame update
        public virtual void Start()
        {
            currentPos = new Vector2Int(
                (int)Mathf.Floor(transform.position.x),
                (int)Mathf.Floor(transform.position.y));

            mainCamera = Camera.main;

            GameObject grid = GameObject.Find("Main grid");
            GridInfo = grid.gameObject.GetComponent<GridInformation>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        protected bool CanMoveTo(Vector2Int targetPos)
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

        protected void MoveTo(Vector2Int targetPos)
        {
            transform.position = new Vector3(targetPos.x + XOffset, targetPos.y + YOffset);
        }
    }
}