using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardSumGame : MonoBehaviour
{
    public TextMeshProUGUI[] playerCardTexts;
    public TextMeshProUGUI playerTotalText;
    public TextMeshProUGUI[] aiCardTexts;
    public TextMeshProUGUI aiTotalText;
    public TextMeshProUGUI resultText;

    public Button playButton;
    public Button betButton;
    public Button backHomeButton;

    public TextMeshProUGUI balanceText;

    private int playerBalance = 10000;
    private int betAmount = 100;
    private bool hasBet = false;
    private string currentUser;

    void Start()
    {
        playButton.onClick.AddListener(PlayRound);
        betButton.onClick.AddListener(PlaceBet);
        backHomeButton.onClick.AddListener(() => SceneManager.LoadScene("GameHub"));

        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");

        // 初始化余额
        string key = currentUser + "_Balance";
        if (PlayerPrefs.HasKey(key))
        {
            playerBalance = PlayerPrefs.GetInt(key);
        }
        else
        {
            playerBalance = 10000;
            PlayerPrefs.SetInt(key, playerBalance);
            PlayerPrefs.Save();
        }

        UpdateBalanceUI();
    }

    void PlaceBet()
    {
        if (playerBalance >= betAmount && !hasBet)
        {
            playerBalance -= betAmount;
            hasBet = true;
            UpdateBalance();
            resultText.text = "Bet placed. Click Play!";
        }
        else if (hasBet)
        {
            resultText.text = "You already placed a bet!";
        }
        else
        {
            resultText.text = "Not enough balance to bet!";
        }
    }

    void PlayRound()
    {
        if (!hasBet)
        {
            resultText.text = "Please place a bet first.";
            return;
        }

        int[] playerCards = new int[3];
        int[] aiCards = new int[3];
        int playerSum = 0;
        int aiSum = 0;

        for (int i = 0; i < 3; i++)
        {
            playerCards[i] = Random.Range(1, 14);
            aiCards[i] = Random.Range(1, 14);
            playerCardTexts[i].text = playerCards[i].ToString();
            aiCardTexts[i].text = aiCards[i].ToString();
            playerSum += playerCards[i];
            aiSum += aiCards[i];
        }

        playerTotalText.text = "Total: " + playerSum;
        aiTotalText.text = "Total: " + aiSum;

        if (playerSum > aiSum)
        {
            playerBalance += betAmount * 2;
            resultText.text = "🎉 You Win!";
        }
        else if (playerSum < aiSum)
        {
            resultText.text = "💻 AI Wins!";
        }
        else
        {
            playerBalance += betAmount; // Tie → refund
            resultText.text = "🤝 It's a Tie!";
        }

        hasBet = false;
        UpdateBalance();
    }

    void UpdateBalance()
    {
        PlayerPrefs.SetInt(currentUser + "_Balance", playerBalance);
        PlayerPrefs.Save();
        UpdateBalanceUI();
    }

    void UpdateBalanceUI()
    {
        balanceText.text = $"Balance: {playerBalance}";
    }
}
