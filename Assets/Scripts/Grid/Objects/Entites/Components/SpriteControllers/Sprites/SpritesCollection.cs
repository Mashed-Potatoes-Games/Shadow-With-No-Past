using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [CreateAssetMenu(fileName = "Sprite", menuName = "Sprites/Sprite", order = 1)]

    public class SpritesCollection : ScriptableObject
    {
        public Sprite Idle;
        public Sprite Defence;
        public Sprite Attack;
        public Sprite Damage;
    }

    public enum SpriteType {
        Idle,
        Defence,
        Attack,
        Damage
    }
}