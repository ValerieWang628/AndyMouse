using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Basic

    private Rigidbody2D thisRigidBody;
    private Vector2 thisAxisInput;
    private Vector2 thisFaceDirection;
    #endregion

    #region Run
    [Header("Run")]
    [SerializeField] private float runSpeed;
    [Range(0f, 30f)] [SerializeField] private float runAcceleration = 15f;
    [Range(0f, 30f)] [SerializeField] private float runDeceleration = 20f;
    [Range(0.2f, 1f)][SerializeField] private float pow = 0.9f;
    [Space(10)] [SerializeField] private float frictionAmount = 0.15f;
    #endregion

    #region Jump
    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [Range(0f, 1f)] [SerializeField] private float airControlMultiplier = 0.6f;
    private bool isJumping = false;
    [Space(10)]
    [Range(0f, 0.3f)] [SerializeField] private float coyoteDuration = 0.15f;
    private float coyoteTimer;
    [Range(0f, 0.3f)] [SerializeField] private float jumpBufferDuration = 0.1f;
    private float jumpBufferTimer;
    #endregion

    #region GroundCheck
    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groudLayer;
    #endregion

    #region NormalShoot
    [Header("NormalShoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private float bulletCoolDownDuration;
    private float bulletCoolDownTimer;
    #endregion

    #region PowerShoot - BeanWave
    [Header("PowerShoot - BeanWave")]
    [SerializeField] private int beanNum = 30;
    [SerializeField] private float beanInterval = 0.08f; //shoot freq
    [SerializeField] private float waveMagnitudeMultiplier = 2f;

    private bool isBeanActivated = true; // accumulate after a few normal hits
    private Vector3 beanBulletSpawnPoint;
    [SerializeField] private GameObject beanBulletPrefab;
    [SerializeField] private AnimationCurve beanPointMoveCurve;
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
            thisFaceDirection = Vector2.left;
        }
        else if (thisAxisInput.x == 1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            thisFaceDirection = Vector2.right;
        }
        #endregion


        #region GroundCheck

        bool isGrounded = (Physics2D.OverlapCircle(groundCheckPoint.position, 0.25f, groudLayer));
        isJumping = (isGrounded) ? false : true;
        #endregion


        #region CoyoteTimer

        if (isGrounded) { coyoteTimer = coyoteDuration; } else { coyoteTimer -= Time.deltaTime; }
        #endregion


        #region JumpBufferTimer

        // if I pressed jump this amount of time early ( => buffertime), I can still jump
        if(Input.GetButtonDown("Jump")) { jumpBufferTimer = jumpBufferDuration; } else { jumpBufferTimer -= Time.deltaTime; }
        #endregion


        #region Jump

        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, jumpForce);
            //thisRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpBufferTimer = 0;
        }
        #endregion


        #region ShootBullet

        if (Input.GetButton("Fire1"))
        {
            if (bulletCoolDownTimer <= 0)
            {
                GameObject aBullet = PoolManager.instance.GetObjFromPool("Bullet");
                aBullet.SetActive(true);
                aBullet.transform.position = bulletPoint.position;
                aBullet.GetComponent<BananaBulletMovement>().ReceiveMomentum(thisFaceDirection);

                bulletCoolDownTimer = bulletCoolDownDuration;
            }
            else
            {
                bulletCoolDownTimer -= Time.deltaTime;
            }
        }
        #endregion


        #region ShootBeanBullet

        if (Input.GetButtonDown("Fire2"))
        {
            if (isBeanActivated)
            {
                StartCoroutine(ShootBeanBullets(beanNum, bulletPoint.position, thisFaceDirection));
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {

        #region Run

        float targetSpeed = thisAxisInput.x * runSpeed;
        float speedDiff = targetSpeed - thisRigidBody.velocity.x;
        float accRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAcceleration : runDeceleration; // if there is no input, decelerate
        if (isJumping) { accRate *= airControlMultiplier; }
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accRate, pow) * Mathf.Sign(speedDiff);
        thisRigidBody.AddForce(movement * Vector2.right);
        #endregion


        #region Friction
        if (!isJumping && Mathf.Abs(thisAxisInput.x) < 0.01f)
        {
            float aFriction = Mathf.Min(Mathf.Abs(thisRigidBody.velocity.x), Mathf.Abs(frictionAmount));
            aFriction *= Mathf.Sign(thisRigidBody.velocity.x);
            thisRigidBody.AddForce(aFriction * (-1) * Vector2.right, ForceMode2D.Impulse);
        }
        #endregion


    }
 
    private void Shoot(int i, bool isUp, Vector3 offsetPoint, Vector3 faceDir)
    {
        int dir = (isUp) ? 1 : -1;

       //bullet point is offset
       beanBulletSpawnPoint = offsetPoint + new Vector3(0, beanPointMoveCurve.Evaluate(i) * waveMagnitudeMultiplier * dir, 0);

        GameObject aBullet = PoolManager.instance.GetObjFromPool("BeanBullet");
        aBullet.SetActive(true);
        aBullet.transform.position = beanBulletSpawnPoint;
        aBullet.GetComponent<BananaBulletMovement>().ReceiveMomentum(faceDir);
    }

    private IEnumerator ShootBeanBullets(int repeatCount, Vector3 offsetPoint, Vector3 faceDir)
    {
        for (int i = 0; i < repeatCount + 1; i++)
        {
            Shoot(i, true, offsetPoint, faceDir);
            Shoot(i, false, offsetPoint, faceDir);
            yield return new WaitForSeconds(beanInterval);
        }
    }

}
