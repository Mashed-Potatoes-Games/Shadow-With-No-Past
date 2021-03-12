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

        public event Action<Vector2Int> OnClick;

        private GridEntity entity;
        private new SpriteRenderer renderer;
        private IMovementController movement;

        private List<TelegraphElement> availableMoves = new List<TelegraphElement>();
        private List<TelegraphElement> attackTelegraphs = new List<TelegraphElement>();

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
            foreach (var element in attackTelegraphs)
            {
                Destroy(element);
            }
            availableMoves.Clear();
        }

        public void TelegraphAvailableMove(float opacity)
        {
            if (AvailableMoveIndicator is null)
            {
                Debug.LogWarning("Move indicator was not assigned, but the method creating it was called anyway.");
                return;
            }

            ClearAvalableMoves();
            var availableMovesPos = BreadthFirstSearch.GetAvailableMoves(
                entity.CurrentPos,
                entity.MoveDistance,
                movement.IsCellFree);
            foreach(var pos in availableMovesPos)
            {
                CreateMoveTelegraphElement(pos, opacity);
            }
        }

        private void CreateMoveTelegraphElement(Vector2Int pos, float opacity)
        {
            TelegraphElement obj = Instantiate(AvailableMoveIndicator);
            obj.OnClick += OnElementClick;
            var gridObj = obj.GetComponent<GridObject>();
            gridObj.YOffset = 0.5f;
            gridObj.CurrentPos = pos;
            var objRenderer = obj.GetComponent<SpriteRenderer>();
            objRenderer.sortingLayerID = renderer.sortingLayerID;
            objRenderer.sortingOrder = renderer.sortingOrder - 1;
            objRenderer.color = new Color(1, 1, 1, opacity);

            availableMoves.Add(obj);
        }

        private void OnElementClick(Vector2Int pos)
        {
            OnClick.Invoke(pos);
        }

        public void ClearAvalableMoves()
        {
            foreach (var element in availableMoves)
            {
                Destroy(element.gameObject);
            }
            availableMoves.Clear();
        }
    }
}