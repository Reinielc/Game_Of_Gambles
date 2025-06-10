using UnityEngine;
using TMPro;

public class GameHubController : MonoBehaviour
{
    public TextMeshProUGUI balanceText;

    void Start()
    {
        // 获取当前用户
        string currentUser = PlayerPrefs.GetString("CurrentUser", "Guest");

        // 获取该用户的余额（默认值为10000）
        int balance = PlayerPrefs.GetInt(currentUser + "_Balance", 10000);

        // 显示余额
        balanceText.text = $"💰 Balance: {balance}";
    }
}



