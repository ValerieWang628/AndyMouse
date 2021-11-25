using UnityEngine;

public class EscapeToQuit : MonoBehaviour
{
    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}
