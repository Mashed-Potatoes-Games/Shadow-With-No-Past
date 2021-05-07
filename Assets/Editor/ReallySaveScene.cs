using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ReallySaveScene : MonoBehaviour
{

    [MenuItem("File/ReallySaveScene")]

    public static void saveScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.isDirty) print("Scene was NOT marked dirty");
        EditorSceneManager.MarkSceneDirty(currentScene);
        if (!EditorSceneManager.SaveScene(currentScene)) print("WARNING: Scene Not Saved!!!");
    }
}
