using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    private Player player;
    private Coroutine ShowDamageScreenCoroutine;
    private float elapsedTime;

    [Header("Damage Screen Details")]
    [SerializeField] private CanvasGroup damageScreenCanvas;
    [SerializeField] private float damageScreenDuration = 1.2f;
    [SerializeField] private float fadeDuration = 0.15f;

    protected override  void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    public override void OnDamage(Vector3 hitPoint)
    {
        base.OnDamage(hitPoint);

        //may be change fighter's material process too?

        //add short immortality too?

        //do red screen when hit;
        ShowDamageScreen();
    }

    protected override IEnumerator OnDamageCo()
    {
        elapsedTime = 0f;
        float blinkTimer = 0f;
        bool blink = false;

        while (elapsedTime < player.immortalTime)
        {
            elapsedTime += Time.deltaTime;
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= onDamageTime)
            {
                blinkTimer = 0f;

                blink = !blink;

                foreach (var renderer in renderers)
                {
                    renderer.material = blink ? onDamageMat : originalMat;
                }
            }

            yield return null;
        }

        // Make sure the normal material is restored.
        foreach (var renderer in renderers)
        {
            renderer.material = originalMat;
        }
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
