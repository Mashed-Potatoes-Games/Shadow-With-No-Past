using ShadowWithNoPast.GameProcess;
using System.Collections;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "SwapWorldsAction", menuName = "Abilities/Actions/SwapWorldsAction")]
    public class SwapWorldsAction : AbilityAction
    {
        public override IEnumerator Execute(WorldPos targetPos, int effectValue)
        {
            Game.WorldsChanger.MoveToOtherWorld(targetPos.GetEntity());
            yield break;
        }


    }
}