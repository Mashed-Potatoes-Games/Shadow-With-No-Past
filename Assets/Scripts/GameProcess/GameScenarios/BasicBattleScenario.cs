using ShadowWithNoPast.Entities;
using ShadowWithNoPast.GameProcess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBattleScenario : TriggerZonesScenario
{
    protected virtual void Start()
    {
        Player.Entity.Died += RestartLevelOnPlayerDeath;
    }

    private void RestartLevelOnPlayerDeath(GridEntity enity, WorldPos pos)
    {
        Game.SceneLoader.RestartLevel();
    }
}
