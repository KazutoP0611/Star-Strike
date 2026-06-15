using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_FadeScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public Coroutine fadeCoroutine { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        FadeOut(1.0f);
    }

    public void FadeIn(float fadeInSecs = 0.5f)
    {
        canvasGroup.alpha = 0;
        DoFade(1, fadeInSecs);
    }

    public void FadeOut(float fadeInSecs = 0.5f)
    {
        canvasGroup.alpha = 1;
        DoFade(0, fadeInSecs);
    }

    private void DoFade(float targetValue, float fadeInSecs)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeEffectCo(targetValue, fadeInSecs));
    }

    private IEnumerator FadeEffectCo(float targetValue, float fadeInSecs)
    {
        float startValue = canvasGroup.alpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeInSecs)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log($"time {elapsedTime}");
            float alpha = Mathf.Lerp(startValue, targetValue, elapsedTime / fadeInSecs);
            canvasGroup.alpha = alpha;

            yield return null;
        }

        canvasGroup.alpha = targetValue;
    }
}
