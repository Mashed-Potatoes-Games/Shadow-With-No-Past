using ShadowWithNoPast.GameProcess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public abstract class Switchable : MonoBehaviour
{
    protected abstract void SwitchTo(WorldType type);

    public void SwitchTo(World world) => SwitchTo(world.Type);
    public virtual void Start()
    {
        if(Game.WorldsChanger != null)
        {
            Game.WorldsChanger.WorldsSwitched += SwitchTo;
            SwitchTo(Game.WorldsChanger.Active);
        }
    }

    public virtual void OnDestroy()
    {
        if (Game.WorldsChanger != null)
        {
            Game.WorldsChanger.WorldsSwitched -= SwitchTo;
        }
    }
}
