using UnityEngine;

public class InputController_TitleScreen : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneToolManager.instance.SwitchToScene("TitleScreen_Options");
        }
    }
}
