using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Line Line1;
    public Line Line2;
    public Line Line3;
    public Button StartButton;
    public Text ScoreText;

    [Header("Control Buttons")]
    public Button RestartButton;
    public Button ReturnToHubButton;

    [Header("Item List")]
    public List<Item> ItemList = new List<Item>(); // 确保这个列表已正确填充

    private int Score;

    private void Awake()
    {
        Instance = this;

        // 加载余额
        var data = SavePlayer.Load();
        Score = data != null ? data.balance : 1000;

        UpdateScoreUI();

        StartButton.onClick.AddListener(() =>
        {
            if (Score < 10) return;

            UpdateScore(Score - 10);
            Play();
            StartButton.interactable = false;
        });

        // 绑定按钮事件
        if (RestartButton != null)
            RestartButton.onClick.AddListener(RestartGame);
        if (ReturnToHubButton != null)
            ReturnToHubButton.onClick.AddListener(ReturnToGameHub);
    }

    public Item RandomGetItem()
    {
        int index = UnityEngine.Random.Range(0, ItemList.Count);
        return ItemList[index];
    }

    public void Play()
    {
        Item item1 = RandomGetItem();
        Item item2 = RandomGetItem();
        Item item3 = RandomGetItem();

        Line1.Create(item1, 10);
        Line2.Create(item2, 15);
        Line3.Create(item3, 20, () =>
        {
            StartButton.interactable = true;

            if (item1.Name == item2.Name && item1.Name == item3.Name)
            {
                UpdateScore(Score + 100);
            }
            else if (item1.Name != item2.Name && item1.Name != item3.Name && item2.Name != item3.Name)
            {
                // no prize
            }
            else
            {
                UpdateScore(Score + 30);
            }
        });
    }

    public void UpdateScore(int newScore)
    {
        Score = newScore;
        SavePlayer.Save("SlotPlayer", Score);
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        ScoreText.text = "Point: " + Score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToGameHub()
    {
        SceneManager.LoadScene("GameHub");
    }
}
