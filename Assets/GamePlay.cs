using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Bird Game"); 
         
    }
    public void PlayCardGame()
    {
        SceneManager.LoadScene("CardGame");
    }
    
    public void LoadGameHub()
    {
        SceneManager.LoadScene("GameHub"); 
    }
    public void PlayHorseGame()
    {
        SceneManager.LoadScene("HorseGame");
    }
    public void RojanGame()
    {
        SceneManager.LoadScene("RojanGame");
    }
    public void PlaySlotMachineGame()
    {
        SceneManager.LoadScene("SlotMachineGame");
    }
    public void PlayCardSumGame()
    {
        SceneManager.LoadScene("SumCardScene"); 
         
    }
    public void PlayDiceGame()
    {
        SceneManager.LoadScene("DiceGame");
     
    }


}
