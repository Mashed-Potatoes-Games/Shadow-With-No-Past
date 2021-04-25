using System.Collections;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public class SpriteController : MonoBehaviour, ISpriteController
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
            spriteRenderer.sprite = sprite;
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