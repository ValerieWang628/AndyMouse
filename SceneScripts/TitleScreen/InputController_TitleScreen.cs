using UnityEngine;

public class InputController_TitleScreen : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown && 
            !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            SceneToolManager.instance.SwitchToScene("S_TitleScreen_Options");
        }
    }
}
