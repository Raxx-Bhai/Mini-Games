using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameManagerScript game;

    public void PlayButton()
    {
        game.Amount = 5000;
        game.SaveAmount();
        SceneManager.LoadScene("Rubik's Cube");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}