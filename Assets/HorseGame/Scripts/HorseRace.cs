using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class HorseRaceGamble : MonoBehaviour
{
    public TextMeshProUGUI horse1PlaceText;
    public TextMeshProUGUI horse2PlaceText;
    public TextMeshProUGUI horse3PlaceText;
    public TextMeshProUGUI gameLogText;

    public TextMeshProUGUI horse1BalanceText;
    public TextMeshProUGUI horse1BetText;
    public Button horse1BetButton;
    public TextMeshProUGUI horse1ResultText;

    public TextMeshProUGUI horse2BalanceText;
    public TextMeshProUGUI horse2BetText;
    public Button horse2BetButton;
    public TextMeshProUGUI horse2ResultText;

    public TextMeshProUGUI horse3BalanceText;
    public TextMeshProUGUI horse3BetText;
    public Button horse3BetButton;
    public TextMeshProUGUI horse3ResultText;

    public Button playRoundButton;
    public TMP_Dropdown guessDropdown;
    public Button guessSubmitButton;
    public TextMeshProUGUI guesserBalanceText;

    public Button restartButton;
    public Button returnToHubButton;

    private int betAmount = 100;
    private int horse1Balance = 1000, horse2Balance = 1000, horse3Balance = 1000;
    private int horse1CurrentBet = 0, horse2CurrentBet = 0, horse3CurrentBet = 0;
    private int guesserBalance = 10000;
    private int horseGuess = -1;
    private string currentUser;

    void Start()
    {
        // 获取当前用户并读取余额
        currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");
        guesserBalance = PlayerPrefs.GetInt(currentUser + "_Balance", 10000);
        PlayerPrefs.SetInt(currentUser + "_Balance", guesserBalance); // 确保写入一次
        PlayerPrefs.Save();

        int split = guesserBalance / 3;
        horse1Balance = split;
        horse2Balance = split;
        horse3Balance = split;

        horse1BetButton.onClick.AddListener(() => Bet(1));
        horse2BetButton.onClick.AddListener(() => Bet(2));
        horse3BetButton.onClick.AddListener(() => Bet(3));
        playRoundButton.onClick.AddListener(PlayRound);
        guessSubmitButton.onClick.AddListener(SubmitGuess);

        restartButton.onClick.AddListener(RestartGame);
        returnToHubButton.onClick.AddListener(ReturnToGameHub);

        UpdateGuesserUI();
        UpdateUI();
    }

    public void SubmitGuess()
    {
        horseGuess = guessDropdown.value;
        gameLogText.text = $"Guesser guessed Horse {horseGuess + 1}!";
    }

    public void Bet(int horseId)
    {
        switch (horseId)
        {
            case 1:
                if (horse1Balance >= betAmount)
                {
                    horse1Balance -= betAmount;
                    horse1CurrentBet += betAmount;
                }
                break;
            case 2:
                if (horse2Balance >= betAmount)
                {
                    horse2Balance -= betAmount;
                    horse2CurrentBet += betAmount;
                }
                break;
            case 3:
                if (horse3Balance >= betAmount)
                {
                    horse3Balance -= betAmount;
                    horse3CurrentBet += betAmount;
                }
                break;
        }
        UpdateUI();
    }

    public void PlayRound()
    {
        int activeHorses = 0;
        if (horse1CurrentBet > 0) activeHorses++;
        if (horse2CurrentBet > 0) activeHorses++;
        if (horse3CurrentBet > 0) activeHorses++;

        if (activeHorses < 2)
        {
            gameLogText.text = "Need at least two horses to be bet on!";
            return;
        }

        int[] places = { 1, 2, 3 };
        System.Random rng = new System.Random();
        places = places.OrderBy(x => rng.Next()).ToArray();

        int h1Place = places[0];
        int h2Place = places[1];
        int h3Place = places[2];

        int totalPot = horse1CurrentBet + horse2CurrentBet + horse3CurrentBet;
        int minPlace = 1;

        int winners = 0;
        if (h1Place == minPlace) winners++;
        if (h2Place == minPlace) winners++;
        if (h3Place == minPlace) winners++;

        int winnings = (winners > 0) ? totalPot / winners : 0;

        int h1Winnings = (h1Place == minPlace) ? winnings : 0;
        int h2Winnings = (h2Place == minPlace) ? winnings : 0;
        int h3Winnings = (h3Place == minPlace) ? winnings : 0;

        int bet1 = horse1CurrentBet;
        int bet2 = horse2CurrentBet;
        int bet3 = horse3CurrentBet;

        if (h1Place == minPlace) horse1Balance += winnings;
        if (h2Place == minPlace) horse2Balance += winnings;
        if (h3Place == minPlace) horse3Balance += winnings;

        string log = "Winner: ";
        if (h1Place == minPlace) log += "Horse 1 ";
        if (h2Place == minPlace) log += "Horse 2 ";
        if (h3Place == minPlace) log += "Horse 3 ";

        if (horseGuess != -1)
        {
            int guessAmount = 0;
            bool guessCorrect = (horseGuess == 0 && h1Place == 1) ||
                                (horseGuess == 1 && h2Place == 1) ||
                                (horseGuess == 2 && h3Place == 1);

            switch (horseGuess)
            {
                case 0: guessAmount = h1Winnings > 0 ? h1Winnings : bet1; break;
                case 1: guessAmount = h2Winnings > 0 ? h2Winnings : bet2; break;
                case 2: guessAmount = h3Winnings > 0 ? h3Winnings : bet3; break;
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

        horse1PlaceText.text = $"Place: {h1Place}";
        horse2PlaceText.text = $"Place: {h2Place}";
        horse3PlaceText.text = $"Place: {h3Place}";

        horse1ResultText.text = $"Horse 1: {h1Place}";
        horse2ResultText.text = $"Horse 2: {h2Place}";
        horse3ResultText.text = $"Horse 3: {h3Place}";

        horse1CurrentBet = 0;
        horse2CurrentBet = 0;
        horse3CurrentBet = 0;
        horseGuess = -1;

        UpdateUI();
        UpdateGuesserUI();
        SaveBalanceBackToPrefs();

        log += $"\nH1 Balance: {horse1Balance} | H2: {horse2Balance} | H3: {horse3Balance}";
        log += $"\nGuesser Balance: {guesserBalance}";
        gameLogText.text = log;
    }

    void SaveBalanceBackToPrefs()
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

    void UpdateGuesserUI()
    {
        guesserBalanceText.text = $"Guesser Balance:\n{guesserBalance}";
    }

    void UpdateUI()
    {
        horse1BalanceText.text = $"Horse 1 Balance:\n{horse1Balance}";
        horse1BetText.text = $"Current Bet:\n{horse1CurrentBet}";

        horse2BalanceText.text = $"Horse 2 Balance:\n{horse2Balance}";
        horse2BetText.text = $"Current Bet:\n{horse2CurrentBet}";

        horse3BalanceText.text = $"Horse 3 Balance:\n{horse3Balance}";
        horse3BetText.text = $"Current Bet:\n{horse3CurrentBet}";
    }
}
