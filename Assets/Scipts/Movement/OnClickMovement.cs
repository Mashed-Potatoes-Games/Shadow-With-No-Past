using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickMovement : MonoBehaviour
{
    public Vector2Int CurrentPos
    {
        get
        {
            if(currentPos.x != (int)Math.Floor(transform.position.x))
            {
                currentPos.x = (int)Math.Floor(transform.position.x);
            }

            if (currentPos.y != (int)Math.Floor(transform.position.y))
            {
                currentPos.y = (int)Math.Floor(transform.position.y);
            }

            return currentPos;
        }
    }
    private Vector2Int currentPos;

    public int MoveDistance = 2;

    private Camera mainCamera;
    private GridInformation GridInfo;

    private const float XOffset = 0.5f;
    private const float YOffset = 1.1f;

    void Start()
    {
        currentPos = new Vector2Int(
                (int)Math.Floor(transform.position.x),
                (int)Math.Floor(transform.position.y));

        mainCamera = Camera.main;

        GameObject grid = GameObject.Find("Main grid");
        GridInfo = grid.gameObject.GetComponent<GridInformation>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)){       
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100.00f)) {
                if (hit.transform != null) {
                    PrintName(hit.transform.gameObject);
                }
            }

            Vector2Int targetPos = CalculateTargetPosition();
            Debug.Log("targetPosition");
            Debug.Log(targetPos);

            if(CanMoveTo(targetPos))
            {
                MoveTo(targetPos);
            }

        }
    }

    private void PrintName(GameObject go) {
        Debug.Log(go.name);
    }

    private Vector2Int CalculateTargetPosition() {
        var mousePosition = Input.mousePosition;
        var transformedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        return new Vector2Int((int)Math.Floor(transformedPosition.x), (int)Math.Floor(transformedPosition.y));
    }

    private bool CanMoveTo(Vector2Int targetPos)
    {
        //TODO: move to GridInfo class!
        bool isSpaceAvailable = GridInfo.IsGround(targetPos) && !GridInfo.IsObstacle(targetPos);
        bool canReachTo;
        int xDiff = Math.Abs(CurrentPos.x - targetPos.x);
        int yDiff = Math.Abs(CurrentPos.y - targetPos.y);

        canReachTo = MoveDistance >= xDiff + yDiff;
        //TODO: make pathfinding through obstacles!

        return isSpaceAvailable && canReachTo;
    }

    private void MoveTo(Vector2Int targetPos) {
        transform.position = new Vector3(targetPos.x + XOffset, targetPos.y + YOffset);
    }
}
