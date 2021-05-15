using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowWithNoPast.UI
{
    [RequireComponent(typeof(Button))]
    public class AbilityUIButton : MonoBehaviour
    {
        [SerializeField]
        private Button button;
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Image abilityIcon;
        [SerializeField]
        private Image circularTint;
        [SerializeField]
        private TextMeshProUGUI cooldownText;
        [SerializeField]
        private AbilityInstance abilityInstance;

        private Color32 defaultColor;

        private void Awake()
        {
            defaultColor = button.image.color;
        }

        public void TieToAbility(AbilityInstance instance)
        {
            if (abilityInstance != null)
            {
                abilityInstance.Updated -= Redraw;
                abilityInstance.UsedWithNoTarget -= SetInUse;
            }
            abilityInstance = instance;

            abilityInstance.Updated += Redraw;
            abilityInstance.UsedWithNoTarget += SetInUse;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(UseAbility);

            Redraw();
        }

        private void UseAbility()
        {
            StartCoroutine(abilityInstance.UseAbility());
            button.image.color = defaultColor;
        }

        private void SetInUse()
        {
            var newColor = Color32.Lerp(defaultColor, new Color32(255, 255, 0, 255), 0.3f);
            button.image.color = newColor;
        }

        private void Redraw()
        {
            if (!ChangeActiveState())
            {
                return;
            }

            abilityIcon.sprite = abilityInstance.Icon;
            button.image.color = defaultColor;


            if (abilityInstance.ReadyToUse)
            {
                circularTint.fillAmount = 0;
                cooldownText.text = "";
                button.interactable = true;
                return;
            }

            float CooldownPercentile = abilityInstance.RemainingCooldown / (float)abilityInstance.CooldownOnUse;
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
}