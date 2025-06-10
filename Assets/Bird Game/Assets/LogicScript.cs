using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public static LogicScript Instance;

    public float playerScore = 0f;
    public Text multiplierText;
    public Text balanceText;
    public Text cashedInText;
    public GameObject gameOverScreen;
    public GameObject startButton;
    public InputField cashInInput;

    private float timer = 0f;
    private float scoreInterval = 10f;
    private bool gameIsOver = false;
    public bool gameStarted = false;

    private int balance;
    private int cashedInAmount = 0;
    private string currentUser;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");

        // ✅ 如果没有该用户的余额记录，则初始化为 10000
        string balanceKey = currentUser + "_Balance";
        if (!PlayerPrefs.HasKey(balanceKey))
        {
            PlayerPrefs.SetInt(balanceKey, 10000);
            PlayerPrefs.Save();
        }

        balance = PlayerPrefs.GetInt(balanceKey, 10000);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);

        UpdateMultiplierText();
        UpdateBalanceText();
        UpdateCashedInText();
    }

    void Update()
    {
        if (gameStarted && !gameIsOver)
        {
            timer += Time.deltaTime;
            if (timer >= scoreInterval)
            {
                AddScore();
                timer = 0f;
            }
        }
    }

    public void AddScore()
    {
        playerScore += 0.5f;
        UpdateMultiplierText();
        UpdateCashedInText();
    }

    private void UpdateMultiplierText()
    {
        if (multiplierText != null)
            multiplierText.text = "Multiplier X: " + playerScore.ToString("F2");
    }

    private void UpdateBalanceText()
    {
        if (balanceText != null)
            balanceText.text = "Balance: " + balance.ToString();
    }

    private void UpdateCashedInText()
    {
        if (cashedInText != null)
        {
            float cashedInTotal = cashedInAmount * playerScore;
            cashedInText.text = "$: " + cashedInTotal.ToString("F2");
        }
    }

    public void restartGame()
    {
        Debug.Log("RestartGame called!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        Debug.Log("Game Over triggered!");
        gameIsOver = true;

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        int earned = Mathf.FloorToInt(playerScore * 10f);
        balance += earned;

        SaveBalance();
        UpdateBalanceText();
    }

    public void StartGame()
    {
        Debug.Log("StartGame called!");

        if (cashInInput != null)
        {
            if (int.TryParse(cashInInput.text, out int inputAmount))
            {
                if (inputAmount > balance)
                {
                    Debug.LogWarning("You don't have enough balance!");
                    return;
                }
                if (inputAmount <= 0)
                {
                    Debug.LogWarning("Enter a positive number!");
                    return;
                }

                cashedInAmount = inputAmount;
                balance -= inputAmount;

                SaveBalance();

                playerScore = 0f;
                gameStarted = true;

                UpdateMultiplierText();
                UpdateBalanceText();
                UpdateCashedInText();

                if (cashInInput.transform.parent != null)
                    cashInInput.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Invalid input! Please enter a number.");
            }
        }
        else
        {
            Debug.LogWarning("CashInInput reference missing!");
        }
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetInt(currentUser + "_Balance", balance);
        PlayerPrefs.Save();
    }
}
