using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entites;
[ExecuteInEditMode]
//This Editor script can be applied only to Grid objects
[RequireComponent(typeof(GridObject))]
//This script is needed be applied to every Entity or Item prefab, so you could move them only on grid coordinates
public class ObjectEditor : MonoBehaviour
{
    GridObject Object;
    void Start()
    {
        Object = GetComponent<GridObject>();
    }

    //Every time editor interracts with the object it calls Update.
    void Update()
    {
        //It will always force rewrite the position, so it will always be snapped to a grid.
        Vector2Int snapPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x - Object.XOffset),
            Mathf.RoundToInt(transform.position.y - Object.YOffset));
        Object.CurrentPos = snapPosition;
    }
}
