using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen instance;

    public delegate void FadeFinished();
    private FadeFinished fadeFinished;

    private Coroutine FadeCo;

    [SerializeField] private bool doFadeOutOnStart = true;

    [Header("General Details")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;

    public void SetFadeFinishDelegate(FadeFinished fadeFinsihCallback) => fadeFinished = fadeFinsihCallback;

    private void Start()
    {
        if (doFadeOutOnStart)
            FadeOut();
    }

    public void FadeIn() => DoFade(1);

    public void FadeOut() => DoFade(0);

    private void DoFade(float fadeTargetAlpha)
    {
        if (FadeCo != null)
            StopCoroutine(FadeCo);

        FadeCo = StartCoroutine(FadeCoroutine(fadeTargetAlpha));
    }

    private IEnumerator FadeCoroutine(float fadeTargetAlpha)
    {
        float timePassed = 0;
        float startFadeAlpha = canvasGroup.alpha;

        // Set canvas group interactable should it be
        // - Fade Target Alpha == 1: block interactable
        // - Fade Target Alpha == 0: no black interactable
        // fade in or fade out, fade screen has to be uninteractable at first fade
        canvasGroup.interactable = true;

        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, fadeTargetAlpha, timePassed / fadeDuration);

            yield return null;
        }

        // Finishing up canvas group
        // Set canvas group to to be target alpha
        canvasGroup.alpha = fadeTargetAlpha;

        // If fade in (target alpha == 1) fade screen is uninteractable
        canvasGroup.interactable = fadeTargetAlpha == 1;

        fadeFinished?.Invoke();
    }
}
