using UnityEngine;
using TMPro;

public class SpinTheWheel : MonoBehaviour
{
    [Header("Wheel Settings")]
    public Rigidbody wheelRb;
    public float minSpinForce = 500f;
    public float maxSpinForce = 1500f;
    public float friction = 0.5f; // how quickly it slows down per frame

    [Header("UI")]
    public TMP_Text resultText;
    public string[] outcomes; // 20 outcomes: +100, -500, x2, etc.

    private bool isSpinning = false;

    public void Spin()
    {
        if (!isSpinning)
        {
            wheelRb.angularVelocity = Vector3.zero;

            float randomForce = Random.Range(minSpinForce, maxSpinForce);
            wheelRb.AddTorque(Vector3.up * randomForce, ForceMode.Impulse);

            isSpinning = true;
            resultText.text = "Spinning...";
        }
    }

    private void Update()
    {
        if (isSpinning)
        {
            // Apply friction manually
            wheelRb.angularVelocity *= friction;

            // Stop when very slow
            if (wheelRb.angularVelocity.magnitude < 0.1f)
            {
                isSpinning = false;

                // Find result
                float angle = wheelRb.transform.eulerAngles.z;
                int segmentCount = outcomes.Length;
                float segmentAngle = 360f / segmentCount;

                // Convert angle to index
                int index = Mathf.FloorToInt(angle / segmentAngle);
                index = segmentCount - 1 - index; // flip direction if needed
                if (index < 0) index += segmentCount;

                string prize = outcomes[index];
                resultText.text = "Result: " + prize;
            }
        }
    }
}