using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerEntity : BaseEntity
    {
        public override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.00f))
                {
                    if (hit.transform != null)
                    {
                        PrintName(hit.transform.gameObject);
                    }
                }

                Vector2Int targetPos = CalculateMouseTargetPosition();
                Debug.Log("targetPosition" + targetPos);
                Debug.Log("playerPosition" + CurrentPos);

                TryMoveTo(targetPos);

            }
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

        private void PrintName(GameObject go)
        {
            Debug.Log(go.name);
        }

        private Vector2Int CalculateMouseTargetPosition()
        {
            var mousePosition = Input.mousePosition;
            var transformedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            return new Vector2Int((int)Math.Floor(transformedPosition.x), (int)Math.Floor(transformedPosition.y));
        }
    }

}
