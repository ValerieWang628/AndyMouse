using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelSelectionMovement : MonoBehaviour
{
    #region Basic

    private Rigidbody2D thisRigidBody;
    private Vector2 thisAxisInput;
    #endregion

    #region Run
    [Header("Run")]
    [SerializeField] private float runSpeed;
    [Range(0f, 30f)] [SerializeField] private float runAcceleration = 15f;
    [Range(0f, 30f)] [SerializeField] private float runDeceleration = 20f;
    [Range(0.2f, 1f)] [SerializeField] private float pow = 0.9f;
    #endregion

    private void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        #region InputUpdate

        thisAxisInput.x = Input.GetAxisRaw("Horizontal");
        thisAxisInput.y = Input.GetAxisRaw("Vertical");

        if (thisAxisInput.x == -1)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (thisAxisInput.x == 1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        #endregion
    }

    private void FixedUpdate()
    {

        #region Run

        float targetSpeed = thisAxisInput.x * runSpeed;
        float speedDiff = targetSpeed - thisRigidBody.velocity.x;
        float accRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAcceleration : runDeceleration; // if there is no input, decelerate
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accRate, pow) * Mathf.Sign(speedDiff);
        thisRigidBody.AddForce(movement * Vector2.right);
        #endregion

    }
}
