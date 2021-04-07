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
            TelegraphAvailableMove(0.75f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearAvalableMoves();
        }
    }
}