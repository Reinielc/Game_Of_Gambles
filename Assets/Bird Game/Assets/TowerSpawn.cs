using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject Tower;
    public float spawnRate = 2f;
    public float startX = 10f;
    public float spawnY = 0f;
    public float moveSpeed = 3f;
    public float minMoveY = -3f;
    public float maxMoveY = 3f;

    private float timer = 0;

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
            SpawnTower();
            timer = 0;
        }
    }

    void SpawnTower()
    {
        float randomY = Random.Range(minMoveY, maxMoveY);
        Vector3 spawnPos = new Vector3(startX, spawnY + randomY, 0);
        Quaternion rotation = Quaternion.Euler(0, 0, 90);

        GameObject newTower = Instantiate(Tower, spawnPos, rotation);

        Rigidbody2D rb = newTower.AddComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-moveSpeed, 0);
        rb.gravityScale = 0;
    }
}
