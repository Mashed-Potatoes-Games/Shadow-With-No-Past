using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridObject))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class TelegraphElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector2Int> OnClick;

        [NonSerialized]
        public GridObject GridObj;
        public SpriteRenderer Renderer;
        private Color savedColorState;

        void Start()
        {
            GridObj = GetComponent<GridObject>();
            Renderer = GetComponent<SpriteRenderer>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(GridObj.Pos);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            savedColorState = Renderer.color;
            Color denseColor = Renderer.color;
            denseColor.a = 1;
            Renderer.color = denseColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Renderer.color = savedColorState;
        }
    }
}
