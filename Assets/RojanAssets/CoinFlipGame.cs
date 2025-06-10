using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CoinFlipGame : MonoBehaviour
{
    [Header("Coin Flip UI References")]
    public Image coinImage;
    public Sprite headsSprite;
    public Sprite tailsSprite;

    public TextMeshProUGUI resultText;
    public TMP_InputField betInputField;
    public TMP_Dropdown difficultyDropdown;
    public TextMeshProUGUI balanceText;

    [Header("Sound References")]
    public AudioSource winSound;
    public AudioSource loseSound;

    [Header("Control Buttons")]
    public Button restartButton;
    public Button returnToHubButton;

    void Start()
    {
        // 初始化余额
        BettingSystem system = FindObjectOfType<BettingSystem>();
        if (system != null)
        {
            balanceText.text = $"Balance: ${system.GetBalance()}";
        }

        // 按钮绑定
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (returnToHubButton != null)
            returnToHubButton.onClick.AddListener(ReturnToGameHub);
    }

    public void FlipCoinAndBet()
    {
        int betAmount;
        if (!int.TryParse(betInputField.text, out betAmount) || betAmount <= 0)
        {
            resultText.text = "Enter a valid bet amount!";
            return;
        }

        BettingSystem system = FindObjectOfType<BettingSystem>();
        if (system == null)
        {
            resultText.text = "System error!";
            return;
        }

        int currentBalance = system.GetBalance();

        if (betAmount > currentBalance)
        {
            resultText.text = "Insufficient balance!";
            return;
        }

        // Coin Flip
        bool isHeads = Random.value > 0.5f;
        coinImage.sprite = isHeads ? headsSprite : tailsSprite;

        // Win or Lose
        bool isWin = Random.value < GetWinChance();
        int newBalance = currentBalance;

        if (isWin)
        {
            newBalance += betAmount;
            resultText.text = $"You Win! +{betAmount}";
            if (winSound != null) winSound.Play();
        }
        else
        {
            newBalance -= betAmount;
            resultText.text = $"You Lose! -{betAmount}";
            if (loseSound != null) loseSound.Play();
        }

        // 更新余额
        system.SetBalance(newBalance);
        balanceText.text = $"Balance: ${newBalance}";
        betInputField.text = "";
    }

    float GetWinChance()
    {
        switch (difficultyDropdown.value)
        {
            case 0: return 0.8f; // Easy
            case 1: return 0.5f; // Medium
            case 2: return 0.25f; // Hard
            default: return 0.5f;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToGameHub()
    {
        SceneManager.LoadScene("GameHub"); // 请确保你的主菜单场景名是 GameHub
    }
}
