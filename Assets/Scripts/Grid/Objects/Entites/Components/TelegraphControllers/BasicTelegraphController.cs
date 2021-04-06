using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Algorithms;
using UnityEngine.EventSystems;
using System;

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

        private GameObject availableMoves;
        private GameObject attackTelegraphs;

        private void Start()
        {
            entity = GetComponent<GridEntity>();
            renderer = GetComponent<SpriteRenderer>();
            movement = GetComponent<IMovementController>();
        }

        public void TelegraphAttack(Vector2Int target, float opacity)
        {
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Attack indicator was not assigned, but the method creating it was called anyway.");
                return;
            }

            ClearAttack();
        }

        public void ClearAttack()
        {
            if(attackTelegraphs != null)
            {
                Destroy(attackTelegraphs);
                attackTelegraphs = null;
            }
        }

        public void TelegraphAvailableMove(float opacity, Action<Vector2Int> OnClickAction = null)
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

            availableMoves = new GameObject("AvailableMovesCollection");
            availableMoves.transform.SetParent(transform.parent);


            foreach(var pos in availableMovesPos)
            {
                CreateMoveTelegraphElement(pos, opacity, OnClickAction);
            }
        }

        private void CreateMoveTelegraphElement(Vector2Int pos, float opacity, Action<Vector2Int> OnClickAction = null)
        {
            TelegraphElement obj = Instantiate(AvailableMoveIndicator);
            obj.OnClick += OnClickAction;
            var gridObj = obj.GetComponent<GridObject>();
            gridObj.YOffset = 0.5f;
            gridObj.Pos = pos;

            gridObj.transform.SetParent(availableMoves.transform);

            var objRenderer = obj.GetComponent<SpriteRenderer>();
            objRenderer.sortingLayerID = renderer.sortingLayerID;
            objRenderer.sortingOrder = renderer.sortingOrder + 1;
            objRenderer.color = new Color(1, 1, 1, opacity);
        }

        public void ClearAvalableMoves()
        {
            if (availableMoves != null)
            {
                Destroy(availableMoves);
                availableMoves = null;
            }
        }
    }
}