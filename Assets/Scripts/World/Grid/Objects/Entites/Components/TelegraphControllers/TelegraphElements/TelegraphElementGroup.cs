using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShadowWithNoPast.Entities
{
    public class TelegraphElementGroup : TelegraphElement
    {
        public List<TelegraphElement> Telegraphs;

        public void TieToTelegraphs(List<TelegraphElement> telegraphs, PointerActions actions = null)
        {
            if(actions != null)
            {
            Clicked += actions.OnClick;
            PointerEntered += actions.OnPointerEnter;
            PointerLeft += actions.OnPointerLeave;
            }

            Telegraphs = telegraphs;
            foreach(TelegraphElement element in Telegraphs)
            {
                element.transform.SetParent(transform);
                element.Clicked += (elem, world) => OnPointerClick(null);
                element.PointerEntered += (elem, world) => OnPointerEnter(null);
                element.PointerLeft += (elem, world) => OnPointerExit(null);
            }
        }

        public override void Highlight()
        {
            Telegraphs.ForEach(telegraph => telegraph.Highlight());
        }

        public override void RemoveHighlight()
        {
            Telegraphs.ForEach(telegraph => telegraph.RemoveHighlight());
        }

        internal override void ToggleTextVisibilty(bool enabled)
        {
            Telegraphs.ForEach(telegraph => telegraph.ToggleTextVisibilty(enabled));
        }
    }
}
