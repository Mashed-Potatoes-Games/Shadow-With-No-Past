using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AbilityUIButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image abilityIcon;
    [SerializeField]
    private Image circularTint;
    [SerializeField]
    private TMPro.TextMeshProUGUI cooldownText;
    [SerializeField]
    private AbilityInstance abilityInstance;

    public void TieToAbility(AbilityInstance instance)
    {
        if(abilityInstance != null)
        {
            abilityInstance.Updated -= Redraw;
        }
        abilityInstance = instance;

        abilityInstance.Updated += Redraw;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(UseAbility);

        Redraw();
    }

    private void UseAbility()
    {
        StartCoroutine(abilityInstance.UseAbility());
    }

    private void Redraw()
    {
        if (!ChangeActiveState())
        {
            return;
        }

        abilityIcon.sprite = abilityInstance.Icon;

        if (abilityInstance.ReadyToUse)
        {
            circularTint.fillAmount = 0;
            cooldownText.text = "";
            button.interactable = true;
            return;
        }

        float CooldownPercentile = (float)abilityInstance.RemainingCooldown / (float)abilityInstance.CooldownOnUse;
        circularTint.fillAmount = CooldownPercentile;
        cooldownText.text = abilityInstance.RemainingCooldown.ToString();
        button.interactable = false;
    }

    private bool ChangeActiveState()
    {
        if (abilityInstance is null || abilityInstance.Ability is null)
        {
            gameObject.SetActive(false);
            return false;
        }

        gameObject.SetActive(true);
        return true;
    }

    private void Start()
    {
        Redraw();
    }
}
