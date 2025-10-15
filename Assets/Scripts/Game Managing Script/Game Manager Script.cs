using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static string AmountKey = "Amount";

    public int Amount = 5000;
    public int Bet_Amount = 0;

    public bool won = false;

    public void GameResult()
    {
        if (won) { Amount += Bet_Amount; }
        else { Amount -= Bet_Amount; }
    }

    public void SaveAmount()
    {
        PlayerPrefs.SetInt(AmountKey, Amount);
        PlayerPrefs.Save();
    }

    public int LoadAmount()
    {
        Amount = PlayerPrefs.GetInt(AmountKey, 5000);
        return Amount;
    }

    private void Update()
    {
        if (Amount == 0)
        {
            Amount = 5000;
            SaveAmount();
            SceneManager.LoadScene("Game End");
        }

        if (Amount >= 50000)
        {
            Amount = 5000;
            SaveAmount();
            SceneManager.LoadScene("Game End");
        }
    }
}