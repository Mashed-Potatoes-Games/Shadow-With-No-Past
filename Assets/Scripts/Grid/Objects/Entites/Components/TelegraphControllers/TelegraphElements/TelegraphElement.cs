using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridObject))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TelegraphElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<TelegraphElement, TargetPos> Clicked;
        public event Action<TelegraphElement, TargetPos> PointerEntered;
        public event Action<TelegraphElement, TargetPos> PointerLeft;

        [NonSerialized]
        public GridObject GridObj;
        [NonSerialized]
        public SpriteRenderer Renderer;
        public TextMeshProUGUI Text;
        public new Collider2D collider;
        private Color savedColorState;

        void Awake()
        {
            GridObj = GetComponent<GridObject>();
            Renderer = GetComponent<SpriteRenderer>();
            TryGetComponent(out collider);
        }

        public void SetTextValue(int value)
        {
            if (Text != null)
            {
                Text.text = value.ToString();
            }
        }

        public void HideText()
        {
            if (Text != null)
            {
                Text.enabled = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, GridObj.GetGlobalPos());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(this, GridObj.GetGlobalPos());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerLeft?.Invoke(this, GridObj.GetGlobalPos());
        }

        public void Highlight()
        {
            savedColorState = Renderer.color;
            Color denseColor = Renderer.color;
            denseColor.a = 1;
            Renderer.color = denseColor;
        }

        public void RemoveHighligh()
        {
            Renderer.color = savedColorState;
        }

        internal void ToggleCollider(bool enabled)
        {
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }

        internal void ToggleTextVisibilty(bool enabled)
        {
            if (Text != null)
            {
                Text.enabled = enabled;
            }
        }
    }

    public class PointerActions 
    {
        public Action<TelegraphElement, TargetPos> OnClick;
        public Action<TelegraphElement, TargetPos> OnPointerEnter;
        public Action<TelegraphElement, TargetPos> OnPointerLeave;

        public PointerActions() { }

        public PointerActions(Action<TelegraphElement, TargetPos> onClick,
                              Action<TelegraphElement, TargetPos> pointerEntered,
                              Action<TelegraphElement, TargetPos> pointerLeft)
        {
            OnClick = onClick;
            OnPointerEnter = pointerEntered;
            OnPointerLeave = pointerLeft;
        }
    }
}
