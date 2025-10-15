using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottleSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    public float minSpinSpeed = 400f;
    public float maxSpinSpeed = 700f;
    public float deceleration = 2f; // Higher = faster slowdown

    [Header("Panel")]
    public GameObject betPanel;
    public GameObject SpinPanel;
    public TMPro.TextMeshProUGUI displayAmount;

    public GameManagerScript game;

    public AudioClip Baudio;

    private float currentSpinSpeed = 0f;
    private bool spinning = false;
    private int betAmount;

    [Header("UI")]
    public TMP_Text resultText;
    public TextMeshProUGUI Message;

    private void Start()
    {
        game.Amount = game.LoadAmount();
        displayAmount.text = $"${game.Amount}";
        ShowBetPannel();
    }

    void ShowBetPannel()
    {
        if (game.Amount <= 25) { game.Bet_Amount = game.Amount; SpinPanel.SetActive(true); }
        else { betPanel.SetActive(true); }
    }

    public void OnBetChosen(string betString)
    {
        betAmount = int.Parse(betString.Replace("$", ""));

        if (betAmount > game.Amount) { Message.text = "Please enter a valid Amount!"; return; }
        else { game.Bet_Amount = betAmount; betPanel.SetActive(false); SpinPanel.SetActive(true); }
    }

    void Update()
    {
        if (spinning)
        {
            // Rotate bottle on Y-axis
            transform.Rotate(0, currentSpinSpeed * Time.deltaTime, 0, Space.Self);

            // Smooth deceleration using ease-out
            currentSpinSpeed = Mathf.Lerp(currentSpinSpeed, 0, Time.deltaTime * deceleration);

            // Stop condition
            if (currentSpinSpeed < 3f)
            {
                spinning = false;
                currentSpinSpeed = 0f;
                SnapRotation();
                DecideWinner();
            }
        }
    }

    public void StartSpin()
    {
        SpinPanel.SetActive(false);
        currentSpinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
        AudioSource.PlayClipAtPoint(Baudio, Camera.main.transform.position);
        spinning = true;
        resultText.text = "";
    }

    void SnapRotation()
    {
        // Optional: Snap Y rotation to nearest degree to avoid tiny fractions
        float y = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }

    void DecideWinner()
    {
        float yRotation = transform.eulerAngles.y % 360f;

        if (yRotation >= 0 && yRotation < 180) { resultText.text = "You Won"; game.won = true; }
        else { resultText.text = "Opponent Won!"; game.won = false; }

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