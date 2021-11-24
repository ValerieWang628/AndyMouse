using UnityEngine;

public class BeanBulletMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float frequency;
    [SerializeField] private float magnitude;
    private Vector2 thisDirection;
    private Vector3 bulPos;

    private void Start()
    {
        bulPos = this.transform.position;
        //Destroy(gameObject, 1f);       
    }

    private void Update()
    {
        if (thisDirection != Vector2.zero)
        {
            bulPos += (Vector3)thisDirection * Time.deltaTime * moveSpeed;
            this.transform.position = bulPos + transform.up * Mathf.Sin(frequency * Time.time) * magnitude;
        }

    }

    public void ReceiveMomentum(Vector2 momentumDir)
    {
        thisDirection = momentumDir;
    }

    private void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
