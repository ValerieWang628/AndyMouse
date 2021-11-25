using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcLevelSelectionController : MonoBehaviour
{

    private void OnMouseDown()
    {
            SceneToolManager.instance.SwitchToScene("S_Shop");
    }
}
