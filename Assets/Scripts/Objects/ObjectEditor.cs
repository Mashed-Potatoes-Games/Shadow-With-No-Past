// In release version this script will not be loaded at all.
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ShadowWithNoPast.Entities;
using System;


[ExecuteInEditMode]
/// <summary>
/// This script allows objects to me moved in grid, and changes ObjectsGrid to know the position changed.
/// it will always load with GridObject component.
/// </summary>
public class ObjectEditor : MonoBehaviour
{
    GridObject GridObj;

    public bool IsConnectedToGrid = false;

    void Awake()
    {
        GridObj = GetComponent<GridObject>();

        // This should always be added with GridObject.
        // Grid object depends on it, so it will always be added on it, but if we try to add it manually, it will kill itself.
        if (GridObj is null)
        {
            DestroyImmediate(this, false);
        }
    }

    // Every time editor interracts with the object it calls Update.
    // It will always force rewrite the position, so it will always be snapped to a grid.
    void Update()
    {
        // This prevents the script from running in Game vindow, while still in editor, or while object is not tied to a grid yet.
        if(!Application.isPlaying && IsConnectedToGrid)
        {
            SnapToGrid();
            // This tells editor that GridObj fields was overriden dirty, and it needs to save the changes.
            EditorUtility.SetDirty(GridObj);
        }
    }

    // Before this object is destroyed, remove itself from the grid.
    void OnDestroy()
    {
        if (!Application.isPlaying && 
            IsConnectedToGrid && 
            GridObj.WorldGrid.GetEntityAt(GridObj.CurrentPos) == GridObj)
        {
            GridObj.WorldGrid.RemoveAt(GridObj, GridObj.CurrentPos);
        }
    }


    private void SnapToGrid()
    {
        WorldManagement ParentGrid = GridObj.WorldGrid;
        // Gets the position in the grid from the position that editor tries to set.
        Vector2Int snapPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x - GridObj.XOffset),
            Mathf.RoundToInt(transform.position.y - GridObj.YOffset));

        // If you can't place object here, you want to keep it in previous place.
        if (ParentGrid.GetCellStatus(snapPosition) != CellStatus.Free)
        {
            GridObj.SnapToGrid();
        }
        else
        {
            ParentGrid.MoveInstantTo(GridObj, snapPosition);
        }
    }
}

#endif