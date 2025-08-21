using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Press number keys (1–6) to pick a specific camera
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToCamera(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCamera(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToCamera(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToCamera(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchToCamera(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchToCamera(5);
    }

    void SwitchToCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == index);
    }
}