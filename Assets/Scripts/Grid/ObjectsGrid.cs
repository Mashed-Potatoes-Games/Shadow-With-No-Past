using System;
using System.Collections;
using UnityEngine;
using ShadowWithNoPast.Entities;

/// <summary>
/// This class ties GridObjects to Grid and them only!
/// To interract with the grid mindind both tiles and objects, use MainGrid !!!
/// </summary>
[ExecuteAlways]
public partial class ObjectsGrid : MonoBehaviour
{
    public class UnexpectedEntityAtPosException : Exception { }
    public class PositionOccupiedException : Exception { }

    //The complexity of getting values from dictionary as well as all of the keys is close to O(1).
    //So it should be efficient to use Dictionary.
    [SerializeField]
    private ObjDictionary objects;

    void Start()
    {
        objects = new ObjDictionary();
        //Every time Editor reloads, dictionary clears, so we need to write objects position again.
        foreach (GridObject obj in transform.GetComponentsInChildren<GridObject>())
        {
            SetNewObjectTo(obj, obj.Pos);
        }
    }
    
    //Object teleports from current position to destination.
    //To mind tiles, use MainGrid!!!
    public void MoveInstantTo(GridObject obj, Vector2Int endPos)
    {
        Vector2Int startPos = obj.Pos;
        if (WhatIn(endPos) == ObjectType.Entity)
        {
            throw new PositionOccupiedException();
        }

        RemoveAt(obj, startPos);
        SetNewObjectTo(obj, endPos);
        obj.Pos = endPos;
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
    bool IsObjectInPos(GridObject entity, Vector2Int pos)
    {
        GridObject ObjInPos;
        bool IsOccupied = objects.TryGetValue(pos, out ObjInPos);
        return IsOccupied && ObjInPos == entity;
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
    public void SetNewObjectTo(GridObject entity, Vector2Int pos)
    {
        if(WhatIn(pos) != ObjectType.Empty)
        {
            throw new PositionOccupiedException();
        }
        objects.Add(pos, entity);
        entity.Pos = pos;
    }
}
public enum ObjectType
{
    Entity,
    Item,
    Empty
}