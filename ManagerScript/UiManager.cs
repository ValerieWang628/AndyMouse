using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("LevelSelection")]
    [SerializeField] private Text promptText;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
    }

    public void EnablePromptText_LevelSelection(Vector3 targetPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);
        promptText.transform.position = screenPos;
        promptText.gameObject.SetActive(true);
    }

    public void DisablePromptText_LevelSelection()
    {
        promptText.gameObject.SetActive(false);
    }



}
