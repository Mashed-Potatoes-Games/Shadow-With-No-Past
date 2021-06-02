using ShadowWithNoPast.GameProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TriggerZonesScenario : MonoBehaviour, IGameScenario
{
    public List<TriggerZone> Zones;

    protected virtual void Start()
    {
        Game.TurnsHandler.TurnPassed += UpdateZones;
    }

    private void UpdateZones()
    {
        if(Game.TurnsHandler.State == TurnSystemState.Exploration)
        {
            foreach(var zone in Zones)
            {
                zone.SetExploration();
            }
        }
    }
}
