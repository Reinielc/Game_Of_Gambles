using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DiceGameManager : MonoBehaviour
{
    public TextMeshProUGUI diceResultText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI balanceText;

    public Button highButton;
    public Button lowButton;
    public Button rollButton;
    public Button aiPlayButton;
    public Button backHomeButton; // ✅ 新增返回主页按钮

    public Image diceImage;
    public Sprite[] diceFaces;

    private int diceNumber;
    private int balance;
    private string playerGuess = "";
    private bool isRolling = false;
    private string currentUser;

    void Start()
    {
        // 获取当前用户名和余额
        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
        balance = PlayerPrefs.GetInt(currentUser + "_Balance", 10000);
        PlayerPrefs.SetInt(currentUser + "_Balance", balance);
        PlayerPrefs.Save();

        diceResultText.text = "Roll the dice!";
        messageText.text = "Make your guess!";
        UpdateBalanceText();

        rollButton.interactable = false;

        highButton.onClick.AddListener(() => OnGuess("High"));
        lowButton.onClick.AddListener(() => OnGuess("Low"));
        rollButton.onClick.AddListener(RollDice);
        aiPlayButton.onClick.AddListener(AIPlay);

        if (backHomeButton != null)
            backHomeButton.onClick.AddListener(() => SceneManager.LoadScene("GameHub")); // ✅ 添加跳转事件
    }

    void OnGuess(string guess)
    {
        playerGuess = guess;
        messageText.text = $"You guessed {guess}. Now roll the dice!";
        rollButton.interactable = true;

        highButton.interactable = guess != "High";
        lowButton.interactable = guess != "Low";
    }

    public void RollDice()
    {
        if (isRolling || balance <= 0) return;

        diceNumber = Random.Range(1, 7);
        diceResultText.text = "Rolling...";
        rollButton.interactable = false;

        StartCoroutine(RollDiceAnimation(diceNumber));
        StartCoroutine(CheckResultAfterRoll());
    }

    private IEnumerator RollDiceAnimation(int finalNumber)
    {
        isRolling = true;
        float rollTime = 1.5f;
        float elapsed = 0f;
        float interval = 0.1f;

        while (elapsed < rollTime)
        {
            int randomFace = Random.Range(1, 7);
            diceImage.sprite = diceFaces[randomFace - 1];
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        diceImage.sprite = diceFaces[finalNumber - 1];
        isRolling = false;
    }

    private IEnumerator CheckResultAfterRoll()
    {
        while (isRolling)
        {
            yield return null;
        }

        bool isHigh = diceNumber >= 4;
        bool isLow = diceNumber <= 3;

        if ((playerGuess == "High" && isHigh) || (playerGuess == "Low" && isLow))
        {
            balance += 10;
            messageText.text = "You WIN! +$10. Now choose High or Low";
        }
        else
        {
            balance -= 10;
            messageText.text = "You LOSE! -$10. Now choose High or Low";
        }

        SaveBalance();
        UpdateBalanceText();

        if (balance <= 0)
        {
            messageText.text = "Game Over! You're out of money.";
            highButton.interactable = false;
            lowButton.interactable = false;
            rollButton.interactable = false;
            aiPlayButton.interactable = false;
        }
        else
        {
            highButton.interactable = true;
            lowButton.interactable = true;
        }
    }

    void AIPlay()
    {
        if (isRolling || balance <= 0) return;

        playerGuess = (Random.value < 0.5f) ? "High" : "Low";
        messageText.text = $"AI guessed {playerGuess}. Rolling...";

        highButton.interactable = playerGuess != "High";
        lowButton.interactable = playerGuess != "Low";

        rollButton.interactable = false;
        RollDice();
    }

    void SaveBalance()
    {
        PlayerPrefs.SetInt(currentUser + "_Balance", balance);
        PlayerPrefs.Save();
    }

    void UpdateBalanceText()
    {
        balanceText.text = $"Balance: ${balance}";
    }
}
