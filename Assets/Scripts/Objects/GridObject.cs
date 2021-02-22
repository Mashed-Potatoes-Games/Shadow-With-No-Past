using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GridObjects
{
    [RequireComponent(typeof(ObjectEditor))]
    [ExecuteAlways]
    public class GridObject : MonoBehaviour
    {
        public GridManagement GlobalParentGrid;

        //Get: returns global position to the entity.
        //Set: changes the value AND moves the GameObject (Adding the offset values).
        public Vector2Int CurrentPos
        {
            get
            {
                return currentPos;
            }

            set
            {
                currentPos = value;
                SnapToGrid();
            }
        }
        [SerializeField]
        protected Vector2Int currentPos;

        //Entites can be the different size and offsets are used to position the object in the center of the grid.
        //Changing the sprite center to custom fucks up hard with it's flip.
        //This values are for 512x512px sprites, which corresponds to 2x2 units in Unity.
        public virtual float XOffset => 0.5f;
        public virtual float YOffset => 1.1f;


        protected virtual void Awake()
        {
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public virtual void MoveTo(Vector2Int targetPos)
        {
            if(GlobalParentGrid != null)
            {
                GlobalParentGrid.MoveInstantTo(this, targetPos);
            }
            CurrentPos = targetPos;
        }

        public void SnapToGrid()
        {
            transform.position = new Vector3(CurrentPos.x + XOffset, CurrentPos.y + YOffset);
        }
    }

}
