using System.Collections;
using UnityEngine;

public class Boss_Weapon : MonoBehaviour
{
    private Coroutine shootingCoroutine;
    private float time;
    private float intensityMultipler;

    [Header("Tube Nozzle Glow Details")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color emissionColor;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float sphereGlowTime = 0.5f;
    [SerializeField] private float targetIntensity = 5.5f;

    [Header("Laser Details")]
    [SerializeField] private GameObject[] lasers;

    private void Start()
    {
        intensityMultipler = Mathf.Pow(2f, targetIntensity);
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        time = 0;

        SetActiveGlowSphere(true);

        StartCoroutine(StartShootingCo());
    }

    private IEnumerator StartShootingCo()
    {
        while (time < sphereGlowTime)
        {
            time += Time.deltaTime;
            float timeRatio = time / sphereGlowTime;

            float animCurveValue = animCurve.Evaluate(timeRatio);
            Color finalColor = emissionColor * (animCurveValue * intensityMultipler);
            SetRendererColor(finalColor);

            yield return null;
        }

        // Set to finished target color;
        Color targetColor = emissionColor * intensityMultipler;
        SetRendererColor(targetColor);

        SetActiveLaserObjects(true);
    }

    private void SetRendererColor(Color targetColor)
    {
        foreach (var renderer in renderers)
        {
            renderer.material.SetColor("_EmissionColor", targetColor);
        }
    }

    private void SetActiveGlowSphere(bool active)
    {
        foreach (var renderer in renderers)
        {
            renderer.gameObject.SetActive(active);
        }
    }

    private void SetActiveLaserObjects(bool active)
    {
        foreach (var laser in lasers)
        {
            laser.SetActive(active);
        }
    }

    [ContextMenu("Reset")]
    private void ResetSphereGlow()
    {
        SetActiveGlowSphere(false);
        //SetRendererColor(emissionColor);
    }
}
