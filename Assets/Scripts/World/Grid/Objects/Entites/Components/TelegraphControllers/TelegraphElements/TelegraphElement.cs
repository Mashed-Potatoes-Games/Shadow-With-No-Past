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
        public event Action<TelegraphElement, WorldPos> Clicked;
        public event Action<TelegraphElement, WorldPos> PointerEntered;
        public event Action<TelegraphElement, WorldPos> PointerLeft;

        public GridObject GridObj;
        public SpriteRenderer Renderer;
        public TextMeshProUGUI Text;
        public new Collider2D collider;
        private Color? savedColorState;

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

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, GridObj.Pos);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEntered?.Invoke(this, GridObj.Pos);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerLeft?.Invoke(this, GridObj.Pos);
        }

        public virtual void Highlight()
        {
            ToggleTextVisibilty(true);
            savedColorState = Renderer.color;
            Color denseColor = Renderer.color;
            denseColor.a = 1;
            Renderer.color = denseColor;
        }

        public virtual void RemoveHighlight()
        {
            ToggleTextVisibilty(false);
            if(savedColorState != null)
            {
                Renderer.color = savedColorState.Value;
            }
        }

        internal void ToggleCollider(bool enabled)
        {
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }

        internal virtual void ToggleTextVisibilty(bool enabled)
        {
            if (Text != null)
            {
                Text.enabled = enabled;
            }
        }
    }

    public class PointerActions 
    {
        public Action<TelegraphElement, WorldPos> OnClick;
        public Action<TelegraphElement, WorldPos> OnPointerEnter;
        public Action<TelegraphElement, WorldPos> OnPointerLeave;

        public PointerActions() { }

        public PointerActions(Action<TelegraphElement, WorldPos> onClick,
                              Action<TelegraphElement, WorldPos> pointerEntered,
                              Action<TelegraphElement, WorldPos> pointerLeft)
        {
            OnClick = onClick;
            OnPointerEnter = pointerEntered;
            OnPointerLeave = pointerLeft;
        }
    }
}
