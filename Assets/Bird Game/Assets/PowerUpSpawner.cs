using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject PowerUp;
    public float spawnRate = 2f;
    public float startX = 10f;
    public float spawnY = 0f;
    public float moveSpeed = 3f;
    public float minMoveY = -3f;
    public float maxMoveY = 3f;

    private float timer = 0;

    private const string POWERUP_TAG = "PowerUp";

    void Update()
    {
        if (!LogicScript.Instance.gameStarted)
            return;

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnPowerUp();
            timer = 0;
        }
    }

    void SpawnPowerUp()
    {
        float randomY = Random.Range(minMoveY, maxMoveY);
        Vector3 spawnPos = new Vector3(startX, spawnY + randomY, 0);
        Quaternion rotation = Quaternion.identity;

        GameObject newPowerUp = Instantiate(PowerUp, spawnPos, rotation);
        newPowerUp.tag = POWERUP_TAG;

        Rigidbody2D rb = newPowerUp.AddComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-moveSpeed, 0);
        rb.gravityScale = 0;
        rb.isKinematic = true;

        CircleCollider2D col = newPowerUp.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
    }
}
