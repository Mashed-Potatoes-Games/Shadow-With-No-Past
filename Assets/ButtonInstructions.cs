using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonInstructions : MonoBehaviour
{
    private Button button;
    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    protected virtual void OnDestroy()
    {
        button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
