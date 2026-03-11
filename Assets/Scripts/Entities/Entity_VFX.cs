using System.Collections;
using System.Linq;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private Coroutine onDamageCoroutine;
    private Material originalMat;

    [Header("On Damage Details")]
    [SerializeField] private float onDamageTime = 0.25f;
    [SerializeField] private Material onDamageMat;
    [SerializeField] private Renderer[] renderers;

    [Header("Particle Details")]
    [SerializeField] private GameObject onDestroyParticle;
    [SerializeField] private float destroyParticleScale = 0.75f;

    private void Awake()
    {
        // If model has multiple materials, this will not give correct material. But in this case it will be fine.
        // I may implement this later.
        if (renderers.Count() > 0)
            originalMat = renderers[0].material;
        else
            Debug.LogWarning("Renderers haven't been set in inspector yet.");
    }

    public void CreateOnDeadEffect()
    {
        GameObject explodeParticle = Instantiate(onDestroyParticle, transform.position, transform.localRotation);
        explodeParticle.transform.localScale = Vector3.one * destroyParticleScale;
    }

    public void OnDamage()
    {
        if (onDamageCoroutine != null)
            StopCoroutine(onDamageCoroutine);

        onDamageCoroutine = StartCoroutine(OnDamageCo());
    }

    IEnumerator OnDamageCo()
    {
        // Change materials to on damage material;
        foreach (var renderer in renderers)
        {
            renderer.material = onDamageMat;
        }

        yield return new WaitForSeconds(onDamageTime);

        // Return materials to normal material;
        foreach (var renderer in renderers)
        {
            renderer.material = originalMat;
        }
    }
}
