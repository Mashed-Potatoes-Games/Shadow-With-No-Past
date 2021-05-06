using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using ShadowWithNoPast.Utils;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif

namespace ShadowWithNoPast.Entities
{
    [ExecuteAlways]
    public class GridEntity : GridObject
    {
        public static readonly int MAX_HEALTH = 8;

        public event Action<GridEntity> Died;

        [field: SerializeField, HideInInspector] public int MaxHealth { get; private set; }
        public int Health { get => health; private set {
                health = value;
                if(health <= 0)
                {

                    health = 0;
                    return;
                }
                if(health > MaxHealth)
                {
                    health = MaxHealth;
                }
            } 
        }
        [SerializeField]
        [HideInInspector]
        private int health;
        // This value was moved from TurnController because it was used too much across other components
        public int MoveDistance = 1;

        public IMovementController MovementController;
        public ITurnController TurnController;
        public ITelegraphController TelegraphController;
        public IEntitySpriteController SpriteController;

        [SerializeField]
        protected Healthbar healthbar;

        protected Direction facing;

        protected override void Awake()
        {
            base.Awake();
            MovementController = GetComponent<IMovementController>();
            TurnController = GetComponent<ITurnController>();
            TelegraphController = GetComponent<ITelegraphController>();
            SpriteController = GetComponent<IEntitySpriteController>();
        }


        protected override void Start()
        {
            base.Start();

            if(healthbar != null)
            {
                healthbar.SetMaxHealth(MaxHealth);
                healthbar.SetHealth(Health, false);
            }
        }

        public void SetMaxHealth(int inMaxHealth)
        {
            if(inMaxHealth > MAX_HEALTH)
            {
                Debug.LogError($"You can't set max health more({inMaxHealth}) than MAX_HEALTH, which is {MAX_HEALTH}");
                return;
            }
            if(inMaxHealth <= 0) 
            {
                Debug.LogError($"Max health {inMaxHealth} can't be 0 or lower");
            }

            MaxHealth = inMaxHealth;
            UpdateHealthbar();
        }

        public void SetHealth(int health)
        {
            Health = health;
            UpdateHealthbar();
        }

        public IEnumerator ReceiveDamage(int damage)
        {
            if (damage <= 0)
            {
                Debug.LogError("Damage can't be negative!");
            }

            Health -= damage;

            UpdateHealthbar();

            yield return SpriteController.AnimateDamage();

            if(Health == 0)
            {
                Died?.Invoke(this);
                World.RemoveAt(this, Vector);
                Destroy(gameObject);
            }

        }

        public void Heal(int healing)
        {
            if(healing <= 0)
            {
                Debug.LogError("Healing can't be negative!");
            }
            Health += healing;


            UpdateHealthbar();
        }

        private void UpdateHealthbar()
        {
            if (healthbar != null)
            {
                if(healthbar.StoredMaxHealth != MaxHealth)
                {
                    healthbar.SetMaxHealth(MaxHealth);
                }
                if(healthbar.StoredHealth != Health)
                {
                    healthbar.SetHealth(Health);
                }

                EditorUtil.UpdateInEditor(healthbar);
            }
        }

        public void FaceTo(Vector2Int targetPos)
        {
            Vector2Int direction = targetPos - Vector;

            if (direction.x > 0)
            {
                FaceTo(Direction.Right);
            }
            else if (direction.x < 0)
            {
                FaceTo(Direction.Left);
            }
        }

        //Change facing direction after a move or other interractions
        public void FaceTo(Direction direction)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = direction switch
            {
                Direction.Right => false,
                Direction.Left => true,
                _ => throw new NotImplementedException(),
            };
        }
    }
    /// <summary>
    /// Direction, the entity can face or attack.
    /// </summary>
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GridEntity))]

    public class EntityEditor : Editor
    {
        private GridEntity entity;
        private int cachedMaxHealth;
        private int cachedHealth;
        public override VisualElement CreateInspectorGUI()
        {
            entity = (GridEntity)target;
            cachedMaxHealth = entity.MaxHealth;
            cachedHealth = entity.Health;
            return base.CreateInspectorGUI();
        }
        public override void OnInspectorGUI()
        {
            var entity = (GridEntity)target;

            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            int maxHealthInput = EditorGUILayout.IntSlider("Max Health", cachedMaxHealth, 1, GridEntity.MAX_HEALTH);
            if (maxHealthInput != cachedMaxHealth)
            {
                entity.SetMaxHealth(maxHealthInput);
                cachedMaxHealth = maxHealthInput;
                EditorUtil.UpdateInEditor(entity);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            int healthInput = EditorGUILayout.IntSlider("Current Health", cachedHealth, 0, cachedMaxHealth);
            if (healthInput != cachedHealth)
            {
                entity.SetHealth(healthInput);
                cachedHealth = healthInput;
                EditorUtil.UpdateInEditor(entity);
            }

            EditorGUILayout.EndVertical();
        }

    }
#endif
}