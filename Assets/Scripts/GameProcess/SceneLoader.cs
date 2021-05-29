using ShadowWithNoPast.Entities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowWithNoPast.GameProcess
{
    public class SceneLoader : MonoBehaviour
    {
        public TransitionObject transition;

        public void SwitchScene(SceneName scene)
        {
            transition.FadeOut(() => StartCoroutine(LoadSceneAsync(scene)));
        }

        internal void RestartLevel()
        {
            transition.FadeOut(() => StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().name)));
        }

        public IEnumerator LoadSceneAsync(SceneName scene)
        {
            yield return LoadSceneAsync(scene.ToString());
        }

        public IEnumerator LoadSceneAsync(string name)
        {
            SceneManager.LoadSceneAsync(name);
            yield return null;
        }
    }
}
public enum SceneName
{
    MainMenu,
    Loading,
    Level1,
    Level2,
    Level3
}