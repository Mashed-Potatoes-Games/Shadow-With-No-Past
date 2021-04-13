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

        public TurnPriority Priority { get; set; } = TurnPriority.Normal;

        private GridEntity entity;
        private WorldManagement world;
        private IMovementController movement;
        private IAbilitiesController abilities;
        private ITelegraphController telegraphController;

        private AbilityInstance savedAbility;
        private TargetPos? savedTarget;

        public Queue<Vector2Int> MovementQueue = new Queue<Vector2Int>();

        // Start is called before the first frame update
        void Awake()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
            movement = GetComponent<IMovementController>();
            abilities = GetComponent<IAbilitiesController>();
            telegraphController = GetComponent<ITelegraphController>();
        }
        public IEnumerator MoveAndTelegraphAction()
        {
            GridEntity player = FindPlayer();
            if (player is null || abilities is null)
            {
                yield break;
            }

            var availableMoves = movement.GetAvailableMoves();
            var availableAttacks = new Dictionary<AbilityInstance, List<Vector2Int>>();
            var inavailableAttacks = new Dictionary<AbilityInstance, List<Vector2Int>>();
            foreach (var ability in abilities)
            {
                var attackTargets = ability.AvailableAttackPoints(player.GetGlobalPos());
                var attackPos = attackTargets.Select(target => target.Pos).ToList();
                //TODO CHANGE TO USING TARGET POS!!!
                var placesToAttack = availableMoves.Intersect(attackPos).ToList();
                if (placesToAttack.Count() > 0 && ability.ReadyToUse)
                {
                    availableAttacks.Add(ability, placesToAttack);
                    continue;
                }

                inavailableAttacks.Add(ability, attackPos);
            }

            AbilityInstance abilityInstance;
            Queue<Vector2Int> path;

            if (availableAttacks.Count > 0)
            {
                PickRandomAttackPoint(availableAttacks, true, out abilityInstance, out path);

                if (path != null)
                {
                    savedAbility = abilityInstance;
                    savedTarget = player.GetGlobalPos();

                    yield return movement.MoveWithDelay(path);
                    telegraphController.TelegraphAbility(player.GetGlobalPos(), abilityInstance, false);
                    yield break;
                }
            }
            
            if(inavailableAttacks.Count > 0)
            {
                PickRandomAttackPoint(inavailableAttacks, false, out abilityInstance, out path);
                if (path != null)
                {
                    yield return movement.MoveWithDelay(path);
                }
            }
        }

        private void PickRandomAttackPoint(Dictionary<AbilityInstance, List<Vector2Int>> availableAttacks, bool isStricts, out AbilityInstance abilityInstance, out Queue<Vector2Int> path)
        {
            int randomAbilityNumber = UnityEngine.Random.Range(0, availableAttacks.Count() - 1);
            var randomAbilityPosPair = availableAttacks.ElementAt(randomAbilityNumber);

            abilityInstance = randomAbilityPosPair.Key;
            int randomPosNumber = UnityEngine.Random.Range(0, randomAbilityPosPair.Value.Count() - 1);

            Vector2Int randomPos = randomAbilityPosPair.Value.ElementAt(randomPosNumber);


            path = movement.GetPath(randomPos, true);
        }

        public IEnumerator ExecuteMove()
        {
            if(savedAbility != null && savedTarget.HasValue)
            {
                yield return savedAbility.UseAbility(savedTarget.GetValueOrDefault());
            }
            EndTurn();
            yield break;
        }
        private void EndTurn()
        {
            TurnPassed?.Invoke();
            savedAbility = null;
            savedTarget = null;
            telegraphController.ClearAbility();
        }

        

        public GridEntity FindPlayer()
        {
            var entities = transform.parent.GetComponentsInChildren<GridEntity>();
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Player"))
                {
                    return entity;
                }
            }

            return null;
        }
    }

}