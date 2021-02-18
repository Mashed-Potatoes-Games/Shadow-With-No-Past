using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.GridObjects;

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
    //The complexity of getting values from dictionary as well as all of the keys is close to O(1)
    //So it should be efficient to use Dictionary
    
    public Dictionary<Vector2Int, GridObject> Objects
        = new Dictionary<Vector2Int, GridObject>();

    void Start()
    {
        GridObject[] ChildEntities = transform.GetComponentsInChildren<GridObject>();
        foreach(GridObject gridObj in ChildEntities)
        {
            SetNewObjectTo(gridObj, gridObj.CurrentPos);
        }
    }


    void Update()
    {

    }

    public void MoveTo(GridObject obj, Vector2Int startPos, Vector2Int endPos)
    {

        if (WhatIn(endPos) == ObjectType.Entity)
        {
            throw new PositionOccupiedException();
        }

        RemoveAt(obj, startPos);
        SetNewObjectTo(obj, endPos);
    }

    public GridObject GetEntityAt(Vector2Int pos)
    {
        GridObject obj;
        Objects.TryGetValue(pos, out obj);
        return obj;
    }

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
    //Can be used externally
    public void RemoveAt(GridObject obj, Vector2Int pos)
    {
        if (!IsObjectInPos(obj, pos))
        {
            throw new UnexpectedEntityAtPosException();
        }
        ClearPosition(pos);
    }

    //This method does not move the entity - it places the entity that isn't present in grid
    //
    public void SetNewObjectTo(GridObject entity, Vector2Int pos)
    {
        if(WhatIn(pos) != ObjectType.Empty)
        {
            throw new PositionOccupiedException();
        }
        Objects.Add(pos, entity);
    }
}
