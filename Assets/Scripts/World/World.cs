using ShadowWithNoPast.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;
using UnityEngine.Rendering;
using System.Collections;
using ShadowWithNoPast.Utils;
using ShadowWithNoPast.Entities.Abilities;
using ShadowWithNoPast.Grid;

[ExecuteAlways]
[RequireComponent(typeof(Grid))]
public class World : MonoBehaviour
{
    public class CannotMoveHereException : Exception { }

    public event Action<World> Activated;
    public event Action<World> Inactivated;

    public event Action<GridObject, WorldPos> ObjectAdded;
    public event Action<GridObject, WorldPos> ObjectRemoved;

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
    public const string INACTIVE_WORLD_RENDERER_PREFIX = "BackWorld/";
    public const string ACTIVE_WORLD_RENDERER_PREFIX = "FrontWorld/";

    public bool Active { get => active; private set => active = value; }
    public string RendererPrefix { get => Active ? ACTIVE_WORLD_RENDERER_PREFIX : INACTIVE_WORLD_RENDERER_PREFIX; }

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

    public void SetActive(bool state)
    {
        Active = state;

        if(state)
        {
            Activated?.Invoke(this);
        }
        else
        {
            Inactivated?.Invoke(this);
        }
        var newPos = transform.position;
        newPos.z = state ? 0 : -11;
        transform.position = newPos;
    }

    public List<GridEntity> GetEntities() => objects.GetEntities();
    public List<GridObject> GetObjects() => objects.GetObjects();

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
            ObjectAdded?.Invoke(obj, new WorldPos(this, pos));
            return;
        }

        throw new CannotMoveHereException();
    }

    public GridEntity GetEntityAt(Vector2Int pos)
    {
        return objects.GetEntityAt(pos);
    }

    public void Remove(GridObject obj)
    {
        RemoveAt(obj, obj.Vector);
    }

    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        objects.RemoveAt(obj, pos);
        ObjectRemoved?.Invoke(obj, new WorldPos(this, pos));
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

    public Vector3 WorldFromCell(Vector2Int cellPos)
    {
        return transform.position + new Vector3(cellPos.x + TileOffset, cellPos.y + TileOffset);
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
