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

        public WorldPos TargetToExecPos(WorldPos target) => Ability.TargetToExecPos(Caller, target);
        public AbilityTargets AvailableTargets() =>
            Ability.AvailableTargets(Caller.Pos);
        public AbilityTargets AvailableAttackPoints(WorldPos target) =>
            Ability.AvailableAttackPoints(target);
        public List<WorldPos> ChangesTargetOnMovement(WorldPos initialTarget) =>
            Ability.ChangesTargetOnMovement(Caller.Pos, initialTarget);

        public IEnumerator UseAbility()
        {
            if (Ability.Type == AbilityTargetType.OnSelf)
            {
                yield return UseAbility(Caller.Pos);
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
            
            var currAbility = Ability;
            var i = 0;
            while(currAbility != null)
            {
                if(EffectValues.Length <= i)
                {
                    Debug.LogError("Ability don't have it's effect value!");
                    yield break;
                }
                yield return Ability.Execute(new AbilityArgs { 
                    Target = target,
                    Caller = Caller,
                    DistanceConstraint = DistanceConstraint,
                    EffectValue = EffectValues[i] 
                });
                i++;
                currAbility = currAbility.SecondaryEffect;
            }

            FinishExecution();
        }

        public void Cancel()
        {
            Cancelled?.Invoke();
        }

        public TelegraphData GetTelegraphData(WorldPos target)
        {
            var i = 0;
            var telegraphData = new TelegraphData();
            var currentAbility = Ability;
            List<SingleTelegraphData> helpingTelegraphs = currentAbility.GetHelpingTelegraphs(Caller, target);
            if (helpingTelegraphs != null)
            {
                telegraphData.Elements.AddRange(helpingTelegraphs);
            }

            do
            {
                foreach(var pos in currentAbility.TargetToAoe(Caller, target))
                {
                    SingleTelegraphData singleTelegraph = new SingleTelegraphData(currentAbility.Action.TelegraphElement, pos, EffectValues[i]);
                    telegraphData.Elements.Add(singleTelegraph);
                }

                i++;
                currentAbility = Ability.SecondaryEffect;
            }
            while (currentAbility != null && i < EffectValues.Length);

            return telegraphData;
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