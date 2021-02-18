using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entites
{
    public class PlayerEntity : BaseEntity
    {
        //Here should be unique initialization of player
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            //Look for mouse click to move.
            //TODO: Try to move this functionality to scriptable tiles!!
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out hit, 100.00f);

                Vector2Int targetPos = CalculateMouseTargetPosition();
                Debug.Log("targetPosition" + targetPos);
                Debug.Log("playerPosition" + CurrentPos);

                TryMoveTo(targetPos);

            }

            //Check for unputes to move or attack adjacent fields.
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMoveTo(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMoveTo(Direction.Right);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMoveTo(Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMoveTo(Direction.Left);
            }
        }

        //This code won't be necessary after changing click catching to scriptable tiles
        private Vector2Int CalculateMouseTargetPosition()
        {
            var mousePosition = Input.mousePosition;
            var transformedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            return new Vector2Int((int)Math.Floor(transformedPosition.x), (int)Math.Floor(transformedPosition.y));
        }

        //This function should never be called, because player is controlled by user.
        public override void MakeTurn()
        {
            throw new NotImplementedException();
        }
    }

}
