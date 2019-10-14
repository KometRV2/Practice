using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class ShortCutScene : MonoBehaviour
{
    private static readonly string[] sceneList = new string[]
    {
        "Title",
        "ModeSelect",
        "Main",
        "Demo"
    };

    [MenuItem("External/シーン切り替え/Title")]
    public static void ChangeSceneToTitle()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneList[0] + ".unity");
    }

    [MenuItem("External/シーン切り替え/ModeSelect")]
    public static void ChangeSceneToModeSelect()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneList[1] + ".unity");
    }

    [MenuItem("External/シーン切り替え/Main")]
    public static void ChangeSceneToMain()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + sceneList[2] + ".unity");
    }

    [MenuItem("External/シーン切り替え/Demo")]
    public static void ChangeSceneToDemo()
    {
        EditorSceneManager.OpenScene("Assets/Practice/" + sceneList[3] + ".unity");
    }
}
