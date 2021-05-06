using System.Collections;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public class BasicEntitySpriteController : MonoBehaviour, IEntitySpriteController
    {
        public SpritesCollection Sprites;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(SpriteType type) {
            var sprite = GetSprite(type);

            if(sprite is null)
            {
                Debug.LogError($"Sprite {type} is not set");
                return;
            }
            if(spriteRenderer == null)
            {
                return;
            }
            spriteRenderer.sprite = sprite;
        }
        public IEnumerator AnimateDamage()
        {
            SetSprite(SpriteType.Damage);
            yield return new WaitForSeconds(0.5f);
            ResetToDefault();
        }

        public void ResetToDefault()
        {
            SetSprite(SpriteType.Idle);
        }

        private Sprite GetSprite(SpriteType type)
        {
            return type switch
            {
                SpriteType.Idle => Sprites.Idle,
                SpriteType.Defence => Sprites.Defence,
                SpriteType.Attack => Sprites.Attack,
                SpriteType.Damage => Sprites.Damage,
                _ => Sprites.Idle
            };
        }
    }
}