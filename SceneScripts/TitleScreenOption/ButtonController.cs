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
}