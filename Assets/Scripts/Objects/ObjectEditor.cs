using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ShadowWithNoPast.GridObjects;
using System;

[ExecuteInEditMode]
//This script is needed be applied to every Entity or Item prefab, so you could move them only on grid coordinates
public class ObjectEditor : MonoBehaviour
{
    GridObject Object;

    public bool IsConnectedToGrid = false;

    void Awake()
    {
        Object = GetComponent<GridObject>();
        if(Object is null)
        {
            DestroyImmediate(this, false);
        }
    }

    //Every time editor interracts with the object it calls Update.
    //It will always force rewrite the position, so it will always be snapped to a grid.
    void Update()
    {
        //This prevents the script from running in Game vindow, while still in editor
        if(!Application.isPlaying && IsConnectedToGrid)
        {
            SnapToGrid();
            EditorUtility.SetDirty(Object);
        }
    }

    void OnDestroy()
    {
        if (!Application.isPlaying && 
            IsConnectedToGrid && 
            Object.GlobalParentGrid.GetEntityAt(Object.CurrentPos) == Object)
        {
            Object.GlobalParentGrid.RemoveAt(Object, Object.CurrentPos);
        }
    }


    private void SnapToGrid()
    {
        GridManagement ParentGrid = Object.GlobalParentGrid;
        //Gets the position in the grid from the position that editor tries to set.
        Vector2Int snapPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x - Object.XOffset),
            Mathf.RoundToInt(transform.position.y - Object.YOffset));

        //If you can't place object here, you want to keep it in previous place.
        if (ParentGrid.GetCellStatus(snapPosition) != GridManagement.CellStatus.Free)
        {
            Object.SnapToGrid();
        }
        else
        {
            ParentGrid.MoveInstantTo(Object, snapPosition);
        }
    }
}

