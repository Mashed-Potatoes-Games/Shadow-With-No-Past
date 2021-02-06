using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickMovement : MonoBehaviour
{
    private GridInformation GridInfo;

    private Vector3Int targetPosition;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        GridInfo = GameObject.Find("Main grid").GetComponent<GridInformation>();
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

            CalculateTargetPosition();
            Debug.Log("targetPosition");
            Debug.Log(targetPosition);

            if(CanMoveTo(targetPosition))
            {
                MoveTarget();
            }

        }
    }


    private void PrintName(GameObject go) {
        Debug.Log(go.name);
    }
    private void CalculateTargetPosition() {
        var mousePosition = Input.mousePosition;
        var transformedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        targetPosition = new Vector3Int((int)Math.Floor(transformedPosition.x), (int)Math.Floor(transformedPosition.y), 0);
    }

    private bool CanMoveTo(Vector3Int position)
    {
        return GridInfo.IsGround(position) && !GridInfo.IsObstacle(position);
    }

    private void MoveTarget() {
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        transform.position = new Vector3(targetPosition.x + 0.5f, targetPosition.y + 1.1f);
    }
}
