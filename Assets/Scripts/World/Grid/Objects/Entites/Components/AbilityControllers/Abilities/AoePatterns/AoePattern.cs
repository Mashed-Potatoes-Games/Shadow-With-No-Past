using ShadowWithNoPast.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "NewAOEPattern", menuName = "Abilities/AOE Pattern", order = 2)]
    public class AoePattern : ScriptableObject, IAoePattern
    {
        public class AoePatternIsEmptyException : Exception { }
        public List<Vector2Int> UpAttackPattern;
        public List<Vector2Int> SingleToAoe(Direction direction)
        {
            if (UpAttackPattern is null || UpAttackPattern.Count == 0)
            {
                throw new AoePatternIsEmptyException();
            }

            if (direction == Direction.Up)
            {
                return UpAttackPattern;
            }

            var transfrormedAttackPattern = new List<Vector2Int>();
            foreach (var pos in UpAttackPattern)
            {
                transfrormedAttackPattern.Add(CoordinateUtils.RotateFromUpDirectionTo(pos, direction));
            }
            return transfrormedAttackPattern;
        }
    }
}