using ShadowWithNoPast.GameProcess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TwoWorldsUI : Switchable
{
    [SerializeField]
    private CanvasGroup RegularUI;
    [SerializeField]
    private CanvasGroup TheEdgeUI;

    private void Awake()
    {
        //gameObject.SetActive(true);
//#if  UNITY_EDITOR
        //gameObject.SetActive(Application.isPlaying);
//#endif
    }
    protected override void SwitchTo(WorldType type)
    {
        TheEdgeUI.alpha = 0;
        RegularUI.alpha = 0;
        if(type == WorldType.Regular)
        {
            RegularUI.alpha = 1;
        }
        if(type == WorldType.TheEdge)
        {
            TheEdgeUI.alpha = 1;
        }
    }
}
