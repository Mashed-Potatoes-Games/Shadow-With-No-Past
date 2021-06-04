using ShadowWithNoPast.Entities.Abilities;
using ShadowWithNoPast.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class EnemyTurnController : MonoBehaviour, ITurnController
    {
        public event Action TurnPassed;
        public bool IsActiveTurn { get; set; }

        public TurnPriority Priority { get; set; } = TurnPriority.Normal;

        private GridEntity entity;
        private World world;
        private IMovementController movement;
        private IAbilitiesController abilities;
        private ITelegraphController telegraphController;

        private AbilityInstance savedAbility;
        private WorldPos? savedTarget;

        public Queue<WorldPos> MovementQueue = new Queue<WorldPos>();

        // Start is called before the first frame update
        void Awake()
        {
            entity = GetComponent<GridEntity>();
            world = entity.World;
            movement = GetComponent<IMovementController>();
            abilities = GetComponent<IAbilitiesController>();
            telegraphController = GetComponent<ITelegraphController>();
        }
        public IEnumerator MoveAndTelegraphAction()
        {
            entity.TelegraphController.ClearAll();
            GridEntity player = Player.Entity;
            if (player == null || abilities == null)
            {
                yield break;
            }

            var availableMoves = movement.GetAvailableMoves();
            //TODO: Add GetHash() to Ability instance.
            var availableAttacks = new Dictionary<AbilityInstance, List<WorldPos>>();
            var inavailableAttacks = new Dictionary<AbilityInstance, List<WorldPos>>();
            foreach (var ability in abilities)
            {
                if(!ability.ReadyToUse)
                {
                    continue;
                }
                var attackTargets = ability.AvailableAttackPoints(player.Pos);
                var attackPos = attackTargets.positions;
                var placesToAttack = availableMoves.Intersect(attackPos).ToList();
                if (placesToAttack.Count() > 0)
                {
                    availableAttacks.Add(ability, placesToAttack);
                    continue;
                }
                // Optimise later at any cost!!
                var availableLate = attackPos.Where(x => movement.GetPath(x) != null);
                if (availableLate.Count() != 0)
                {
                    inavailableAttacks.Add(ability, availableLate.ToList());

                }
            }

            AbilityInstance abilityInstance;
            Queue<WorldPos> path;

            if (availableAttacks.Count > 0)
            {
                PickRandomAttackPoint(availableAttacks, true, out abilityInstance, out path);

                if (path != null)
                {
                    savedAbility = abilityInstance;
                    savedTarget = player.Pos;

                    yield return movement.MoveWithDelay(path);
                    telegraphController.TelegraphAbility(player.Pos, abilityInstance, false);
                    yield break;
                }
            }

            if (inavailableAttacks.Count > 0)
            {
                PickRandomAttackPoint(inavailableAttacks, false, out abilityInstance, out path);
                if (path != null)
                {
                    yield return movement.MoveWithDelay(path);
                }
            }
        }

        private void PickRandomAttackPoint(Dictionary<AbilityInstance, List<WorldPos>> availableAttacks, bool isStricts, out AbilityInstance abilityInstance, out Queue<WorldPos> path)
        {
            int randomAbilityNumber = UnityEngine.Random.Range(0, availableAttacks.Count() - 1);
            var randomAbilityPosPair = availableAttacks.ElementAt(randomAbilityNumber);

            abilityInstance = randomAbilityPosPair.Key;
            int randomPosNumber = UnityEngine.Random.Range(0, randomAbilityPosPair.Value.Count() - 1);

            WorldPos randomPos = randomAbilityPosPair.Value.ElementAt(randomPosNumber);


            path = movement.GetPath(randomPos, true);
        }

        public IEnumerator ExecuteMove()
        {
            IsActiveTurn = true;
            if (ReadyToExecute())
            {
                yield return savedAbility.UseAbility(savedTarget.GetValueOrDefault());
            }
            EndTurn();
            yield break;
        }

        public bool ReadyToExecute()
        {
            return savedAbility != null && savedTarget.HasValue;
        }

        private void EndTurn()
        {
            TurnPassed?.Invoke();
            savedAbility = null;
            savedTarget = null;
            telegraphController.ClearAbility();
            IsActiveTurn = false;
        }

        public bool EngageCombat()
        {
            var availableMoves = movement.GetAvailableMoves();

            foreach (var ability in abilities)
            {
                if (!ability.ReadyToUse)
                {
                    continue;
                }
                var attackTargets = ability.AvailableAttackPoints(Player.Entity.Pos);
                var attackPos = attackTargets.positions;
                var placesToAttack = availableMoves.Intersect(attackPos).ToList();
                if (placesToAttack.Count() > 0)
                {
                    return true;
                }
                // Optimise later at any cost!!
                var availableLate = attackPos.Where(x => movement.GetPath(x) != null);
                if (availableLate.Count() != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}