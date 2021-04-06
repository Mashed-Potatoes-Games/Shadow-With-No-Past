using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ShadowWithNoPast.Utils;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "AttackAction", menuName = "Abilities/Actions/AttackAction", order = 1)]
    class AttackAction : AbilityAction
    {
        public override void Execute(TargetPos targetPos, int effectValue)
        {
            base.Execute(targetPos, effectValue);
            GridEntity target = targetPos.GetEntity();
            if(target is null)
            {
                // Nobody to attack, play miss animation or skip
                return;
            }
            target.ReceiveDamage(effectValue);
        }
    }
}
