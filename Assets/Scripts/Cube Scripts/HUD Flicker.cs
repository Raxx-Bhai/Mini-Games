using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDFlicker : MonoBehaviour
{
    [Header("HUD Cube Layers")]
    public List<Transform> LeftLayer;
    public List<Transform> MiddleLayer;
    public List<Transform> RightLayer;
    public List<Transform> UpLayer;
    public List<Transform> EquatorLayer;
    public List<Transform> DownLayer;
    public List<Transform> FrontLayer;
    public List<Transform> StandingLayer;
    public List<Transform> BackLayer;

    [Header("Flicker Settings")]
    public float flickerDuration = 0.8f;
    public float sceneDelay = 0.2f;
    public Color flickerColor = Color.red;

    private Dictionary<string, List<Transform>> layerMap;

    void Awake()
    {
        // Map layer names to lists for easy lookup
        layerMap = new Dictionary<string, List<Transform>>
        {
            {"Left", LeftLayer},
            {"Middle", MiddleLayer},
            {"Right", RightLayer},
            {"Up", UpLayer},
            {"Equator", EquatorLayer},
            {"Down", DownLayer},
            {"Front", FrontLayer},
            {"Standing", StandingLayer},
            {"Back", BackLayer}
        };
    }

    public void FlickerLayer(string layerName)
    {
        if (!layerMap.ContainsKey(layerName))
        {
            Debug.LogWarning($"HUD Flicker: No layer named {layerName}");
            return;
        }

        List<Transform> targetLayer = layerMap[layerName];
        StartCoroutine(FlickerRoutine(targetLayer));
    }

    private IEnumerator FlickerRoutine(List<Transform> cubes)
    {
        List<Renderer> renderers = new List<Renderer>();
        Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

        // Save renderers and original colors
        foreach (Transform t in cubes)
        {
            Renderer r = t.GetComponent<Renderer>();
            if (r != null)
            {
                renderers.Add(r);
                originalColors[r] = r.material.color;
            }
        }

        // Flicker for duration
        float timer = 0f;
        while (timer < flickerDuration)
        {
            foreach (Renderer r in renderers)
                r.material.color = flickerColor;

            yield return new WaitForSeconds(0.1f);

            foreach (Renderer r in renderers)
                r.material.color = originalColors[r];

            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        // Restore colors
        foreach (Renderer r in renderers)
            r.material.color = originalColors[r];

        // Delay before scene change
        yield return new WaitForSeconds(sceneDelay);

        // TODO: Load your scene here
        // SceneManager.LoadScene("NextScene");
    }
}
