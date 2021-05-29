using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [ExecuteAlways]
    public class GridObject : MonoBehaviour
    {

        public event Action<GridObject, WorldPos, WorldPos> Moved;

        [field: SerializeField]
        public WorldPos Pos { get; private set; }
        public World World => Pos.World;
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
        public void SetNewPosition(WorldPos newPos) 
        {
            var oldPos = Pos;
            Pos = newPos;
            SnapToGrid();
            Moved?.Invoke(this, oldPos, newPos);
        }

        //Entites can be the different size and offsets are used to position the object in the center of the grid.
        //Changing the sprite center to custom fucks up hard with it's flip.
        //This values are for 512x512px sprites, which corresponds to 2x2 units in Unity.
        //Can (and may) be changed after changing tiles to be tied to a center, rather than a corner
        [field: SerializeField]
        public virtual float XOffset { get; set; } = 0.5f;
        [field: SerializeField]
        public virtual float YOffset { get; set; } = 1.1f;
        [field: SerializeField]
        public virtual float ZValue { get; set; } = 0f;

        #region Empty Awake, Start and Update ready to be overriden.
        // Made in case global objects logic is needed.
        protected virtual void Awake() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        #endregion


        public void SnapToGrid()
        {
            transform.localPosition = new Vector3(Vector.x + XOffset, Vector.y + YOffset, ZValue);
        }
    }

}
