using ShadowWithNoPast.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
public class TriggerZone
{
    public List<WorldPos> TriggerZones;

    [SerializeField]
    private ITriggerAction triggerAction;

    [SerializeField]
    private GridObject triggerIcon;

    private GridObject triggerZonesGroup;

    public bool Enabled { get; private set; } = false;

    private TriggerZone() { }
    public TriggerZone(List<WorldPos> zones, ITriggerAction action, params GridEntity[] entities)
    {
        TriggerZones = zones;
        triggerAction = action;
        foreach(var entity in entities)
        {
            entity.Moved += triggerAction.Action;
        }
    }

    public virtual void SetExploration()
    {
        if(!Enabled)
        {
            Activate();
        }
    }

    private void Activate()
    {
        foreach(var pos in TriggerZones)
        {
            if(triggerIcon != null)
            {

            }
        }
    }
}