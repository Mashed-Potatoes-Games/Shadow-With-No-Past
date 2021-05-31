using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBackground : Switchable
{
    [SerializeField]
    private Sprite regularBackground; 

    [SerializeField]
    private Sprite edgeBackground;

    protected override void SwitchTo(WorldType type)
    {
        GetComponent<Image>().sprite = type switch 
        {
            WorldType.Regular => regularBackground,
            WorldType.TheEdge => edgeBackground,
            _ => throw new NotImplementedException()

        };
    }
}
