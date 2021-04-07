using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShadowWithNoPast.Entities.Abilities
{

    [Serializable]
    public class AbilityInstance
    {

        public AbilityInstance(GridEntity caller, Ability ability, params int[] effectValues)
        {
            Caller = caller;
            Ability = ability;
            EffectValues = effectValues;
        }

        public event Action UsedWithNoTarget;
        public event Action Updated;
        public event Action Used;
        public GridEntity Caller;

        public AudioClip AbilitySound => Ability.AbilitySound;
        public Sprite Icon => Ability.Icon;
        public Ability Ability;
        public int[] EffectValues = { 1 };
        public int CooldownOnUse;
        public int RemainingCooldown = 0;

        [field: SerializeField] public bool ReadyToUse { get; private set; } = true;
        [field: SerializeField] public bool EndsTurn { get; private set; } = true;

        public List<TargetPos> AvailableTargets() => 
            Ability.AvailableTargets(Caller.GetGlobalPos());
        public List<TargetPos> CanAttackTargetFrom(TargetPos target) =>
            Ability.AvailableTargets(target);

        public IEnumerator UseAbility()
        {
            if(Ability.Type == AbilityType.OnSelf)
            {
                yield return UseAbility(new TargetPos());
                yield break;
            }
            UsedWithNoTarget?.Invoke();
        }

        public IEnumerator UseAbility(TargetPos target)
        {
            yield return Ability.Execute(Caller, target, EffectValues[0]);

            FinishExecution();
        }
        private void FinishExecution()
        {
            // "+ 1" is here, because after the ability is used it always decrement all abilities CD's by 1.
            //If there is better solution, I will gladly change this shitty pile of code.
            RemainingCooldown = CooldownOnUse + 1;
            ReadyToUse = false;

            Used?.Invoke();
            Updated?.Invoke();
        }

        public void OnTurnPassed()
        {
            if(RemainingCooldown > 0)
            {
                RemainingCooldown--;
                if(RemainingCooldown == 0)
                {
                    ReadyToUse = true;
                }
                Updated?.Invoke();
            }
        }
    }
}