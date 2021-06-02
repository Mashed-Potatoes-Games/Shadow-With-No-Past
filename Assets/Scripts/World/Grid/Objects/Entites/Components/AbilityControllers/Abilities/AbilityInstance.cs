using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ShadowWithNoPast.Entities.Abilities
{

    [Serializable]
    public class AbilityInstance
    {
        public event Action UsedWithNoTarget;
        public event Action Cancelled;
        public event Action Updated;
        public event Action Used;


        public const float MinimalAbilityExecutionTime = 1;

        [HideInInspector]
        public GridEntity Caller;
        public Ability Ability;
        [SerializeField]
        public int[] EffectValues;
        public AudioClip AbilitySound => Ability.AbilitySound;
        public Sprite Icon => Ability.Icon;
        public int DistanceConstraint => Ability.DistanceConstraint;
        public int CooldownOnUse;
        [HideInInspector]
        public int RemainingCooldown = 0;

        [field: SerializeField, HideInInspector] public bool ReadyToUse { get; private set; } = true;
        [field: SerializeField] public bool EndsTurn { get; private set; } = true;

        public AbilityInstance(GridEntity caller, Ability ability, params int[] effectValues)
        {
            Caller = caller;
            Ability = ability;
            CooldownOnUse = Ability.DefaultCooldown;
            EffectValues = new int[Ability.Actions.Length];
            effectValues.CopyTo(EffectValues, 0);
        }

        public WorldPos TargetToExecPos(WorldPos target) => Ability.TargetToExecPos(Caller, target);
        public AbilityTargets AvailableTargets() =>
            Ability.AvailableTargets(Caller.Pos);
        public AbilityTargets AvailableAttackPoints(WorldPos target) =>
            Ability.AvailableAttackPoints(target);
        public List<WorldPos> ChangesTargetOnMovement(WorldPos initialTarget) =>
            Ability.ChangesTargetOnMovement(Caller.Pos, initialTarget);

        public IEnumerator UseAbility()
        {
            if (!Caller.TurnController.IsActiveTurn)
            {
                yield break;
            }
            if (Ability.Type == AbilityTargetType.OnSelf)
            {
                yield return UseAbility(Caller.Pos);
                yield break;
            }
            UsedWithNoTarget?.Invoke();
        }

        public IEnumerator UseAbility(WorldPos target)
        {
            var startTime = Time.time;
            target = Ability.TargetToExecPos(Caller, target);
            yield return Ability.PreExecute(Caller, target);

            for (int i = 0; i < Ability.Actions.Length; i++)
            {
                ActionWithAoe actionWithAoe = Ability.Actions[i];
                Caller.FaceTo(target.Vector);
                yield return actionWithAoe.Execute(Caller, target, EffectValues[i]);

            }

            while(Time.time < startTime + MinimalAbilityExecutionTime)
            {
                yield return null;
            }

            Caller.SpriteController.ResetToDefault();
            FinishExecution();
        }

        public void Cancel()
        {
            Cancelled?.Invoke();
        }

        public TelegraphData GetTelegraphData(WorldPos target)
        {

            var telegraphData = new TelegraphData();
            List<SingleTelegraphData> helpingTelegraphs = Ability.GetAbilityTelegraphs(Caller, target);
            if (helpingTelegraphs != null)
            {
                telegraphData.Elements.AddRange(helpingTelegraphs);
            }

            if(Ability.Actions == null || Ability.Actions.Length == 0)
            {
                Debug.LogWarning("Ability has no actions");
                return telegraphData;
            }

            for (int i = 0; i < Ability.Actions.Length; i++)
            {
                ActionWithAoe ActionWithAoe = Ability.Actions[i];
                if (ActionWithAoe.Pattern == null)
                {
                    AddTelegraph(ref telegraphData, ActionWithAoe.Action.TelegraphElement, target, EffectValues[i]);
                    continue;
                }
                foreach (var pos in ActionWithAoe.Pattern.TargetToAoe(Caller, target))
                {
                    AddTelegraph(ref telegraphData, ActionWithAoe.Action.TelegraphElement, pos, EffectValues[i]);
                }
            }
            return telegraphData;
        }

        private static void AddTelegraph(ref TelegraphData telegraphData, TelegraphElement element, WorldPos pos, int effectValue)
        {
            SingleTelegraphData singleTelegraph =
                                    new SingleTelegraphData(element, pos, effectValue);
            telegraphData.Elements.Add(singleTelegraph);
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
            if (RemainingCooldown > 0)
            {
                RemainingCooldown--;
                if (RemainingCooldown == 0)
                {
                    ReadyToUse = true;
                }
                Updated?.Invoke();
            }
        }
    }
}