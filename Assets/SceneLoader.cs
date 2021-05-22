using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadowWithNoPast.GameProcess
{
    public class SceneLoader : MonoBehaviour
    {
        public TransitionObject transition;

        private SceneName loading = SceneName.Loading;

        public void SwitchScene(SceneName scene)
        {
            transition.FadeOut(() => StartCoroutine(LoadSceneAsync(scene)));
        }

        public IEnumerator LoadSceneAsync(SceneName scene)
        {
            SceneManager.LoadSceneAsync(scene.ToString());
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