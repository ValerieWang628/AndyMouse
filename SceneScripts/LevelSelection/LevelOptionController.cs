using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOptionController : MonoBehaviour
{
    [SerializeField] private string levelToBeEntered;
    private bool playerIsOverlappingTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlappingTrigger = true;
            //UiManager.instance.EnablePromptText_LevelSelection(this.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlappingTrigger = false;
            //UiManager.instance.DisablePromptText_LevelSelection();
        }
    }

    private void OnMouseDown()
    {
        if (playerIsOverlappingTrigger)
        {
            SceneToolManager.instance.SwitchToScene(levelToBeEntered);
        }
    }
}
