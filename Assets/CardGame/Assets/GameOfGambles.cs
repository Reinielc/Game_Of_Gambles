using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOfGamble : MonoBehaviour
{
    public TextMeshProUGUI player1CardText;
    public TextMeshProUGUI player2CardText;
    public TextMeshProUGUI player3CardText;
    public TextMeshProUGUI gameLogText;
    public TextMeshProUGUI player1BalanceText;
    public TextMeshProUGUI player1BetText;
    public Button player1BetButton;

    public TextMeshProUGUI player2BalanceText;
    public TextMeshProUGUI player2BetText;
    public Button player2BetButton;

    public TextMeshProUGUI player3BalanceText;
    public TextMeshProUGUI player3BetText;
    public Button player3BetButton;

    public Button playRoundButton;
    public TMP_Dropdown guessDropdown;
    public Button guessSubmitButton;
    public TextMeshProUGUI guesserBalanceText;

    public Button restartButton;
    public Button returnToHubButton;

    private int betAmount = 100;
    private int player1Balance = 1000, player2Balance = 1000, player3Balance = 1000;
    private int player1CurrentBet = 0, player2CurrentBet = 0, player3CurrentBet = 0;
    private int guesserBalance = 10000;
    private int playerGuess = -1;

    private string currentUser;

    void Start()
    {
        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
        int totalBalance = PlayerPrefs.GetInt(currentUser + "_Balance", 10000);

        // 分配三个AI玩家相等余额，Guesser 拥有所有余额（只是显示，实际使用同一个账户）
        int split = totalBalance / 3;
        player1Balance = split;
        player2Balance = split;
        player3Balance = split;
        guesserBalance = totalBalance;

        player1BetButton.onClick.AddListener(() => Bet(1));
        player2BetButton.onClick.AddListener(() => Bet(2));
        player3BetButton.onClick.AddListener(() => Bet(3));
        playRoundButton.onClick.AddListener(PlayRound);
        guessSubmitButton.onClick.AddListener(SubmitGuess);
        restartButton.onClick.AddListener(RestartGame);
        returnToHubButton.onClick.AddListener(ReturnToGameHub);

        UpdateUI();
        UpdateGuesserUI();
    }

    public void SubmitGuess()
    {
        playerGuess = guessDropdown.value;
        gameLogText.text = $"Guesser guessed Player {playerGuess + 1}!";
    }

    public void Bet(int playerId)
    {
        switch (playerId)
        {
            case 1:
                if (player1Balance >= betAmount)
                {
                    player1Balance -= betAmount;
                    player1CurrentBet += betAmount;
                }
                break;
            case 2:
                if (player2Balance >= betAmount)
                {
                    player2Balance -= betAmount;
                    player2CurrentBet += betAmount;
                }
                break;
            case 3:
                if (player3Balance >= betAmount)
                {
                    player3Balance -= betAmount;
                    player3CurrentBet += betAmount;
                }
                break;
        }
        UpdateUI();
    }

    public void PlayRound()
    {
        int activePlayers = 0;
        if (player1CurrentBet > 0) activePlayers++;
        if (player2CurrentBet > 0) activePlayers++;
        if (player3CurrentBet > 0) activePlayers++;

        if (activePlayers < 2)
        {
            gameLogText.text = "Need at least two players to bet!";
            return;
        }

        int p1Card = Random.Range(2, 15);
        int p2Card = Random.Range(2, 15);
        int p3Card = Random.Range(2, 15);

        int maxCard = Mathf.Max(p1Card, Mathf.Max(p2Card, p3Card));
        int totalPot = player1CurrentBet + player2CurrentBet + player3CurrentBet;

        int winners = 0;
        if (p1Card == maxCard) winners++;
        if (p2Card == maxCard) winners++;
        if (p3Card == maxCard) winners++;

        int winnings = (winners > 0) ? totalPot / winners : 0;

        int p1Winnings = (p1Card == maxCard) ? winnings : 0;
        int p2Winnings = (p2Card == maxCard) ? winnings : 0;
        int p3Winnings = (p3Card == maxCard) ? winnings : 0;

        if (p1Card == maxCard) player1Balance += winnings;
        if (p2Card == maxCard) player2Balance += winnings;
        if (p3Card == maxCard) player3Balance += winnings;

        string log = "Winner: ";
        if (p1Card == maxCard) log += "Player 1 ";
        if (p2Card == maxCard) log += "Player 2 ";
        if (p3Card == maxCard) log += "Player 3 ";

        if (playerGuess != -1)
        {
            int guessAmount = 0;
            bool guessCorrect = false;

            switch (playerGuess)
            {
                case 0:
                    guessAmount = p1Winnings > 0 ? p1Winnings : player1CurrentBet;
                    guessCorrect = p1Winnings > 0;
                    break;
                case 1:
                    guessAmount = p2Winnings > 0 ? p2Winnings : player2CurrentBet;
                    guessCorrect = p2Winnings > 0;
                    break;
                case 2:
                    guessAmount = p3Winnings > 0 ? p3Winnings : player3CurrentBet;
                    guessCorrect = p3Winnings > 0;
                    break;
            }

            if (guessCorrect)
            {
                guesserBalance += guessAmount;
                log += $"\nGuesser guessed correctly and earned {guessAmount}!";
            }
            else
            {
                guesserBalance -= guessAmount;
                log += $"\nGuesser guessed wrong and lost {guessAmount}.";
            }
        }

        string[] cardNames = { "", "", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        player1CardText.text = $"Card: {cardNames[p1Card]}";
        player2CardText.text = $"Card: {cardNames[p2Card]}";
        player3CardText.text = $"Card: {cardNames[p3Card]}";

        player1CurrentBet = 0;
        player2CurrentBet = 0;
        player3CurrentBet = 0;
        playerGuess = -1;

        UpdateUI();
        UpdateGuesserUI();

        log += $"\nP1 Balance: {player1Balance} | P2: {player2Balance} | P3: {player3Balance}";
        log += $"\nGuesser Balance: {guesserBalance}";
        gameLogText.text = log;

        SaveBalanceToPrefs(); // ⬅️ 更新保存
    }

    void SaveBalanceToPrefs()
    {
        PlayerPrefs.SetInt(currentUser + "_Balance", guesserBalance);
        PlayerPrefs.Save();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToGameHub()
    {
        SceneManager.LoadScene("GameHub");
    }

    void UpdateUI()
    {
        player1BalanceText.text = $"Player 1 Balance:\n{player1Balance}";
        player1BetText.text = $"Current Bet:\n{player1CurrentBet}";

        player2BalanceText.text = $"Player 2 Balance:\n{player2Balance}";
        player2BetText.text = $"Current Bet:\n{player2CurrentBet}";

        player3BalanceText.text = $"Player 3 Balance:\n{player3Balance}";
        player3BetText.text = $"Current Bet:\n{player3CurrentBet}";
    }

    void UpdateGuesserUI()
    {
        guesserBalanceText.text = $"Guesser Balance:\n{guesserBalance}";
    }
}

