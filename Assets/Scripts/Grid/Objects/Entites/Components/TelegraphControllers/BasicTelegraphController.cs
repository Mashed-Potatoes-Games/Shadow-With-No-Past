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

        private GameObject attackTelegraphs;
        private GameObject availableMovesTelegraphs;
        private GameObject availableAttackTelegraphs;

        private void Start()
        {
            entity = GetComponent<GridEntity>();
            renderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IMovementController>();
        }

        public void TelegraphAvailableAttacks(List<TargetPos> targets, float opacity, Action<TargetPos> OnClickAction)
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
                CreateAttackTelegraphElement(target.Pos, OnClickAction);
            }
        }

        private void CreateAttackTelegraphElement(Vector2Int pos, Action<TargetPos> onClickAction)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, AttackIndicator, availableAttackTelegraphs, onClickAction);
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

        public void TelegraphAvailableMove(Action<TargetPos> onClickAction)
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
                CreateMoveTelegraphElement(pos, onClickAction);
            }
        }

        private void CreateMoveTelegraphElement(Vector2Int pos, Action<TargetPos> OnClickAction = null)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, AvailableMoveIndicator, availableMovesTelegraphs, OnClickAction);

            TweakRenderer(obj);
        }

        private TelegraphElement InstantiateTelegraphElement(
            Vector2Int pos,
            TelegraphElement element,
            GameObject parent,
            Action<TargetPos> OnClickAction = null)
        {
            TelegraphElement obj = Instantiate(element);
            if(OnClickAction != null)
            {
                obj.OnClick += OnClickAction;
            }
            else
            {
                obj.GetComponent<Collider2D>().enabled = false;
            }
            var gridObj = obj.GetComponent<GridObject>();
            gridObj.YOffset = 0.5f;
            gridObj.Pos = pos;

            gridObj.transform.SetParent(parent.transform);
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

        public void TelegraphAttack(TargetPos target, AbilityInstance abilityInstance)
        {
            ClearAttack();
            var telegraphDict = abilityInstance.GetTelegraphData(target);
            foreach(var pair in telegraphDict)
            {
                TelegraphElement elementPrefab = pair.Key.Element;
                int value = pair.Key.Value;

                attackTelegraphs = new GameObject("AttackTelegraphs");
                attackTelegraphs.transform.SetParent(transform.parent);

                foreach (var targetPos in pair.Value)
                {
                    TelegraphElement elementInstance = InstantiateTelegraphElement(
                        targetPos.Pos,
                        elementPrefab,
                        attackTelegraphs);

                    if(value > 0)
                    {
                        var textComponent = elementInstance.gameObject.AddComponent<TextMeshProUGUI>();
                        textComponent.text = value.ToString();
                    }
                }
            }
        }

        public void ClearAttack()
        {
            if (attackTelegraphs != null)
            {
                Destroy(attackTelegraphs);
                attackTelegraphs = null;
            }
        }
    }
}