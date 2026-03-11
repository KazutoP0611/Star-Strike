using System.Collections;
using System.Linq;
using UnityEngine;

public enum ParticleType
{
    Damage,
    Dead
}

public class Entity_VFX : MonoBehaviour
{
    private Coroutine onDamageCoroutine;
    private Material originalMat;

    [Header("On Damage Details")]
    [SerializeField] private float onDamageTime = 0.25f;
    [SerializeField] private Material onDamageMat;
    [SerializeField] private Renderer[] renderers;

    [Header("Particle Details")]
    [SerializeField] private GameObject onDamageParticle;
    [SerializeField] private float damageParticleScale = 1f;
    [Space]
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

    public void CreateEffect(ParticleType particleType)
    {
        GameObject explodeParticle = Instantiate(particleType == ParticleType.Damage ? onDamageParticle : onDestroyParticle, transform.position, transform.localRotation);
        explodeParticle.transform.localScale = Vector3.one * (particleType == ParticleType.Damage ? damageParticleScale : destroyParticleScale);
    }

    public void CreateEffect(ParticleType particleType, Vector3 hitPoint)
    {
        GameObject explodeParticle = Instantiate(particleType == ParticleType.Damage ? onDamageParticle : onDestroyParticle, hitPoint, transform.localRotation);
        explodeParticle.transform.localScale = Vector3.one * (particleType == ParticleType.Damage ? damageParticleScale : destroyParticleScale);
    }

    public void OnDamage(Vector3 hitPoint)
    {
        CreateEffect(ParticleType.Damage, hitPoint);

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
