using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Bird Game"); 
        
    }
    
    public void LoadGameHub()
    {
        SceneManager.LoadScene("GameHub"); 
    }

}
