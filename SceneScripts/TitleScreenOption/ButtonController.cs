using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    void Start()
    {

    }

    public void OnStartClicked()
    {
        SceneToolManager.instance.SwitchToScene("S_Tutorial");
    }

    public void OnContinueClicked()
    {

        SceneToolManager.instance.SwitchToScene("S_LevelSelection");
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}