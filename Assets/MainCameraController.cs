using ShadowWithNoPast.Entities;
using ShadowWithNoPast.GameProcess;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Camera))]
public class MainCameraController : Switchable
{
    [SerializeField]
    private Color32 edgeBackground;

    [SerializeField]
    private Color32 regularBackground;
    

    protected override void SwitchTo(WorldType type)
    {
        Camera.main.backgroundColor = type switch { 
            WorldType.Regular => regularBackground,
            WorldType.TheEdge => edgeBackground,
            _ => throw new NotImplementedException()
        }; 
    }

    public void StartFollow(GridObject obj)
    {
        Follow(obj, obj.Pos, obj.Pos);
        obj.Moved += Follow;

    }

    public void StopFollow(GridObject obj)
    {
        obj.Moved -= Follow;
    }

    private void Follow(GridObject obj, WorldPos start, WorldPos end)
    {
        Vector3 Vector3Pos = obj.transform.position;
        Vector3Pos.z = transform.position.z;
        transform.position = Vector3Pos;
        Game.WorldsChanger.SetActive(end.World);
    }

}
