using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;


        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }


    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bird collected the power-up!");
            Destroy(gameObject);
        }
    }
}
