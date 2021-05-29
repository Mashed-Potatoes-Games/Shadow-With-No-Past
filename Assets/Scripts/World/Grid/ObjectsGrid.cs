using System;
using System.Collections;
using UnityEngine;
using ShadowWithNoPast.Entities;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class ties GridObjects to Grid and them only!
/// To interract with the grid mindind both tiles and objects, use MainGrid !!!
/// </summary>
[ExecuteAlways]
public partial class ObjectsGrid : MonoBehaviour
{
    public class UnexpectedEntityAtPosException : Exception { }
    public class PositionOccupiedException : Exception { }

    private ObjDictionary objects = new ObjDictionary();

    void Awake()
    {
        //Every time Editor reloads, dictionary clears, so we need to write objects position again.
        foreach (GridObject obj in transform.GetComponentsInChildren<GridObject>())
        {
            SetNewObjectTo(obj, obj.Vector);
        }
    }
    
    //Object teleports from current position to destination.
    //To mind tiles, use MainGrid!!!
    public void MoveInstantTo(GridObject obj, Vector2Int endPos)
    {
        Vector2Int startPos = obj.Vector;
        if (WhatIn(endPos) == ObjectType.Entity)
        {
            throw new PositionOccupiedException();
        }

        RemoveAt(obj, startPos);
        SetNewObjectTo(obj, endPos);
    }

    public GridEntity GetEntityAt(Vector2Int pos)
    {
        GridObject obj;
        objects.TryGetValue(pos, out obj);
        if(obj is GridEntity)
        {
            return (GridEntity)obj;
        }
        return null;
    }

    //Returns type of the object, so the entity could behave differently.
    public ObjectType WhatIn(Vector2Int pos)
    {
        if(objects.ContainsKey(pos))
        {
            GridObject obj = objects[pos];
            if(obj is GridEntity)
            {
            return ObjectType.Entity;
            }
        }
        return ObjectType.Empty;
    }

    //Check entity in position and return is it the same entity.
    //This function is needed to catch the mistakes, where entity is not in the position, it should be.
    bool IsObjectInPos(GridObject obj, Vector2Int pos)
    {
        GridObject ObjInPos;
        bool IsOccupied = objects.TryGetValue(pos, out ObjInPos);
        return IsOccupied && ObjInPos == obj;
    }

    //Deletes any entity at the position
    private void ClearPosition(Vector2Int pos)
    {
        objects.Remove(pos);
    }

    //Will throw an exception if the Position contains entity that is different from specified.
    //Can be used externally.
    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        if (!IsObjectInPos(obj, pos))
        {
            throw new UnexpectedEntityAtPosException();
        }
        ClearPosition(pos);
    }

    //This method does not move the entity - it places the entity that isn't present in grid.
    //To mind tiles, use MainGrid!!!
    public void SetNewObjectTo(GridObject obj, Vector2Int pos)
    {
        if(WhatIn(pos) != ObjectType.Empty)
        {
            throw new PositionOccupiedException();
        }
        objects.Add(pos, obj);
        obj.SetNewPosition(pos);
        obj.transform.SetParent(transform);
    }

    public List<GridEntity> GetEntities()
    {
        var entities = new List<GridEntity>();
        foreach (GridObject obj in objects.Values)
        {
            if (obj is GridEntity entity)
            {
                entities.Add(entity);
            }
        }

        return entities;
    }

    public List<GridObject> GetObjects()
    {
        var objects = new List<GridObject>();
        foreach (GridObject obj in this.objects.Values)
        {
                objects.Add(obj);
        }
        return objects;
    }
}


public enum ObjectType
{
    Entity,
    Item,
    Empty
}