using ShadowWithNoPast.Entities;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
public class TriggerZone
{
    public List<WorldPos> TriggerZones;

    private ITriggerAction triggerAction;

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
}