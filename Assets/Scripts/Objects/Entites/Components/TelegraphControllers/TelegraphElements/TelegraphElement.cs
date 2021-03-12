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
    public class TelegraphElement : MonoBehaviour
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

        public void OnMouseDown()
        {
            OnClick.Invoke(GridObj.CurrentPos);
        }

        private void OnMouseEnter()
        {
            savedColorState = Renderer.color;
            Color denseColor = Renderer.color;
            denseColor.a = 1;
            Renderer.color = denseColor;
        }

        private void OnMouseExit()
        {
            Renderer.color = savedColorState;
        }
    }
}
