using UnityEngine;

public class BananaBulletMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector2 thisDirection;

    private void Update()
    {
        this.transform.position += (Vector3)thisDirection * moveSpeed * Time.deltaTime;
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
