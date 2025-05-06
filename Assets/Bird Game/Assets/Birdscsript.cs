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
    public LogicScript logic; // Automatically assigned in Start()

    private bool isAlive = true;

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
        if (isAlive)
        {
            HandleFlap();
            ClampBirdPosition();
        }
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
}
