using ShadowWithNoPast.GridObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This allows Awake to be called in the editor, to get the links to children Tilemaps and ObjectsGrid.
[ExecuteAlways]
[RequireComponent(typeof(Grid))]
public class GridManagement : MonoBehaviour
{
    public class CannotMoveHereException : Exception { }
    public enum CellStatus
    {
        NoGround,
        Obstacle,
        Free,
        Entity,
        Item
    }

    public const float TileOffset = 0.5f;

    public List<Tilemap> GroundMaps;
    public List<Tilemap> ObstacleMaps;
    public ObjectsGrid ObjGrid;

    public void Awake()
    {
        GetMaps();
    }

    void GetMaps()
    {
        //Tilemaps or other objects need to be the childer in order to find and work with those.
        //This is because every world has it's own tilemaps and it allows grids to differentiate between them.
        List<GameObject> GridChildren = GetGridChildren();

        foreach (GameObject obj in GridChildren)
        {
            //We are trying to get the components, tied to the GameObject.
            Tilemap tilemap = obj.GetComponent<Tilemap>();
            ObjectsGrid objGrid = obj.GetComponent<ObjectsGrid>();
            if (tilemap != null)
            {
                //To differentiate between Ground and Obstacles they have different tags.
                if (obj.CompareTag("GroundTilemap"))
                {
                    GroundMaps.Add(tilemap);
                }
                else if (obj.CompareTag("ObstacleTilemap"))
                {
                    ObstacleMaps.Add(tilemap);
                }
            }
            else if (objGrid != null)
            {
                ObjGrid = objGrid;
            }
        }
    }

    List<GameObject> GetGridChildren()
    {
        List<GameObject> GridChildren = new List<GameObject>();

        //Transform is IEnumerable that contains all the children transforms.
        foreach (Transform child in transform)
        {
            //We need not transfrorms, but the objects itself.
            GridChildren.Add(child.gameObject);
        }

        return GridChildren;
    }

    public bool IsObstacle(Vector2Int pos)
    {
        bool isObstacle = false;
        //Cycles through obstacle tilemaps to find at least one in given position
        Vector3Int vector3Pos = new Vector3Int(pos.x, pos.y, 0);
        foreach (Tilemap tilemap in ObstacleMaps)
        {
            if (tilemap.GetTile(vector3Pos) != null)
            {
                isObstacle = true;
                break;
            }
        }
        return isObstacle;
    }

    public bool IsGround(Vector2Int pos)
    {
        //Cycles through ground tilemaps to find at least one in given position
        Vector3Int vector3Pos = new Vector3Int(pos.x, pos.y, 0);
        bool isGround = false;
        foreach (Tilemap tilemap in GroundMaps)
        {
            if (tilemap.GetTile(vector3Pos) != null)
            {
                isGround = true;
                break;
            }
        }
        return isGround;
    }


    //Returns, what is at the given coordinate.
    public CellStatus GetCellStatus(Vector2Int pos)
    {
        if (!IsGround(pos))
        {
            return CellStatus.NoGround;
        } else if (IsObstacle(pos))
        {
            return CellStatus.Obstacle;
        } else 
        {
            switch (ObjGrid.WhatIn(pos))
            {
                //In the grid of objects - position is empty,
                case ObjectsGrid.ObjectType.Empty:
                    return CellStatus.Free;
                    //but in the Grid info position is not, because there is ground and obstacles.
                    //So it's named "Free", which means it contains ground tile, has no obstacles and free for any GridObject.
                case ObjectsGrid.ObjectType.Entity:
                    return CellStatus.Entity;
                case ObjectsGrid.ObjectType.Item:
                    return CellStatus.Item;
                default:
                    throw new NotImplementedException();
                    //Other Grid types can be implemented - such as burning tiles, so "Free" is not by default,
                    //and if it catches any different object - you will know it's a place you need to add.
            }
        }
    }

    //It "teleports" object from it's place to destination pos.
    public void MoveInstantTo(GridObject obj, Vector2Int pos)
    {
        if(GetCellStatus(pos) == CellStatus.Free)
        {
            ObjGrid.MoveInstantTo(obj, pos);
        }
        else if(ObjGrid.GetEntityAt(pos) != obj)
        {
            throw new CannotMoveHereException();
        }
    }

    //It initiates the chosen objects on the grid.
    //Don't try to move objects with it, because it won't delete previous object position.
    public void SetNewObjectTo(GridObject obj, Vector2Int pos)
    {
        if(GetCellStatus(pos) == CellStatus.Free)
        {
            ObjGrid.SetNewObjectTo(obj, pos);
        } else
        {
            throw new CannotMoveHereException();
        }
    }

    //Those are wrappers, so the entites, using GridManagement won't touch ObjectsGrid by it's own.
    public GridObject GetEntityAt(Vector2Int pos)
    {
        return ObjGrid.GetEntityAt(pos);
    }

    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        ObjGrid.RemoveAt(obj, pos);
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
