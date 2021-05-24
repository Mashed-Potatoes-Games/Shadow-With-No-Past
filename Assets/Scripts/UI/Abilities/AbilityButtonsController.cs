using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities.Abilities;
using ShadowWithNoPast.Entities;

namespace ShadowWithNoPast.UI
{
    public class AbilityButtonsController : MonoBehaviour
    {
        private IAbilitiesController playerAbilities;
        [SerializeField]
        private List<AbilityUIButton> abilityButtons;
        private void Awake()
        {
        }

        private void Start()
        {
            var player = Player.Entity;

            if (player is null)
            {
                Debug.LogWarning("No player on scene, abilities buttons won't be initialized");
                return;
            }
            playerAbilities = player.GetComponent<IAbilitiesController>();

            if (playerAbilities is null)
            {
                return;
            }

            for (int i = 0; i < abilityButtons.Count; i++)
            {
                if (playerAbilities.Count <= i)
                {
                    break;
                }
                abilityButtons[i].TieToAbility(playerAbilities[i]);
            }
        }
    }
}