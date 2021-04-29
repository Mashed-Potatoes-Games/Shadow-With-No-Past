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
            CooldownOnUse = Ability.DefaultCooldown;
        }

        public event Action UsedWithNoTarget;
        public event Action Cancelled;
        public event Action Updated;
        public event Action Used;

        [HideInInspector]
        public GridEntity Caller;
        public Ability Ability;
        public AudioClip AbilitySound => Ability.AbilitySound;
        public Sprite Icon => Ability.Icon;
        public int[] EffectValues = { 1 };
        public int DistanceConstraint => Ability.DistanceConstraint;
        public int CooldownOnUse;
        [HideInInspector]
        public int RemainingCooldown = 0;

        [field: SerializeField] public bool ReadyToUse { get; private set; } = true;
        [field: SerializeField] public bool EndsTurn { get; private set; } = true;

        public AbilityTargets AvailableTargets() =>
            Ability.AvailableTargets(Caller.GetGlobalPos());
        public AbilityTargets AvailableAttackPoints(WorldPos target) =>
            Ability.AvailableAttackPoints(target);

        public IEnumerator UseAbility()
        {
            if (Ability.Type == AbilityTargetType.OnSelf)
            {
                yield return UseAbility(Caller.GetGlobalPos());
                yield break;
            }
            UsedWithNoTarget?.Invoke();
        }

        public IEnumerator UseAbility(WorldPos target)
        {
            if(EffectValues is null || EffectValues.Length == 0)
            {
                Debug.LogError("Effect values are null or don't contain values!");
            }
            yield return Ability.Execute(Caller, target, EffectValues[0]);
            var nextEffect = Ability.SecondaryEffect;
            var i = 1;
            while(nextEffect != null)
            {
                if(EffectValues.Length <= i)
                {
                    Debug.LogError("Secondary ability don't have it's effect value!");
                    yield break;
                }
                yield return nextEffect.Execute(Caller, target, EffectValues[i]);
                i++;
            }

            FinishExecution();
        }

        public void Cancel()
        {
            Cancelled?.Invoke();
        }

        public Dictionary<TelegraphElementWithValue, List<WorldPos>> GetTelegraphData(WorldPos target)
        {
            var i = 0;
            var telegraphData = new Dictionary<TelegraphElementWithValue, List<WorldPos>>();
            var currentAbility = Ability;
            do
            {
                var elementWithValue = new TelegraphElementWithValue(
                    currentAbility.Action,
                    EffectValues[i]
                    );

                telegraphData.Add(
                    elementWithValue, 
                    currentAbility.TargetToAoe(Caller, target));

                i++;
                currentAbility = Ability.SecondaryEffect;
            }
            while (currentAbility != null && i < EffectValues.Length);

            return telegraphData;
        }

        public struct TelegraphElementWithValue
        {
            public TelegraphElement Element;
            public bool IsValueDependent;
            public int Value;

            public TelegraphElementWithValue(TelegraphElement element, bool isValueDependent = false, int value = 0)
            {
                Element = element;
                IsValueDependent = isValueDependent;
                Value = value;
            }

            public TelegraphElementWithValue(AbilityAction action, int value = 0)
            {
                Element = action.TelegraphElement;
                IsValueDependent = action.IsValueDependent;
                Value = value;
            }
        }

        private void FinishExecution()
        {
            // "+ 1" is here, because after the turn is over, it always decrement all abilities CD's by 1.
            //If there is better solution without adding complication, I will gladly change this shitty pile of code.
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