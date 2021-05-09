using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    public interface IAbility
    {
        AbilityTargetType Type { get; }
        AudioClip AbilitySound { get; }
        Sprite Icon { get; }
        AbilityAction Action { get; }
        Ability SecondaryEffect { get; }
        int DefaultCooldown { get; }
        int DistanceConstraint { get; }

        AbilityTargets AvailableTargets(WorldPos executionPos);
        AbilityTargets AvailableAttackPoints(WorldPos target);
        IEnumerator Execute(AbilityArgs args);
        List<WorldPos> TargetToAoe(GridEntity caller, WorldPos target);
    }

    public struct AbilityTargets
    {
        public AbilityTargetType type;
        public List<WorldPos> positions;

        public AbilityTargets(AbilityTargetType type, List<WorldPos> positions)
        {
            this.type = type;
            this.positions = positions;
        }
    }

    public enum AbilityTargetType
    {
        OnSelf,
        Directional,
        Pickable
    }

    public struct AbilityArgs
    {
        public GridEntity Caller;
        public WorldPos Target;
        public int EffectValue;
        public int DistanceConstraint;
    }
}