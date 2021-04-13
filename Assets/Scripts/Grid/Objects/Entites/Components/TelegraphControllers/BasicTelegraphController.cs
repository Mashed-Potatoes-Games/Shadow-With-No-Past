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
        
        public TelegraphElement AvailableMoveIndicator;
        public TelegraphElement AttackIndicator;


        private GridEntity entity;
        private new SpriteRenderer renderer;
        private IMovementController movement;

        private GameObject abilityTelegraphs;
        private List<TelegraphElement> abilityTelegraphElements = new List<TelegraphElement>();
        private GameObject availableMovesTelegraphs;
        private GameObject availableAttackTelegraphs;

        private void Start()
        {
            entity = GetComponent<GridEntity>();
            renderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IMovementController>();
        }

        public void TelegraphAvailableAttacks(List<TargetPos> targets, float opacity, PointerActions actions = null)
        {
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Attack indicator was not assigned, but the method creating it was called anyway.");
                return;
            }


            ClearAvailableAttacks();

            availableAttackTelegraphs = new GameObject("AvailableAttacksCollection");
            availableAttackTelegraphs.transform.SetParent(transform.parent);

            foreach (var target in targets)
            {
                CreateAttackTelegraphElement(target.Pos, actions);
            }
        }

        private void CreateAttackTelegraphElement(Vector2Int pos, PointerActions actions)
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
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Move indicator was not assigned, but the method creating it was called anyway.");
                return;
            }

            ClearAvalableMoves();
            var availableMovesPos = movement.GetAvailableMoves();

            availableMovesTelegraphs = new GameObject("AvailableMovesCollection");
            availableMovesTelegraphs.transform.SetParent(transform.parent);


            foreach (var pos in availableMovesPos)
            {
                CreateMoveTelegraphElement(pos, actions);
            }
        }

        private void CreateMoveTelegraphElement(Vector2Int pos, PointerActions actions = null)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, AvailableMoveIndicator, availableMovesTelegraphs, actions);

            TweakRenderer(obj);
        }

        private TelegraphElement InstantiateTelegraphElement(
            Vector2Int pos,
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
            gridObj.Pos = pos;

            gridObj.transform.SetParent(parent.transform);
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

        public void TelegraphAbility(TargetPos target, AbilityInstance abilityInstance, bool showAttackValue = false, PointerActions actions = null)
        {
            ClearAbility();
            var telegraphDict = abilityInstance.GetTelegraphData(target);
            foreach(var pair in telegraphDict)
            {
                TelegraphElement elementPrefab = pair.Key.Element;
                int value = pair.Key.Value;

                abilityTelegraphs = new GameObject("AttackTelegraphs");
                abilityTelegraphs.transform.SetParent(transform.parent);

                foreach (var targetPos in pair.Value)
                {
                    TelegraphElement elementInstance = InstantiateTelegraphElement(
                        targetPos.Pos,
                        elementPrefab,
                        abilityTelegraphs,
                        null,
                        abilityTelegraphElements);

                    elementInstance.SetTextValue(value);
                    if(!showAttackValue)
                    {
                        elementInstance.ToggleTextVisibilty(false);
                    }
                }
            }
        }

        public void HighlighAbility()
        {
            foreach(var telegraphElem in abilityTelegraphElements)
            {
                telegraphElem.ToggleTextVisibilty(true);
            }
        }

        public void RemoveHighlighAbility()
        {
            foreach (var telegraphElem in abilityTelegraphElements)
            {
                telegraphElem.ToggleTextVisibilty(false);
            }
        }

        public void ClearAbility()
        {
            if (abilityTelegraphs != null)
            {
                Destroy(abilityTelegraphs);
                abilityTelegraphs = null;
                abilityTelegraphElements.Clear();
            }
        }
    }
}