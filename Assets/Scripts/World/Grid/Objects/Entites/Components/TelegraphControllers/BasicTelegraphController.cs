using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;
using UnityEngine.EventSystems;
using System;
using ShadowWithNoPast.Entities.Abilities;
using TMPro;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class BasicTelegraphController : MonoBehaviour, ITelegraphController
    {
        
        public TelegraphElement MoveIndicator;
        public TelegraphElement AttackIndicator;


        protected GridEntity entity;
        private new SpriteRenderer renderer;
        private IMovementController movement;


        private WorldPos abilityTarget;
        private PointerActions abilityActions;
        private TelegraphElementGroup abilityTelegraphGroup;

        private GameObject availableMovesTelegraphs;

        private GameObject availableAttackTelegraphs;

        private void Start()
        {
            entity = GetComponent<GridEntity>();
            renderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IMovementController>();

            entity.Died += (entity, WordPos) => ClearAll();
        }

        public void ClearAll()
        {
            ClearAbility();
            ClearAvailableAttacks();
            ClearAvalableMoves();
        }

        public void TelegraphAvailableAttacks(AbilityTargets targets, float opacity, PointerActions actions = null)
        {
            if (AttackIndicator is null)
            {
                Debug.LogWarning("Attack indicator was not assigned, but the method creating it was called anyway.");
                return;
            }


            ClearAvailableAttacks();

            if(targets.type == AbilityTargetType.OnSelf)
            {
                Debug.LogWarning("Ability is used without the target, and you are trying to telegraph available attacks.");
            }

            availableAttackTelegraphs = new GameObject("AvailableAttacksCollection");
            availableAttackTelegraphs.transform.SetParent(transform.parent);

            if(targets.type == AbilityTargetType.Pickable)
            {
                foreach (var target in targets.positions)
                {
                    CreateAttackTelegraphElement(target, actions);
                }
            }
            if(targets.type == AbilityTargetType.Directional)
            {
                foreach(var target in targets.positions)
                {
                    CreateAttackTelegraphElement(target, actions);
                }
            }
        }

        private void CreateAttackTelegraphElement(WorldPos pos, PointerActions actions)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, AttackIndicator, availableAttackTelegraphs, actions);
            TweakRenderer(obj);
        }

        private void TweakRenderer(TelegraphElement obj)
        {
            var objRenderer = obj.GetComponent<SpriteRenderer>();
            objRenderer.sortingLayerID = renderer.sortingLayerID;
            objRenderer.sortingOrder = renderer.sortingOrder + 1;
        }

        public void ClearAvailableAttacks()
        {
            if(availableAttackTelegraphs != null)
            {
                Destroy(availableAttackTelegraphs);
                availableAttackTelegraphs = null;
            }
        }

        public void TelegraphAvailableMove(PointerActions actions = null)
        {
            if (MoveIndicator is null)
            {
                Debug.LogWarning("Move indicator was not assigned, but the method creating it was called anyway.");
                return;
            }

            ClearAvalableMoves();
            var availableMovesPos = movement.GetAvailableMoves();

            availableMovesTelegraphs = new GameObject("AvailableMovesCollection");
            availableMovesTelegraphs.transform.SetParent(transform.parent, false);


            foreach (var pos in availableMovesPos)
            {
                CreateMoveTelegraphElement(pos, actions);
            }
        }

        private void CreateMoveTelegraphElement(WorldPos pos, PointerActions actions = null)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, MoveIndicator, availableMovesTelegraphs, actions);

            TweakRenderer(obj);
        }

        private TelegraphElement InstantiateTelegraphElement(
            WorldPos pos,
            TelegraphElement element,
            GameObject parent,
            PointerActions actions = null,
            List<TelegraphElement> collection = null)
        {
            TelegraphElement obj = Instantiate(element);
            if(actions != null)
            {
                if(actions.OnClick != null)
                    obj.Clicked += actions.OnClick;
                if(actions.OnPointerEnter != null)
                    obj.PointerEntered += actions.OnPointerEnter;
                if(actions.OnPointerLeave != null)
                    obj.PointerLeft += actions.OnPointerLeave;

                obj.ToggleCollider(true);
            }
            else
            {
                obj.ToggleCollider(false);
            }
            var gridObj = obj.GetComponent<GridObject>();
            gridObj.YOffset = 0.5f;
            gridObj.SetNewPosition(pos);

            gridObj.transform.SetParent(parent.transform, false);
            obj.Renderer.sortingLayerID = renderer.sortingLayerID;

            if(collection != null)
            {
                collection.Add(obj);
            }

            return obj;
        }

        public void ClearAvalableMoves()
        {
            if (availableMovesTelegraphs != null)
            {
                Destroy(availableMovesTelegraphs);
                availableMovesTelegraphs = null;
            }
        }

        public virtual void TelegraphAbility(WorldPos target, AbilityInstance abilityInstance, bool isActive = false, PointerActions actions = null)
        {
            ClearAbility();
            target = abilityInstance.TargetToExecPos(target);
            abilityTarget = target;
            abilityActions = actions;
            TelegraphData telegraphData = abilityInstance.GetTelegraphData(target);
            abilityTelegraphGroup = telegraphData.Instantiate(renderer.sortingLayerID, actions);
            abilityTelegraphGroup.transform.SetParent(target.World.transform, true);
            if(isActive)
            {
                abilityTelegraphGroup.Highlight();
            }
            else
            {
                abilityTelegraphGroup.RemoveHighlight();
            }
        }

        public void HighlighAbility()
        {
            if(abilityTelegraphGroup != null)
            {
                abilityTelegraphGroup.Highlight();
            }
        }

        public void RemoveHighlighAbility()
        {
            if(abilityTelegraphGroup != null)
            {
                abilityTelegraphGroup.RemoveHighlight();
            }
        }

        public void ClearAbility()
        {
            if (abilityTelegraphGroup != null)
            {
                Destroy(abilityTelegraphGroup.gameObject);
            }
        }

        public void RepaintAbility(AbilityInstance instance)
        {
            TelegraphAbility(abilityTarget, instance, false, abilityActions);
        }
    }
}