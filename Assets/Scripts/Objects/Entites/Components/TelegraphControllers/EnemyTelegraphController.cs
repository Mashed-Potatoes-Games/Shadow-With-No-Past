using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public class EnemyTelegraphController : BasicTelegraphController
    {
        public void OnMouseEnter()
        {
            TelegraphAvailableMove(0.75f);
        }

        public void OnMouseExit()
        {
            ClearAvalableMoves();
        }
    }
}