using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeRotator : MonoBehaviour
{
    public HUDFlicker hudflicker;

    public float rotationSpeed = 300f; // degrees per second

    private bool isRotating = false;
    private int direction = +1;
    private int movecounter = 0;
    private string movetracker = "None";

    private List<Transform> GetFaceCubes(Vector3 axis, int layer) // -1, 0, +1
    {
        List<Transform> faceCubes = new List<Transform>();

        foreach (Transform cube in transform)
        {
            Vector3 pos = cube.localPosition;

            if (axis == Vector3.right && Mathf.RoundToInt(pos.x) == layer)
                faceCubes.Add(cube);
            else if (axis == Vector3.up && Mathf.RoundToInt(pos.y) == layer)
                faceCubes.Add(cube);
            else if (axis == Vector3.forward && Mathf.RoundToInt(pos.z) == layer)
                faceCubes.Add(cube);
        }

        return faceCubes;
    }
    // Public moves (WCA style notation)
    public void RotateRight() { if (!isRotating) StartCoroutine(RotateFace(Vector3.forward, +1, direction)); movecounter++; movetracker = "Right"; CheckGamble(); }
    public void RotateLeft() { if (!isRotating) StartCoroutine(RotateFace(Vector3.forward, -1, -direction)); movecounter++; movetracker = "Left"; CheckGamble(); }
    public void RotateUp() { if (!isRotating) StartCoroutine(RotateFace(Vector3.up, +1, direction)); movecounter++; movetracker = "Up"; CheckGamble(); }
    public void RotateDown() { if (!isRotating) StartCoroutine(RotateFace(Vector3.up, -1, -direction)); movecounter++; movetracker = "Down"; CheckGamble(); }
    public void RotateFront() { if (!isRotating) StartCoroutine(RotateFace(Vector3.right, +1, direction)); movecounter++; movetracker = "Front"; CheckGamble(); }
    public void RotateBack() { if (!isRotating) StartCoroutine(RotateFace(Vector3.right, -1, -direction)); movecounter++; movetracker = "Back"; CheckGamble(); }

    // Middle slices
    public void RotateMiddle() { if (!isRotating) StartCoroutine(RotateFace(Vector3.forward, 0, -direction)); movecounter++; movetracker = "Middle"; CheckGamble(); }
    public void RotateEquator() { if (!isRotating) StartCoroutine(RotateFace(Vector3.up, 0, -direction)); movecounter++; movetracker = "Equator"; CheckGamble(); }
    public void RotateStanding() { if (!isRotating) StartCoroutine(RotateFace(Vector3.right, 0, -direction)); movecounter++; movetracker = "Standing"; CheckGamble(); }

    private void CheckGamble()
    {
        if (movecounter >= 17)
        {
            StartCoroutine(GambleAfterMove());
            movecounter = 0; // reset counter for next cycle
        }
    }

    private IEnumerator GambleAfterMove()
    {
        // wait until current rotation is finished
        while (isRotating)
            yield return null;

        CallGamble();
    }


    public void CallGamble()
    {
        movecounter = 0;

        if (movetracker == "Left") { hudflicker.FlickerLayer("Left"); }
        else if (movetracker == "Middle") { hudflicker.FlickerLayer("Middle"); }
        else if (movetracker == "Right") { hudflicker.FlickerLayer("Right"); }
        else if (movetracker == "Up") { hudflicker.FlickerLayer("Up"); }
        else if (movetracker == "Equator") { hudflicker.FlickerLayer("Equator"); }
        else if (movetracker == "Down") { hudflicker.FlickerLayer("Down"); }
        else if (movetracker == "Front") { hudflicker.FlickerLayer("Front"); }
        else if (movetracker == "Standing") { hudflicker.FlickerLayer("Standing"); }
        else if (movetracker == "Back") { hudflicker.FlickerLayer("Back"); }

    }

    private IEnumerator RotateFace(Vector3 axis, int layer, int direction)
    {
        isRotating = true;

        // 1. find cubes in this slice
        List<Transform> faceCubes = GetFaceCubes(axis, layer);

        // 2. make pivot
        GameObject pivot = new GameObject("Pivot");
        pivot.transform.parent = transform;
        pivot.transform.localPosition = Vector3.zero;

        // 3. parent cubes to pivot
        foreach (Transform cube in faceCubes)
            cube.SetParent(pivot.transform);

        // 4. rotate pivot smoothly
        float angle = 0f;
        while (angle < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            if (angle + step > 90f) step = 90f - angle;

            pivot.transform.Rotate(axis, step * direction, Space.Self);
            angle += step;
            yield return null;
        }

        // 5. unparent cubes
        foreach (Transform cube in faceCubes)
            cube.SetParent(transform);

        Destroy(pivot);
        isRotating = false;
    }

    //Invert Direction of Rotation
    public void InvertDirection() { direction *= -1; }
}