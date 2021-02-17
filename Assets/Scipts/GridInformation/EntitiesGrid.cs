using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

public class EntitiesGrid : MonoBehaviour
{
    public class UnexpectedEntityAtPosException : Exception { }
    public class PositionOccupiedException : Exception { }

    //The complexity of getting values from dictionary as well as all of the keys is close to O(1)
    //So it should be efficient to use Dictionary
    public Dictionary<Vector2Int, BaseEntity> PositionsOfEntities
        = new Dictionary<Vector2Int, BaseEntity>();
    void Start()
    {

    }


    void Update()
    {

    }

    public void MoveTo(BaseEntity entity, Vector2Int startPos, Vector2Int endPos)
    {

        if (IsOccupied(endPos))
        {
            throw new PositionOccupiedException();
        }

        RemoveAt(entity, startPos);
        SetEntityTo(entity, endPos);
    }

    public BaseEntity GetEntityAt(Vector2Int pos)
    {
        BaseEntity entity;
        PositionsOfEntities.TryGetValue(pos, out entity);
        return entity;
    }

    public bool IsOccupied(Vector2Int pos)
    {
        if(PositionsOfEntities.ContainsKey(pos))
        {
            return true;
        }
        return false;
    }

    bool IsEntityInPos(BaseEntity entity, Vector2Int pos)
    {
        BaseEntity EntityInPos;
        bool IsOccupied = PositionsOfEntities.TryGetValue(pos, out EntityInPos);
        return IsOccupied && EntityInPos == entity;
    }

    //Deletes any entity at the position
    private void ClearPosition(Vector2Int pos)
    {
        PositionsOfEntities.Remove(pos);
    }

    //Will throw an exception if the Position contains entity that is different from specified.
    //Can be used externally
    public void RemoveAt(BaseEntity entity, Vector2Int pos)
    {
        if (!IsEntityInPos(entity, pos))
        {
            throw new UnexpectedEntityAtPosException();
        }
        ClearPosition(pos);
    }

    //This method does not move the entity - it places the entity that isn't present in grid
    //
    //Each time entity initializes it adds itself to a position it was set to in the editor
    //For this to happen, the entity should be a child of the GameObject, this script is attached to
    public void SetEntityTo(BaseEntity entity, Vector2Int pos)
    {
        if(IsOccupied(pos))
        {
            throw new PositionOccupiedException();
        }
        PositionsOfEntities.Add(pos, entity);
    }
}
