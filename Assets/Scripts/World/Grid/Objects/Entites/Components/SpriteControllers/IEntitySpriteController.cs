using System.Collections;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public interface IEntitySpriteController
    {

        public void SetSprite(SpriteType type);
        IEnumerator AnimateDamage();
        void ResetToDefault();
    }
}