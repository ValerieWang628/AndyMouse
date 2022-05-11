using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParaLayer
{
    public GameObject layerObj;
    public float latencyMultiplier;
    [HideInInspector] public float xOffsetToTarget;
}

public class CamFollow : MonoBehaviour
{
    [Header("Basic")]
    [SerializeField] private float smoothMultiplier;
    [SerializeField] private Transform followTarget;
    [Header("Bound")]
    [SerializeField] private Vector3 camBoundLeft;
    [SerializeField] private Vector3 camBoundRight;
    [Header("Parallax")]
    [SerializeField] private List<ParaLayer> layers;

    private void Start()
    {
        #region InitOffset

        // initiaze background offset so that WYSIWYG
        foreach (ParaLayer aLayer in layers)
        {
            aLayer.xOffsetToTarget = aLayer.layerObj.transform.position.x - followTarget.position.x;
        }
        #endregion
    }

    private void LateUpdate()
    {
        #region CameraFollow

        Vector3 camPos = this.transform.position;
        Vector3 tPos = followTarget.position;
        float calcX = Mathf.Lerp(camPos.x, tPos.x, Time.deltaTime * smoothMultiplier);
        calcX = Mathf.Clamp(calcX, camBoundLeft.x, camBoundRight.x);
        this.transform.position = new Vector3(calcX, camPos.y, -10);
        #endregion


        #region BackgroundParallax

        foreach (ParaLayer aLayer in layers)
        {
            Vector3 aLayerPos = aLayer.layerObj.transform.position;
            aLayer.layerObj.transform.position = new Vector3(tPos.x * aLayer.latencyMultiplier + aLayer.xOffsetToTarget, aLayerPos.y, aLayerPos.z);
        }
        #endregion
    }

}
