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
    protected Material originalMat;

    public Coroutine onDamageCoroutine { get; private set; }

    [Header("On Damage Details")]
    [SerializeField] protected float onDamageTime = 0.25f;
    [SerializeField] protected Material onDamageMat;
    [SerializeField] protected Renderer[] renderers;

    [Header("Particle Details")]
    [SerializeField] private GameObject onDamageParticle;
    [SerializeField] private float damageParticleScale = 1f;
    [Space]
    [SerializeField] private GameObject onDestroyParticle;
    [SerializeField] private float destroyParticleScale = 0.75f;

    protected virtual void Awake()
    {
        // If model has multiple materials, this will not give correct material. But in this case it will be fine.
        // I may implement this later.
        if (renderers.Count() > 0)
            originalMat = renderers[0].material;
        else
            Debug.LogWarning("Renderers haven't been set in inspector yet.");
    }

    public void CreateEffect(ParticleType particleType) => InstantiateEffect(particleType, transform.position);

    public void CreateEffect(ParticleType particleType, Vector3 hitPoint) => InstantiateEffect(particleType, hitPoint);

    protected void InstantiateEffect(ParticleType particleType, Vector3 hitPoint)
    {
        GameObject explodeParticle = Instantiate(particleType == ParticleType.Damage ? onDamageParticle : onDestroyParticle, hitPoint, transform.localRotation);
        explodeParticle.transform.localScale = Vector3.one * (particleType == ParticleType.Damage ? damageParticleScale : destroyParticleScale);
    }

    public virtual void OnDamage(Vector3 hitPoint)
    {
        CreateEffect(ParticleType.Damage, hitPoint);

        if (onDamageCoroutine != null)
            StopCoroutine(onDamageCoroutine);

        onDamageCoroutine = StartCoroutine(OnDamageCo());
    }

    protected virtual IEnumerator OnDamageCo()
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

    //This should be only at boss's vfx?
    public void SetActiveEmission(bool active)
    {
        if (active)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
        else
        {
            foreach (var renderer in renderers)
            {
                renderer.material.DisableKeyword("_EMISSION");
            }
        }
    }

    public void OnDestroy()
    {
        StopCoroutine(onDamageCoroutine);

        foreach (var renderer in renderers)
        {
            renderer.gameObject.SetActive(false);
        }
    }
}
