using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ShadowWithNoPast.Entities
{
    [ExecuteAlways]
    public class GridEntity : GridObject
    {
        protected IMovementController movementController;
        protected ITurnController turnController;

        #region Ability to face, flipping SpriteRenderer

        protected Direction facing;

        public void FaceTo(Vector2Int targetPos)
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
        public void FaceTo(Direction direction)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = direction switch
            {
                Direction.Right => false,
                Direction.Left => true,
                _ => throw new NotImplementedException(),
            };
        }
        #endregion
    }
    /// <summary>
    /// Direction, the entity can face or attack.
    /// </summary>
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}