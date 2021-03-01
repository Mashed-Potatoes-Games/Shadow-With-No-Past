using ShadowWithNoPast.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;

[RequireComponent(typeof(Grid))]
public class WorldManagement : MonoBehaviour
{
    public class CannotMoveHereException : Exception { }

    [SerializeField]
    private ObstacleTilemap obstacles;
    [SerializeField]
    private GroundTilemap ground;
    [SerializeField]
    private ObjectsGrid objects;

    public WorldManagement OtherWorld { get; private set; }

    public const float TileOffset = 0.5f;

    public WorldType Type;

    [SerializeField]
    public bool active { get; private set; }

    //Needed for the editor script
    public bool ShowTileCoordinates = false;

    public void SwitchActiveWorld(bool thisActive = true)
    {

        //TODO Change screen appearance
        active = thisActive;
        OtherWorld.active = !thisActive;
    }

    //Returns, what is at the given coordinate.
    public CellStatus GetCellStatus(Vector2Int pos)
    {
        if (!ground.IsGround(pos))
        {
            return CellStatus.NoGround;
        } 
        
        if (obstacles.IsObstacle(pos))
        {
            return CellStatus.Obstacle;
        }

        return (objects.WhatIn(pos)) switch
        {
            ObjectType.Empty => CellStatus.Free,
            ObjectType.Entity => CellStatus.Entity,
            ObjectType.Item => CellStatus.Item,
            _ => throw new NotImplementedException(),
        };
    }

    //Use it only after ensuring the cell is free
    public void MoveInstantTo(GridObject obj, Vector2Int pos)
    {
        if(GetCellStatus(pos) == CellStatus.Free)
        {
            objects.MoveInstantTo(obj, pos);
        }
        else if(objects.GetEntityAt(pos) != obj)
        {
            throw new CannotMoveHereException();
        }
    }
    /// <summary>
    /// It initiates the chosen objects on the grid.
    /// Don't try to move objects with it, because it won't delete previous object position.
    /// </summary>
    public void SetNewObjectTo(GridObject obj, Vector2Int pos)
    {
        if(GetCellStatus(pos) == CellStatus.Free)
        {
            objects.SetNewObjectTo(obj, pos);
        } else
        {
            throw new CannotMoveHereException();
        }
    }

    public GridObject GetEntityAt(Vector2Int pos)
    {
        return objects.GetEntityAt(pos);
    }

    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        objects.RemoveAt(obj, pos);
    }



    public static Vector2Int CellFromMousePos()
    {
        var mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return CellFromWorld(worldPos);
    }

    public static Vector2Int CellFromWorld(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x - TileOffset), Mathf.RoundToInt(worldPos.y - TileOffset));
    }
}

public enum CellStatus
{
    NoGround,
    Obstacle,
    Free,
    Entity,
    Item
}

public enum WorldType
{
    Regular,
    Dark
}
