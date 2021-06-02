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
    private GridObject triggerObject;

    private GameObject triggerZonesGroup;

    public bool Enabled { get; private set; } = false;

    private TriggerZone() { }
    public TriggerZone(List<WorldPos> zones, ITriggerAction action, TargetType targetType, params GridEntity[] entities)
    {
        TriggerZones = zones;
        triggerAction = action;
        switch(targetType)
        {
            case TargetType.Player:
                Player.Entity.Moved += CheckTargetMove;
                break;
            case TargetType.AnyEntity:
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
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
    triggerZonesGroup = new GameObject("TriggerZones");
    triggerZonesGroup.transform.SetParent(TriggerZones.First().World.transform);
        foreach(var pos in TriggerZones)
        {
            if(triggerObject != null)
            {

            }
        }
    }

    private void CheckTargetMove(GridObject trigerer, WorldPos startPos, WorldPos endPos)
    {

    }
}

public enum TargetType
{
    Player,
    AnyEntity
}