using ShadowWithNoPast.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;
using UnityEngine.Rendering;
using System.Collections;
using ShadowWithNoPast.Utils;
using ShadowWithNoPast.Entities.Abilities;

[ExecuteAlways]
[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(WorldEventManager))]
public class WorldManagement : MonoBehaviour
{
    public class CannotMoveHereException : Exception { }

    public event Action<WorldManagement> Activated;
    public event Action<WorldManagement> Inactivated;

    public event Action<GridObject> ObjectAdded;
    public event Action<GridObject> ObjectRemoved;

    public WorldType Type;

    [SerializeField]
    private ObstacleTilemap obstacles;
    [SerializeField]
    private GroundTilemap ground;
    [SerializeField]
    private ObjectsGrid objects;

    public WorldEventManager EventManager;

    public AbilitiesTargetsAccounter AttacksAccounter;


    public const float TileOffset = 0.5f;


     public bool Active { get => active; private set => active = value; }
    [SerializeField]
    private bool active;

    //Needed for the editor script
    public bool ShowTileCoordinates = false;

    public void Awake()
    {
        EventManager ??= GetComponent<WorldEventManager>();

        obstacles ??= GetComponentInChildren<ObstacleTilemap>();
        ground ??= GetComponentInChildren<GroundTilemap>();
        objects ??= GetComponentInChildren<ObjectsGrid>();
    }
    private void Start()
    {
        AttacksAccounter = new AbilitiesTargetsAccounter(this);
    }

    public void SetActive()
    {
        Active = true;

        const string activeWorldLayerName = "FrontWorld/";
        SwitchAllRenderersToMainLayer(activeWorldLayerName);

        Activated?.Invoke(this);
    }

    public void SetInactive()
    {
        Active = false;

        const string inactiveWorldLayerName = "BackWorld/";
        SwitchAllRenderersToMainLayer(inactiveWorldLayerName);

        Inactivated?.Invoke(this);
    }

    public List<GridEntity> GetEntities() => objects.GetEntities();
    public List<GridObject> GetObjects() => objects.GetObjects();

    private void SwitchAllRenderersToMainLayer(string worldLayerName)
    {
        var renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in renderers)
        {
            RendererUtil.ChangeRenderToLayer(renderer, worldLayerName);
        }
    }

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
        if (GetCellStatus(pos) == CellStatus.Free)
        {
            objects.MoveInstantTo(obj, pos);
        }
        else if (objects.GetEntityAt(pos) != obj)
        {
            throw new CannotMoveHereException();
        }
    }

    //Use it only after ensuring the cell is free
    public void MoveInstantTo(GridObject obj, WorldPos target)
    {
        if(target.World == this)
        {
            MoveInstantTo(obj, target.Vector);
            return;
        }

        RemoveAt(obj, target.Vector);
        target.World.SetNewObjectTo(obj, target.Vector);
    }

    /// <summary>
    /// It initiates the chosen objects on the grid.
    /// Don't try to move objects with it, because it won't delete previous object position.
    /// </summary>
    public void SetNewObjectTo(GridObject obj, Vector2Int pos)
    {
        if (GetCellStatus(pos) == CellStatus.Free)
        {
            objects.SetNewObjectTo(obj, pos);
            ObjectAdded?.Invoke(obj);
            return;
        }

        throw new CannotMoveHereException();
    }

    public GridEntity GetEntityAt(Vector2Int pos)
    {
        return objects.GetEntityAt(pos);
    }

    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        objects.RemoveAt(obj, pos);
        ObjectRemoved?.Invoke(obj);
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
    TheEdge
}
