using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowWithNoPast.Entities.Abilities
{
    public interface IAbilitiesController
    {
        public event Action<AbilityInstance> AbilityUsed;
        event Action<AbilityInstance> AbilityUsedWithNoTarget;

        int Count { get; }

        List<AbilityInstance> GetAbilities();
        List<AbilityInstance> GetReadyAbilities();
        void ExecutePassiveAbilities();
        
        public AbilityInstance this[int i] { get; }
        
    }
}
