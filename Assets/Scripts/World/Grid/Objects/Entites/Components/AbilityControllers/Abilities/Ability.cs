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
    public abstract class Ability : ScriptableObject
    {
        public abstract AbilityTargetType Type { get; }
        public virtual SpriteType ExecutionSprite => SpriteType.Attack;
        public AudioClip AbilitySound => abilitySound;
        public Sprite Icon => icon;
        public ActionWithAoe[] Actions => actions;
        public int DefaultCooldown => defaultCooldown;
        public int DistanceConstraint => defaultDistanceConstraint;

        [SerializeField]
        private AudioClip abilitySound;
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private ActionWithAoe[] actions;
        [SerializeField]
        private int defaultCooldown;
        [SerializeField]
        private int defaultDistanceConstraint;


        public virtual IEnumerator PreExecute(GridEntity caller, WorldPos target)
        {
            caller.FaceTo(target.Vector);
            caller.SpriteController.SetSprite(SpriteType.Attack);
            yield break;
        }

        public virtual List<SingleTelegraphData> GetAbilityTelegraphs(GridEntity caller, WorldPos target)
        {
            return null;
        }

        public virtual WorldPos TargetToExecPos(GridEntity caller, WorldPos target) => target;

        public virtual List<WorldPos> ChangesTargetOnMovement(WorldPos executionPos, WorldPos initialTarget)
        {
            return null;
        }

        public abstract AbilityTargets AvailableTargets(WorldPos executionPos);

        public abstract AbilityTargets AvailableAttackPoints(WorldPos target);
    }

    [Serializable]
    public class ActionWithAoe {
        [SerializeReference]
        public AbilityAction Action;
        [SerializeReference]
        public AoePattern Pattern;

        public ActionWithAoe(AbilityAction action, AoePattern pattern)
        {
            Action = action;
            Pattern = pattern;
        }
        public virtual IEnumerator Execute(GridEntity caller, WorldPos target, int effectValue)
        {
            if (Pattern == null)
            {
                yield return Action.Execute(target, effectValue);
                yield break;
            }

            var targets = Pattern.TargetToAoe(caller, target);
            var coroutinesExecFlags = new List<bool>(9);
            for (int i = 0; i < targets.Count; i++)
            {
                coroutinesExecFlags.Add(false);
                caller.StartCoroutine(
                    FlagAtTheEnd(
                        Action.Execute(targets[i], effectValue),
                        coroutinesExecFlags,
                        i));
            }

            while (!coroutinesExecFlags.All(val => val))
            {
                yield return null;
            }
        }
        static IEnumerator FlagAtTheEnd(IEnumerator func, List<bool> flags, int pos)
        {
            yield return func;
            flags[pos] = true;
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