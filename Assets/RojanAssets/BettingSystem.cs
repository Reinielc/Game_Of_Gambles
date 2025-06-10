using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BettingSystem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI balanceText;
    public TMP_InputField betInputField;
    public TextMeshProUGUI resultText;
    public TMP_Dropdown difficultyDropdown;
    public Button flipButton;

    [Header("Sound References")]
    public AudioSource winSound;
    public AudioSource loseSound;

    [Header("Coin Reference")]
    public CoinFlipGame coinFlipper;

    private int playerBalance;
    private string currentUser;

    void Start()
    {
        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
        LoadBalance();
        UpdateBalanceText();
        flipButton.onClick.AddListener(FlipAndBet);
    }

    public void FlipAndBet()
    {
        if (coinFlipper != null)
        {
            coinFlipper.FlipCoinAndBet();
        }
    }

    public int GetBalance()
    {
        return playerBalance;
    }

    public void SetBalance(int newBalance)
    {
        playerBalance = newBalance;
        SaveBalance();
        UpdateBalanceText();
    }

    void UpdateBalanceText()
    {
        balanceText.text = $"Balance: ${playerBalance}";
    }

    void LoadBalance()
    {
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
    }

    void SaveBalance()
    {
        PlayerPrefs.SetInt(currentUser + "_Balance", playerBalance);
        PlayerPrefs.Save();
    }
}
