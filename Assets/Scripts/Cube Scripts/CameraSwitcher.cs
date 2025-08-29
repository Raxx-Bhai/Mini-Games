using UnityEngine;
using System.Collections;

public class CameraPivotRotator : MonoBehaviour
{
    [Header("Camera Pivot")]
    public Transform pivot;             // The pivot object (camera is child)

    [Header("Predefined Rotations")]
    public Transform[] cameraAngles;    // Empty objects placed where you want pivot to rotate

    [Header("Settings")]
    public float rotationspeed = 180f;   // Rotation speed in degrees per second

    private Coroutine rotateRoutine;

    // ⬇️ This will now only be called from UI Buttons
    public void RotateToView(int index)
    {
        if (index < 0 || index >= cameraAngles.Length) return;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateSmoothly(cameraAngles[index]));
    }

    private IEnumerator RotateSmoothly(Transform targetAngle)
    {
        Quaternion endRot = targetAngle.rotation;

        while (Quaternion.Angle(pivot.rotation, endRot) > 0.1f)
        {
            pivot.rotation = Quaternion.RotateTowards(
                pivot.rotation,
                endRot,
                rotationspeed * Time.deltaTime
            );

            yield return null;
        }

        pivot.rotation = endRot; // Snap exactly to final rotation
    }
}
