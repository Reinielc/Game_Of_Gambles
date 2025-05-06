using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject Tower;         // Tower prefab
    public float spawnRate = 2f;     // Spawn rate (seconds)
    public float startX = 10f;       // X spawn position (right side)
    public float spawnY = 0f;        // Fixed Y position
    public float moveSpeed = 3f;     // Movement speed (leftward)
    public float minMoveY = -3f;     // Min Y random offset
    public float maxMoveY = 3f;      // Max Y random offset

    private float timer = 0;

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnTower();
            timer = 0;
        }
    }

    void SpawnTower()
    {
        // Randomize Y position within range
        float randomY = Random.Range(minMoveY, maxMoveY);
        Vector3 spawnPos = new Vector3(startX, spawnY + randomY, 0);

        // Rotate 90° to make the tower horizontal
        Quaternion rotation = Quaternion.Euler(0, 0, 90);

        // Spawn the tower
        GameObject newTower = Instantiate(Tower, spawnPos, rotation);

        // Start moving the tower left (toward player)
        Rigidbody2D rb = newTower.AddComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-moveSpeed, 0); // Leftward movement
        rb.gravityScale = 0; // Disable gravity
    }
}