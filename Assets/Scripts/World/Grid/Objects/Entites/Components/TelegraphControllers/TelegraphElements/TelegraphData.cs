using ShadowWithNoPast.Entities;
using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphData
{
    public List<SingleTelegraphData> Elements = new List<SingleTelegraphData>();
    public PointerActions Actions;

    public TelegraphElementGroup Instantiate(int sortingLayerID, PointerActions actions = null)
    {
        List<TelegraphElement> telegraphs = new List<TelegraphElement>();
        foreach(SingleTelegraphData element in Elements)
        {
            telegraphs.Add(element.Instantiate(sortingLayerID));
        }

        TelegraphElementGroup group = new GameObject("TelegrpaphElementGroup", typeof(TelegraphElementGroup))
            .GetComponent<TelegraphElementGroup>();
        group.TieToTelegraphs(telegraphs, actions);
        return group;
    }
}

public class SingleTelegraphData
{
    public TelegraphElement Element;
    public WorldPos Pos;
    public int Value;
    public Direction? Rotation;


    public TelegraphElement Instantiate(
            int sortingLayerID,
            PointerActions actions = null)
    {
        TelegraphElement obj = GameObject.Instantiate(Element);
        if (actions != null)
        {
            if (actions.OnClick != null)
                obj.Clicked += actions.OnClick;
            if (actions.OnPointerEnter != null)
                obj.PointerEntered += actions.OnPointerEnter;
            if (actions.OnPointerLeave != null)
                obj.PointerLeft += actions.OnPointerLeave;

            obj.ToggleCollider(true);
        }
        else
        {
            obj.ToggleCollider(false);
        }

        if(Value != 0)
        {
            obj.SetTextValue(Value);
        }

        var gridObj = obj.GridObj;
        gridObj.SetNewPosition(Pos);


        obj.GetComponent<Renderer>().sortingLayerID = sortingLayerID;
        if(Rotation != null)
        {
            obj.transform.rotation = Quaternion.Euler(DirToRotation(Rotation.Value));
        }

        return obj;
    }

    private Vector3 DirToRotation(Direction dir)
    {
        return dir switch
        {
            Direction.Right => new Vector3(0, 0, 0),
            Direction.Down => new Vector3(0, 0, 90),
            Direction.Left => new Vector3(0, 0, 180),
            Direction.Up => new Vector3(0, 0, 270),
            _ => throw new NotImplementedException(),
        };
    }

    public SingleTelegraphData(TelegraphElement element, WorldPos pos, int value = 0, Direction? rotation = null)
    {
        Element = element;
        Pos = pos;
        Value = value;
        Rotation = rotation;
    }
}
