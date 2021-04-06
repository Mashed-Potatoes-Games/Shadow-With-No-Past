using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    public interface IAoePattern
    {
        public List<Vector2Int> SingleToAoe(Direction direction);
    }
}