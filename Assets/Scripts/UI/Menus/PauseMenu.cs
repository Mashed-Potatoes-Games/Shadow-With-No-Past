using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PauseMenu : Switchable
{
    [SerializeField]
    private Sprite regularMenu;
    [SerializeField]
    private Sprite theEdgeMenu;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    protected override void SwitchTo(WorldType type)
    {
        image.sprite = type switch
        {
            WorldType.Regular => regularMenu,
            WorldType.TheEdge => theEdgeMenu,
            _ => throw new System.NotImplementedException()
        };
    }
}
