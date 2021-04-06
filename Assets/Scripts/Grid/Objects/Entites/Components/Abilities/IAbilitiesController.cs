using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowWithNoPast.Entities.Abilities
{
    interface IAbilitiesController
    {
        public event Action<AbilityInstance> AbilityUsed;

        int Count { get; }

        List<AbilityInstance> GetAbilities();
        List<AbilityInstance> GetReadyAbilities();
        void ExecutePassiveAbilities();
        
        public AbilityInstance this[int i] { get; }
        
    }
}
