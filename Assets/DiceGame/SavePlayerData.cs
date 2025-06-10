using UnityEngine;

[System.Serializable]
public class SavePlayerData
{
    public string playerName;
    public int balance;
}

public static class SavePlayer
{
    public static void Save(string name, int balance)
    {
        SavePlayerData data = new SavePlayerData
        {
            playerName = name,
            balance = balance
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SavedPlayer", json);
        PlayerPrefs.Save();
    }

    public static SavePlayerData Load()
    {
        if (PlayerPrefs.HasKey("SavedPlayer"))
        {
            string json = PlayerPrefs.GetString("SavedPlayer");
            return JsonUtility.FromJson<SavePlayerData>(json);
        }

        return null;
    }
}

