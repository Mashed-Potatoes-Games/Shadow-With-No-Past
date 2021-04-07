using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;
using UnityEngine.EventSystems;
using System;
using ShadowWithNoPast.Entities.Abilities;

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

        private GameObject availableMovesTelegraphs;
        private GameObject attackTelegraphs;

        private void Start()
        {
            entity = GetComponent<GridEntity>();
            renderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IMovementController>();
        }

        public void TelegraphAttack(List<TargetPos> targets, float opacity, Action<TargetPos> OnClickAction)
        {
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Attack indicator was not assigned, but the method creating it was called anyway.");
                return;
            }


            ClearAttack();

            attackTelegraphs = new GameObject("AvailableAttacksCollection");
            attackTelegraphs.transform.SetParent(transform.parent);

            foreach (var target in targets)
            {
                CreateAttackTelegraphElement(target.Pos, opacity, OnClickAction);
            }
        }

        private void CreateAttackTelegraphElement(Vector2Int pos, float opacity, Action<TargetPos> onClickAction)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, onClickAction, AttackIndicator, attackTelegraphs);
            TweakRenderer(obj, opacity);
        }

        private void TweakRenderer(TelegraphElement obj, float opacity)
        {
            var objRenderer = obj.GetComponent<SpriteRenderer>();
            objRenderer.sortingLayerID = renderer.sortingLayerID;
            objRenderer.sortingOrder = renderer.sortingOrder + 1;
            objRenderer.color = new Color(1, 1, 1, opacity);
        }

        public void ClearAttack()
        {
            if(attackTelegraphs != null)
            {
                Destroy(attackTelegraphs);
                attackTelegraphs = null;
            }
        }

        public void TelegraphAvailableMove(float opacity, Action<TargetPos> onClickAction)
        {
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Move indicator was not assigned, but the method creating it was called anyway.");
                return;
            }

            ClearAvalableMoves();
            var availableMovesPos = BreadthFirstSearch.GetAvailableMoves(
                entity.Pos,
                entity.MoveDistance,
                movement.IsCellFree);

            availableMovesTelegraphs = new GameObject("AvailableMovesCollection");
            availableMovesTelegraphs.transform.SetParent(transform.parent);


            foreach(var pos in availableMovesPos)
            {
                CreateMoveTelegraphElement(pos, opacity, onClickAction);
            }
        }

        private void CreateMoveTelegraphElement(Vector2Int pos, float opacity, Action<TargetPos> OnClickAction = null)
        {
            TelegraphElement obj = InstantiateTelegraphElement(pos, OnClickAction, AvailableMoveIndicator, availableMovesTelegraphs);

            TweakRenderer(obj, opacity);
        }

        private TelegraphElement InstantiateTelegraphElement(Vector2Int pos, Action<TargetPos> OnClickAction, TelegraphElement element, GameObject parent)
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
    }
}