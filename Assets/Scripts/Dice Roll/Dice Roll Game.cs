using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DiceRollGame : MonoBehaviour
{
    [Header("Camera System")]
    public Transform pivot;                  // Camera pivot
    public Quaternion[] faceRotations;       // The 6 rotations for dice faces
    public Quaternion defaultRotation;       // Default camera rotation
    public float rotationSpeed = 180f;
    public float waitingtime = 0.75f;

    [Header("UI")]
    public GameObject betPanel;
    public GameObject rollpanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI displayAmount;

    public GameManagerScript game;

    private int playerRoll;
    private int aiRoll;
    private int betAmount;

    void Start()
    {
        game.Amount = game.LoadAmount();
        displayAmount.text = $"${game.Amount}";
        ShowBetPanel();
    }

    void ShowBetPanel()
    {
        if (game.Amount <= 25) { game.Bet_Amount = game.Amount; rollpanel.SetActive(true); }
        else { betPanel.SetActive(true); }
    }

    public void OnBetChosen(string betString)
    {
        betAmount = int.Parse(betString.Replace("$", ""));

        if (betAmount > game.Amount) { resultText.text = "Please enter a valid Amount"; }
        else
        {
            pivot.rotation = defaultRotation;
            game.Bet_Amount = betAmount;
            betPanel.SetActive(false);
            rollpanel.SetActive(true);
        }
    }

    public void OnRollButton()
    {
        rollpanel.SetActive(false);
        StartCoroutine(PlayerRollRoutine());
    }

    private IEnumerator PlayerRollRoutine()
    {
        int chosen = Random.Range(0, 6);
        playerRoll = chosen + 1;

        yield return StartCoroutine(RotateSmoothly(faceRotations[chosen]));
        resultText.text = "You rolled: " + playerRoll;

        yield return new WaitForSeconds(waitingtime);
        yield return StartCoroutine(RotateSmoothly(defaultRotation));

        yield return new WaitForSeconds(waitingtime);
        StartCoroutine(AIRollRoutine());
    }

    private IEnumerator AIRollRoutine()
    {
        int chosen = Random.Range(0, 6);
        aiRoll = chosen + 1;

        yield return StartCoroutine(RotateSmoothly(faceRotations[chosen]));
        resultText.text = "Opponent rolled: " + aiRoll;

        yield return new WaitForSeconds(waitingtime);
        yield return StartCoroutine(RotateSmoothly(defaultRotation));

        yield return new WaitForSeconds(waitingtime);
        CompareResults();
    }

    private void CompareResults()
    {
        if (playerRoll > aiRoll) { resultText.text = $"You Win! ({playerRoll} vs {aiRoll})"; game.won = true; StartCoroutine(LoadPreviousScene()); }
        else if (aiRoll > playerRoll) { resultText.text = $"Opponent Wins! ({aiRoll} vs {playerRoll})"; game.won = false; StartCoroutine(LoadPreviousScene()); }
        else { StartCoroutine(RestartRoll()); }
    }

    private IEnumerator RestartRoll()
    {
        resultText.text = "Tie! Re-rolling...";
        yield return new WaitForSeconds(waitingtime);
        OnRollButton();
    }

    private IEnumerator LoadPreviousScene()
    {
        yield return new WaitForSeconds(0.5f);
        game.GameResult();
        game.SaveAmount();

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Rubik's Cube");
    }

    private IEnumerator RotateSmoothly(Quaternion endRot)
    {
        Quaternion startRot = pivot.rotation;
        float duration = Quaternion.Angle(startRot, endRot) / rotationSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0, 1, t); // easing
            pivot.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        pivot.rotation = endRot;
    }
}