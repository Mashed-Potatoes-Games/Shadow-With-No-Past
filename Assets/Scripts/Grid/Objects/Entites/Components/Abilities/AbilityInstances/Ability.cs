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
    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/Ability", order = 1)]
    public class Ability : ScriptableObject
    {
        public AudioClip AbilitySound;
        public Sprite Icon;
        public AbilityAction Action;

        public AbilityType Type = AbilityType.Any;
        [Range(1, 10)]
        public int DistanceConstraint = 1;
        public int DefaultCooldown = 0;
        public WorldTarget WorldTarget = WorldTarget.Current;
        public IAoePattern AoePattern;
        public Ability SecondaryEffect;

        public IEnumerator Execute(GridObject caller, TargetPos target, int effectValue)
        {
            if(target.World == null)
            {
                target.World = caller.WorldGrid;
            }
            if(!AvailableTargets(caller.GetGlobalPos()).Contains(target))
            {
                Debug.LogError("System tried to apply ability, on the target, that is out of reach!");
                yield break;
            }
            if (AoePattern is null)
            {
                Action.Execute(target, effectValue);
                yield break;
            }

            yield return ExecuteWithAoe(caller, target, effectValue);
        }

        public List<TargetPos> AvailableTargets(TargetPos executionPos)
        {
            switch (Type)
            {
                case AbilityType.OnSelf:
                    return new List<TargetPos>()
                    {
                        executionPos,
                    };
                case AbilityType.Directional:
                    return GetDirectionalTargets(executionPos);
                case AbilityType.Any:
                    return GetAnyTargets(executionPos);
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerator ExecuteWithAoe(GridObject caller, TargetPos target, int effectValue)
        {
            var directionVector = caller.Pos - target.Pos;
            var direction = CoordinateUtils.GetDirectionFromVector(directionVector);

            var AoeDirectionVectors = AoePattern.SingleToAoe(direction ?? Direction.Up);

            foreach (Vector2Int aoeDirectionVector in AoeDirectionVectors)
            {
                TargetPos aoeTarget = new TargetPos(target.World, target.Pos + aoeDirectionVector);
                Action.Execute(aoeTarget, effectValue);
            }

            yield break;
        }

        private List<TargetPos> GetDirectionalTargets(TargetPos executionPos)
        {
            var targets = new List<TargetPos>();
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var directionVector = CoordinateUtils.GetVectorFromDirection(direction);

                for(int i = 1; i <= DistanceConstraint; i++)
                {

                    Vector2Int targetDirection = directionVector * i;
                    Vector2Int targetPos = executionPos.Pos + targetDirection;
                    //TODO: Targets check!
                    targets.Add(new TargetPos(executionPos.World, targetPos));
                }

            }
            return targets;
        }

        private List<TargetPos> GetAnyTargets(TargetPos executionPos)
        {
            List<Vector2Int> targetsPos = Algorithms.BreadthFirstSearch.GetAvailableMoves(executionPos.Pos, DistanceConstraint, (Vector2Int pos) => true);
            List<TargetPos> targets = targetsPos.Select(pos => new TargetPos(executionPos.World, pos)).ToList();
            return targets;
        }
    }


    public enum AbilityType
    {
        OnSelf,
        Directional,
        Any
    }

    public enum WorldTarget
    {
        Current,
        Opposite,
        Any
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