using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickMovement : MonoBehaviour
{
    // [SerializeField] private float movementSpeed;
    // private Vector3 targetPosition;
    // private Camera mainCamera;
    
    void Start()
    {
        // mainCamera = Camera.main;
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
        }

        // if(Input.GetMouseButtonDown(0)) {
        //     CalculateTargetPosition();
        //     Debug.Log("targetPosition");
        //     Debug.Log(targetPosition);
        // }

        // MoveTarget();
    }


    private void PrintName(GameObject go) {
        Debug.Log(go.name);
    }
    // private void CalculateTargetPosition() {
        
    //     var mousePosition = Input.mousePosition;
    //     var transformedPosition = mainCamera.ScreenToWorldPoint(mousePosition);
    //     targetPosition = new Vector3(transformedPosition.x, transformedPosition.y, 0);
    // }

    // private void MoveTarget() {
    //     transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
    // }
}
