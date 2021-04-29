using ShadowWithNoPast.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShadowWithNoPast.Entities.Abilities
{
    [Serializable]
    public abstract class Ability : ScriptableObject, IAbility
    {
        public abstract AbilityTargetType Type { get; }
        public AudioClip AbilitySound => abilitySound;
        public Sprite Icon => icon;
        public AbilityAction Action => action;
        public int DefaultCooldown => defaultCooldown;
        public int DistanceConstraint => defaultDistanceConstraint;

        [SerializeField]
        private AudioClip abilitySound;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private AbilityAction action;
        [SerializeField]
        private int defaultCooldown;
        [SerializeField]
        private int defaultDistanceConstraint;


        public IAoePattern AoePattern;

        [field: SerializeField] public Ability SecondaryEffect { get; }

        public IEnumerator Execute(GridEntity caller, WorldPos target, int effectValue)
        {
            //Excessive transformation because other functions are made to work with Vector2Pos
            //Remove this after making all of the pos-related funcitons to work with TargetPos!
            if (target.World == null)
            {
                target.World = caller.World;
            }

            var targets = TargetToAoe(caller, target);
            foreach (var targetPos in targets)
            {
                Action.Execute(targetPos, effectValue);
            }
            yield break;
        }

        public abstract AbilityTargets AvailableTargets(WorldPos executionPos);

        public abstract AbilityTargets AvailableAttackPoints(WorldPos target);

        public List<WorldPos> TargetToAoe(GridEntity caller, WorldPos target)
        {
            if (target.World is null)
            {
                target.World = caller.World;
            }
            List<WorldPos> executePositions;
            if (AoePattern is null)
            {
                executePositions = new List<WorldPos>() { target };
                return executePositions;
            }
            var directionVector = caller.Pos - target.Vector;
            var direction = CoordinateUtils.GetDirectionFromVector(directionVector);

            var aoeDirectionVectors = AoePattern.SingleToAoe(direction ?? Direction.Up);
            executePositions = aoeDirectionVectors.Select(
                direction => new WorldPos(target.World, caller.Pos + direction)
                ).ToList();
            return executePositions;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Ability))]
    public class AbilityEditor : Editor
    {
        public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        {
            Ability example = (Ability)target;

            if (example == null || example.Icon == null)
                return null;

            Texture2D texture = new Texture2D(width, height);
            EditorUtility.CopySerialized(example.Icon.texture, texture);

            return texture;
        }
    }
#endif
}