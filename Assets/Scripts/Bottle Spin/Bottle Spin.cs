using UnityEngine;
using TMPro;

public class BottleSpin : MonoBehaviour
{
    [Header("Bottle Settings")]
    public Rigidbody bottleRb;
    public float minForce = 50f;
    public float maxForce = 150f;
    public float CoefficientofFriction = 0.3f;

    [Header("UI")]
    public TMP_Text resultText;

    private bool isSpinning = false;

    public void SpinBottle()
    {
        if (!isSpinning)
        {
            // Reset spin
            bottleRb.angularVelocity = Vector3.zero;

            // Apply random spin force (Impulse)
            float randomForce = Random.Range(minForce, maxForce);
            bottleRb.AddTorque(Vector3.up * randomForce, ForceMode.Impulse);

            isSpinning = true;
        }
    }

    private void Update()
    {
        if (isSpinning)
        {
            if (bottleRb.angularVelocity.magnitude > 0.01f)
            {
                // Apply friction continuously
                Vector3 friction = -bottleRb.angularVelocity.normalized * CoefficientofFriction;
                bottleRb.AddTorque(friction, ForceMode.Acceleration);
            }
            else
            {
                // Spin stopped
                isSpinning = false;

                float finalY = bottleRb.transform.eulerAngles.y;
                string winner = (finalY % 360f < 180f) ? "Cyan (You)" : "Pink (Opponent)";
                resultText.text = "Winner: " + winner;
            }
        }
    }
}