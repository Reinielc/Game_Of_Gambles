using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// 管理器
/// </summary>
public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public Line Line1;
    public Line Line2;
    public Line Line3;
    public Button StartButton;
    public Text ScoreText;
    private int Score;
    private void Awake()
    {
        Instance = this;
        UpdateScore(30);
        StartButton.onClick.AddListener(() =>
        {
            if (Score <= 0)
            {
                return;
            }
            UpdateScore(Score - 10);
            Play();
            StartButton.interactable = false;
        });
    }
    public List<Item> ItemList = new List<Item>();
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
            //显示分数
            if (item1.Name == item2.Name && item1.Name == item3.Name)
            {
                UpdateScore(Score + 100);
            }
            else if (item1.Name != item2.Name && item1.Name != item3.Name && item2.Name != item3.Name)
            {

            }
            else
            {
                UpdateScore(Score + 30);
            }
        });
    }

    private void UpdateScore(int value)
    {
        Score = value;
        ScoreText.text = "Point:" + Score.ToString();
    }
}

[Serializable]
public class Item
{
    public string Name;
    public Sprite Icon;
}