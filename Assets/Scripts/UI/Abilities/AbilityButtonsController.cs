using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadowWithNoPast.Entities.Abilities;

public class AbilityButtonsController : MonoBehaviour
{
    private IAbilitiesController playerAbilities;
    [SerializeField]
    private List<AbilityUIButton> abilityButtons;
    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if(player is null)
        {
            Debug.LogWarning("No player on scene, abilities buttons won't be initialized");
            return;
        }
        playerAbilities = player.GetComponent<IAbilitiesController>();
    }

    private void Start()
    {
        if(playerAbilities is null)
        {
            return;
        }

        for(int i = 0; i < abilityButtons.Count; i++)
        {
            if(playerAbilities.Count <= i)
            {
                break;
            }
            abilityButtons[i].TieToAbility(playerAbilities[i]);
        }
    }
}
