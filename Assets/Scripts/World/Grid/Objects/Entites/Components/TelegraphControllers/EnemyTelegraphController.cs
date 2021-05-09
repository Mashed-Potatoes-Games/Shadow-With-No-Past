using ShadowWithNoPast.Entities.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowWithNoPast.Entities
{
    public class EnemyTelegraphController : BasicTelegraphController, IPointerEnterHandler, IPointerExitHandler
    {
        private List<WorldPos> ChangesTargetOnMovement;

        public override void TelegraphAbility(WorldPos target, AbilityInstance abilityInstance, bool showAttackValue = false, PointerActions actions = null)
        {
            base.TelegraphAbility(target, abilityInstance, showAttackValue, actions);

            ChangesTargetOnMovement = abilityInstance.ChangesTargetOnMovement(target);
            if(ChangesTargetOnMovement != null)
            {
                target.World.AttacksAccounter.RefreshPossibleTargets(abilityInstance, ChangesTargetOnMovement);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            TelegraphAvailableMove();
            HighlighAbility();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearAvalableMoves();
            RemoveHighlighAbility();
        }
    }
}