using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [RequireComponent(typeof(Camera))]
public class CameraBackgroundChanger : Switchable
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

}
