using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CoinTossManager : MonoBehaviour
{
    public GameObject betPanel;
    public GameObject decidePanel;
    public GameObject invertPanel;
    public GameObject box;
    public GameObject boxhead;
    public Transform coin;

    public AudioClip caudio;

    public GameManagerScript game;

    public TMPro.TextMeshProUGUI resulttext;
    public TMPro.TextMeshProUGUI Message;
    public TMPro.TextMeshProUGUI displayAmount;

    private int betAmount;
    private CoinSide playerChoice;
    private CoinSide coinResult;

    private enum CoinSide { Heads, Tails }

    void Start()
    {
        game.Amount = game.LoadAmount();
        displayAmount.text = $"${game.Amount}";
        ShowBetPanel();
    }

    void ShowBetPanel()
    {
        if (game.Amount <= 25) { game.Bet_Amount = game.Amount; decidePanel.SetActive(true); }
        else { betPanel.SetActive(true); }
    }

    public void OnBetChosen(string betString)
    {
        betAmount = int.Parse(betString.Replace("$", ""));

        if (betAmount > game.Amount) { Message.text = "Please enter a valid Amount!"; return; }
        else { game.Bet_Amount = betAmount; betPanel.SetActive(false); decidePanel.SetActive(true); }
    }

    public void OnChoiceMade(string choice)
    {
        playerChoice = (choice == "Heads") ? CoinSide.Heads : CoinSide.Tails;
        decidePanel.SetActive(false);
        StartCoroutine(CoinTossSequence());
    }

    public void OnInvertChoice(bool invert)
    {
        invertPanel.SetActive(false);
        if (invert) PlayInvertAnimation();
        else PlayDirectOpenAnimation();
    }

    private System.Collections.IEnumerator CoinTossSequence()
    {
        // Coin rises
        Vector3 startPos = coin.position;
        Vector3 upPos = new Vector3(startPos.x, 0.505f, startPos.z);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            coin.position = Vector3.Lerp(startPos, upPos, t);
            yield return null;
        }

        // Move forward along Z with spin
        Vector3 endPos = new Vector3(startPos.x, upPos.y, 20f);
        t = 0;
        AudioSource.PlayClipAtPoint(caudio, coin.transform.position);
        while (t < 1f)
        {
            t += Time.deltaTime;
            coin.position = Vector3.Lerp(upPos, endPos, t);
            coin.Rotate(Vector3.right * 720f * Time.deltaTime);
            yield return null;
        }

        // Decide coin result randomly
        coinResult = (Random.value > 0.5f) ? CoinSide.Heads : CoinSide.Tails;

        // Snap into box
        coin.SetParent(box.transform);
        coin.localPosition = Vector3.zero;

        // NEW: Set coin orientation based on result
        if (coinResult == CoinSide.Heads)
        {
            // Heads up = default prefab rotation
            coin.localRotation = Quaternion.identity;
        }
        else
        {
            // Tails up = flip on X axis (try this first)
            coin.localRotation = Quaternion.Euler(180f, 0f, 0f);
        }

        // Box return
        yield return StartCoroutine(MoveBoxBack());

        // Show invert panel
        invertPanel.SetActive(true);
    }

    private System.Collections.IEnumerator MoveBoxBack()
    {
        Vector3 startPos = box.transform.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y, 0);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            box.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }
    void PlayInvertAnimation()
    {
        boxhead.transform.position += new Vector3(0, 2, 0);
        CheckResult(playerChoice != coinResult); // invert flips the result
    }

    void PlayDirectOpenAnimation()
    {
        // TODO: animate box opening
        boxhead.transform.position += new Vector3(0, 2, 0);
        CheckResult(playerChoice == coinResult);
    }

    void CheckResult(bool win)
    {
        if (win)
        {
            game.won = true;
            resulttext.text = "You Won!";
        }
        else
        {
            game.won = false;
            resulttext.text = "You Lost!";
        }

        StartCoroutine(LoadPreviousScene());
    }

    IEnumerator LoadPreviousScene()
    {
        yield return new WaitForSeconds(0.5f);
        game.GameResult();
        game.SaveAmount();

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Rubik's Cube");
    }
}