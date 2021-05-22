using ShadowWithNoPast.GameProcess;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private SceneLoader loader;

    // Start is called before the first frame update
    void Awake()
    {
        button.onClick.AddListener(StartSceneLoading);
    }

    private void StartSceneLoading()
    {
        button.interactable = false;
        loader.SwitchScene(SceneName.Level1);
    }

    private void Notify(AsyncOperation obj)
    {
        Debug.Log("Scene loaded!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
