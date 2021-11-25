using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLevelSelectionFollow : MonoBehaviour
{
    [SerializeField] private float smoothMultiplier;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 camBoundLeft;
    [SerializeField] private Vector3 camBoundRight;

    private void LateUpdate()
    {
        Vector3 camPos = this.transform.position;
        Vector3 tPos = followTarget.position;
        float calcX = Mathf.Lerp(camPos.x, tPos.x, Time.deltaTime * smoothMultiplier);
        calcX = Mathf.Clamp(calcX, camBoundLeft.x, camBoundRight.x);
        //Vector3 calcPos = new Vector3(Mathf.Lerp(camPos.x, tPos.x, Time.deltaTime * smoothMultiplier), camPos.y, -10);
        this.transform.position = new Vector3(calcX, camPos.y, -10);
    }
}
