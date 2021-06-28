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
        //Game.TurnsHandler.TurnPassed += UpdateZones;
        Zones.ForEach(zone => zone.SetExploration());
    }

    private void UpdateZones()
    {
        if(Zones != null && Zones.Count != 0)
        {
            foreach(var zone in Zones)
            {
                switch (Game.TurnsHandler.State)
                {
                    case TurnSystemState.Exploration:
                        zone.SetExploration();
                        break;
                    case TurnSystemState.Battle:
                        zone.SetBattle();
                        break;
                }
            }
        }
    }
}
