using UnityEngine;

public class TowerMove : MonoBehaviour
{
    public float moveSpeed = 0.5f;  // Default speed
    private float originalSpeed;

    void Start()
    {
        originalSpeed = moveSpeed;  // Save the initial speed
    }

    void Update()
    {
        // Move the tower leftwards every frame
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // Destroy tower if it moves off screen (adjust value if needed)
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    // Called from BirdScript coroutine to slow the tower
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    // Called from BirdScript coroutine to reset to original speed
    public void ResetSpeed()
    {
        moveSpeed = originalSpeed;
    }
}