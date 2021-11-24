using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D thisRigidBody;
    private Vector2 thisAxisInput;

    [Header("Run")]
    [SerializeField] private float runSpeed;
    [Range(0f, 30f)] [SerializeField] private float runAcceleration = 15f;
    [Range(0f, 30f)] [SerializeField] private float runDeceleration = 20f;
    [Range(0.2f, 1f)][SerializeField] private float pow = 0.9f;
    [Space(10)] [SerializeField] private float frictionAmount = 0.15f;


    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [Range(0f, 1f)] [SerializeField] private float airControlMultiplier = 0.6f;
    private bool isJumping = false;
    [Space(10)]
    [Range(0f, 0.3f)] [SerializeField] private float coyoteDuration = 0.15f;
    private float coyoteTimer;
    [Range(0f, 0.3f)] [SerializeField] private float jumpBufferDuration = 0.1f;
    private float jumpBufferTimer;


    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groudLayer;


    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private Transform bulletPointCeiling;
    private Vector3 ceilingPos;
    private int dir;
    [SerializeField] private Transform bulletPointFloor;
    private Vector3 beanBulletPoint;
    [SerializeField] private float bulletCoolDownDuration;
    private float bulletCoolDownTimer;

    [Space(10)]
    [SerializeField] private GameObject beanBulletPrefab;
    [SerializeField] private AnimationCurve beanPointMovement;

    private void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        #region InputUpdate

        thisAxisInput.x = Input.GetAxisRaw("Horizontal");
        thisAxisInput.y = Input.GetAxisRaw("Vertical");
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
                aBullet.GetComponent<BananaBulletMovement>().ReceiveMomentum(Vector2.right);

                //GameObject aBullet = Instantiate(bulletPrefab, bulletPoint.position, Quaternion.identity);
                //aBullet.GetComponent<BananaBulletScript>().ReceiveMomentum(Vector2.right);
                bulletCoolDownTimer = bulletCoolDownDuration;
            }
            else
            {
                bulletCoolDownTimer -= Time.deltaTime;
            }
            //InvokeRepeating("Shoot", 0.15f, 0.15f);

            #endregion
        }

        if (Input.GetButtonDown("Fire2"))
        {
            beanBulletPoint = bulletPoint.position;
            StartCoroutine(ShootBeanBullets(30));
        }
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

    private void Shoot(int i)
    {
        ceilingPos = bulletPointCeiling.position;

        //if (beanBulletPoint.y >= ceilingPos.y) { dir = -1; }
        //if (beanBulletPoint.y <= bulletPoint.position.y) { dir = 1;}

        //beanBulletPoint += transform.up * 0.2f * dir;
        //beanBulletPoint += transform.up * (beanPointMovement.Evaluate(i)/10) * dir;
        beanBulletPoint = bulletPoint.position + new Vector3(0, beanPointMovement.Evaluate(i), 0);

        GameObject aBullet = Instantiate(beanBulletPrefab, beanBulletPoint, Quaternion.identity);
        aBullet.GetComponent<BananaBulletMovement>().ReceiveMomentum(Vector2.right);
    }

    private IEnumerator ShootBeanBullets(int repeatCount)
    {
        for (int i = 0; i < repeatCount + 1; i++)
        {
            Shoot(i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
