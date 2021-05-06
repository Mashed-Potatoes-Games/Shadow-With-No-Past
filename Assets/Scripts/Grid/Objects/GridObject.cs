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

        public event Action<GridObject> Moved;

        [field: SerializeField]
        public WorldPos Pos { get; private set; }
        public WorldManagement World => Pos.World;
        public Vector2Int Vector => Pos.Vector;
        
        

        /// <summary>
        /// This will only set the position of the object relative to the world grid.
        /// To move objects properly use IMovementController, which works with world grid or with world itself.
        /// Using it in other components will inevitably lead to bugs.
        /// </summary>
        public void SetNewPosition(Vector2Int pos)
        {
            SetNewPosition(new WorldPos(Pos.World, pos));
        }

        /// <summary>
        /// This will only set the position of the object relative to the grid.
        /// To move objects properly use IMovementController, which works with world grid or with world itself.
        /// Using it in other components will inevitably lead to bugs.
        /// </summary>
        public void SetNewPosition(WorldPos pos) 
        {
            Pos = pos;
            SnapToGrid();
            Moved?.Invoke(this);
        }

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
        // Made in case global objects logic is needed.
        protected virtual void Awake() { }

        // Start is called before the first frame update
        protected virtual void Start() { }

        // Update is called once per frame
        protected virtual void Update() { }
        #endregion


        public void SnapToGrid()
        {
            transform.localPosition = new Vector3(Vector.x + XOffset, Vector.y + YOffset, ZValue);
        }
    }

}
