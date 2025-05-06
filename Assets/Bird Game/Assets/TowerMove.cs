using UnityEngine;

public class TowerMove : MonoBehaviour
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
}
