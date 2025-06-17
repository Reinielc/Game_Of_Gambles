using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

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
    public Button ReStart;
    public Text ScoreText;
    public Text AllScoreText;
    private int Score;
    public int AllScore;
    public GameObject StartPanel;
    public Button StartGameButton;

    public Button AutoButton;
    private bool IsAuto;
    private bool IsPlay;
    private AudioSource source;

    public AudioClip ButtonClip;
    public AudioClip EndClip;
    public AudioClip ingClip;

    private void Awake()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
        UpdateScore(30);
        UpdateAllScore(0);
        StartButton.onClick.AddListener(() =>
        {
            source.PlayOneShot(ButtonClip);
            Play();
        });
        ReStart.onClick.AddListener(() =>
        {
            source.PlayOneShot(ButtonClip);
            UpdateScore(30);
        });
        StartGameButton.onClick.AddListener(() =>
        {
            source.PlayOneShot(ButtonClip);
            StartPanel.SetActive(false);
        });
        AutoButton.onClick.AddListener(() =>
        {
            source.PlayOneShot(ButtonClip);
            IsAuto = !IsAuto;
            if (IsAuto)
            {
                AutoButton.GetComponentInChildren<Text>().text = "Cancal";
            }
            else
            {
                AutoButton.GetComponentInChildren<Text>().text = "Auto";
            }
            if (IsAuto && !IsPlay)
            {
                Play();
            }
            if (!IsAuto && !IsPlay)
            {
                StartButton.interactable = true;
                ReStart.interactable = true;
            }
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
        if (Score <= 0)
        {
            return;
        }
        IsPlay = true;
        UpdateScore(Score - 10);
        StartButton.interactable = false;
        ReStart.interactable = false;
        Item item1 = RandomGetItem();
        Item item2 = RandomGetItem();
        Item item3 = RandomGetItem();
        Line1.Create(item1, 10);
        Line2.Create(item2, 15);
        Line3.Create(item3, 20, () =>
        {
            source.PlayOneShot(EndClip);
            IsPlay = false;
            StartButton.interactable = true;
            ReStart.interactable = true;
            //显示分数
            if (item1.Name == item2.Name && item1.Name == item3.Name)
            {
                UpdateScore(Score + 100);
                UpdateAllScore(100);
            }
            else if (item1.Name != item2.Name && item1.Name != item3.Name && item2.Name != item3.Name)
            {
            }
            else
            {
                UpdateScore(Score + 30);
                UpdateAllScore(30);
            }
            if (IsAuto)
            {
                StartButton.interactable = false;
                ReStart.interactable = false;
                StartCoroutine(IEAuto());
            }
        });
    }

    private IEnumerator IEAuto()
    {
        yield return new WaitForSeconds(1);
        Play();
    }

    private void UpdateScore(int value)
    {
        Score = value;
        ScoreText.text = "Point:" + Score.ToString();
    }

    private void UpdateAllScore(int value)
    {
        AllScore += value;
        AllScoreText.text = "Total Point:" + AllScore.ToString();
    }
}

[Serializable]
public class Item
{
    public string Name;
    public Sprite Icon;
}