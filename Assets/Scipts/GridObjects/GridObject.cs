using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GridObjects
{
    [ExecuteAlways]
    public class GridObject : MonoBehaviour
    {
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
                transform.position = new Vector3(value.x + XOffset, value.y + YOffset);

                currentPos = value;
            }
        }
        protected Vector2Int currentPos;

        //Entites can be the different size and offets are used to position the object in the center of the grid.
        //Changing the sprite center to custom fucks up hard with it's flip.
        //This values are for 512x512px sprites, which corresponds to 2x2 units in Unity.
        public virtual float XOffset => 0.5f;
        public virtual float YOffset => 1.1f;


        protected virtual void Awake()
        {
            currentPos = new Vector2Int(
                Mathf.RoundToInt(transform.position.x - XOffset),
                Mathf.RoundToInt(transform.position.y - YOffset));
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
            CurrentPos = targetPos;
        }
    }

}
