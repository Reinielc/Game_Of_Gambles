using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public float playerScore;
    public Text scoreText;
    public GameObject gameOverScreen;

    private float timer = 0f;
    private float scoreInterval = 10f;
    private bool gameIsOver = false;

    void Update()
    {
        if (!gameIsOver)
        {
            timer += Time.deltaTime;
            if (timer >= scoreInterval)
            {
                addScore();
                timer = 0f;
            }
        }
    }

    public void addScore()
    {
        playerScore += 1;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        Debug.Log("Game Over triggered!");
        gameIsOver = true;
        gameOverScreen.SetActive(true);
    }
}
