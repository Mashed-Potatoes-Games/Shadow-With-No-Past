using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShadowWithNoPast.Entities
{
    public class EnemyTelegraphController : BasicTelegraphController, IPointerEnterHandler, IPointerExitHandler
    {
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