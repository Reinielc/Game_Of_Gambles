using System.Collections;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D myRigidbody;
    public float flapStrength = 10f;

    [Header("Screen Boundaries")]
    public float topBuffer = 1f;
    public float bottomBuffer = 1f;

    private float screenTop;
    private float screenBottom;

    [Header("Game Logic")]
    public LogicScript logic;

    private bool isAlive = true;

    private const string POWERUP_TAG = "PowerUp";
    private bool isSlowActive = false;

    void Start()
    {
        CalculateScreenBounds();

        if (logic == null)
        {
            logic = FindObjectOfType<LogicScript>();
            if (logic == null)
            {
                Debug.LogError("LogicScript not found in scene.");
            }
        }
    }

    void Update()
    {
        if (!LogicScript.Instance.gameStarted || !isAlive)
            return;

        HandleFlap();
        ClampBirdPosition();
    }

    void HandleFlap()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
        }
    }

    void ClampBirdPosition()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, screenBottom, screenTop);
        transform.position = clampedPosition;
    }

    void CalculateScreenBounds()
    {
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - topBuffer;
        screenBottom = Camera.main.ScreenToWorldPoint(Vector3.zero).y + bottomBuffer;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            isAlive = false;
            logic?.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(POWERUP_TAG))
        {
            Debug.Log("PowerUp collected!");
            Destroy(other.gameObject);

            if (!isSlowActive)
            {
                StartCoroutine(SlowDownTowers(0.2f, 2f));
            }
        }
    }

    private IEnumerator SlowDownTowers(float slowSpeed, float duration)
    {
        isSlowActive = true;

        TowerMove[] towers = FindObjectsOfType<TowerMove>();

        foreach (TowerMove tower in towers)
        {
            tower.SetSpeed(slowSpeed);
        }

        yield return new WaitForSeconds(duration);

        foreach (TowerMove tower in towers)
        {
            tower.ResetSpeed();
        }

        isSlowActive = false;
    }
}
