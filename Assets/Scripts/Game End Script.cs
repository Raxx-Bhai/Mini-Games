using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndScript : MonoBehaviour
{
    public GameManagerScript game;
    public TMPro.TextMeshProUGUI Message;

    public void Start()
    {
        game.Amount = game.LoadAmount();

        if (game.Amount <= 1)
        {
            Message.text = "You Lost!";
        }

        else if (game.Amount >= 50000)
        {
            Message.text = "You Won!";
        }
    }

    public void PressMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}