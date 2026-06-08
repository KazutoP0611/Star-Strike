using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    private Coroutine ShowDamageScreenCoroutine;

    [Header("Damage Screen Details")]
    [SerializeField] private CanvasGroup damageScreenCanvas;
    [SerializeField] private float damageScreenDuration = 1.2f;
    [SerializeField] private float fadeDuration = 0.15f;

    public override void OnDamage(Vector3 hitPoint)
    {
        base.OnDamage(hitPoint);

        //may be change fighter's material process too?

        //add short immortality too?

        //do red screen only when hit;
        ShowDamageScreen();

        //check hp and show warning according to hp level?;
    }

    private void ShowDamageScreen()
    {
        if (ShowDamageScreenCoroutine != null)
            StopCoroutine(ShowDamageScreenCoroutine);

        ShowDamageScreenCoroutine = StartCoroutine(ShowDamageScreenCo());
    }

    private IEnumerator ShowDamageScreenCo()
    {
        float elaspedTime = 0;
        while (elaspedTime < fadeDuration)
        {
            elaspedTime += Time.deltaTime;
            damageScreenCanvas.alpha = elaspedTime / fadeDuration;
            yield return null;
        }

        yield return new WaitForSeconds(damageScreenDuration);

        while (elaspedTime > 0)
        {
            elaspedTime -= Time.deltaTime;
            damageScreenCanvas.alpha = elaspedTime / fadeDuration;
            yield return null;
        }
    }
}
