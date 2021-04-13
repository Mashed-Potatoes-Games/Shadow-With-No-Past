using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities;
using System;

namespace ShadowWithNoPast.Entities.Abilities
{
    public abstract class AbilityAction : ScriptableObject
    {

        public TelegraphElement TelegraphElement;
        public virtual bool IsValueDependent => true;

        // All of the thing needed to be done, when skill is executed,
        // exact actions should be overriden in the innerhited abilities.
        public virtual void Execute(TargetPos targetPos, int effectValue)
        {

        }
    }
}