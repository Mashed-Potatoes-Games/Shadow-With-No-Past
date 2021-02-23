using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.GridObjects;

/// <summary>
/// This class ties GridObjects to Grid and them only!
/// To interract with the grid mindind both tiles and objects, use MainGrid !!!
/// </summary>
[ExecuteAlways]
public class ObjectsGrid : MonoBehaviour
{
    public class UnexpectedEntityAtPosException : Exception { }
    public class PositionOccupiedException : Exception { }

    public enum ObjectType{
        Entity,
        Item,
        Empty
    }

    //The complexity of getting values from dictionary as well as all of the keys is close to O(1).
    //So it should be efficient to use Dictionary.
    public Dictionary<Vector2Int, GridObject> Objects
        = new Dictionary<Vector2Int, GridObject>();

    void Start()
    {
        //Every time Editor reloads, dictionary clears, so we need to write objects position again.
        foreach (GridObject obj in transform.GetComponentsInChildren<GridObject>())
        {
            SetNewObjectTo(obj, obj.CurrentPos);
        }
    }
    
    //Object teleports from current position to destination.
    //To mind tiles, use MainGrid!!!
    public void MoveInstantTo(GridObject obj, Vector2Int endPos)
    {
        Vector2Int startPos = obj.CurrentPos;
        if (WhatIn(endPos) == ObjectType.Entity)
        {
            throw new PositionOccupiedException();
        }

        RemoveAt(obj, startPos);
        SetNewObjectTo(obj, endPos);
        obj.CurrentPos = endPos;
    }

    public GridObject GetEntityAt(Vector2Int pos)
    {
        GridObject obj;
        Objects.TryGetValue(pos, out obj);
        return obj;
    }

    //Returns type of the object, so the entity could behave differently.
    public ObjectType WhatIn(Vector2Int pos)
    {
        if(Objects.ContainsKey(pos))
        {
            GridObject obj = Objects[pos];
            if(obj is BaseEntity)
            {
            return ObjectType.Entity;
            }
        }
        return ObjectType.Empty;
    }

    //Check entity in position and return is it the same entity.
    //This function is needed to catch the mistakes, where entity is not in the position, it should be.
    bool IsObjectInPos(GridObject entity, Vector2Int pos)
    {
        GridObject ObjInPos;
        bool IsOccupied = Objects.TryGetValue(pos, out ObjInPos);
        return IsOccupied && ObjInPos == entity;
    }

    //Deletes any entity at the position
    private void ClearPosition(Vector2Int pos)
    {
        Objects.Remove(pos);
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
    public void SetNewObjectTo(GridObject entity, Vector2Int pos)
    {
        if(WhatIn(pos) != ObjectType.Empty)
        {
            throw new PositionOccupiedException();
        }
        Objects.Add(pos, entity);
        entity.CurrentPos = pos;
    }
}
