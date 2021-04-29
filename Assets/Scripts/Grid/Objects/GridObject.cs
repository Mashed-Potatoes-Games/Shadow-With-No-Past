using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(ObjectEditor))]
    [ExecuteAlways]
    public class GridObject : MonoBehaviour
    {
        public WorldManagement World;

        //Get: returns global position to the entity.
        //Set: changes the value AND moves the GameObject (Adding the offset values).
        public Vector2Int Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
                SnapToGrid();
            }
        }
        [SerializeField]
        protected Vector2Int pos;

        //Entites can be the different size and offsets are used to position the object in the center of the grid.
        //Changing the sprite center to custom fucks up hard with it's flip.
        //This values are for 512x512px sprites, which corresponds to 2x2 units in Unity.
        [field: SerializeField]
        public virtual float XOffset { get; set; } = 0.5f;
        [field: SerializeField]
        public virtual float YOffset { get; set; } = 1.1f;
        [field: SerializeField]
        public virtual float ZValue { get; set; } = 0f;

        #region Empty Awake, Start and Update ready to be overriden.
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
        #endregion

        internal WorldPos GetGlobalPos()
        {
            return new WorldPos(World, Pos);
        }

        public void SnapToGrid()
        {
            transform.localPosition = new Vector3(Pos.x + XOffset, Pos.y + YOffset, ZValue);
        }
    }

}
