using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Canon : MonoBehaviour
{
    private Material gunMuzzleMat;
    private Color beforeShootColor;

    private float currentTime;

    private Coroutine shootCoroutine;

    public const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    public const string EMISSIVE_KEYWORD = "_EMISSION";

    [SerializeField] private Renderer gunMuzzleRenderer;
    [SerializeField] private float shootTime = 0.5f;
    [SerializeField] private float waitSecsBeforeShoot = 0.05f;
    [SerializeField] private Vector2 intensity;

    private void Start()
    {
        if (gunMuzzleRenderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORD)
            && gunMuzzleRenderer.material.HasColor(EMISSIVE_COLOR_NAME))
        {
            gunMuzzleMat = gunMuzzleRenderer.material;
            beforeShootColor = gunMuzzleMat.GetColor(EMISSIVE_COLOR_NAME);
        }
    }

    [ContextMenu("Shoot")]
    private void Shoot()
    {
        if (shootCoroutine != null)
            StopCoroutine(shootCoroutine);

        shootCoroutine = StartCoroutine(ShootCo());
    }

    IEnumerator ShootCo()
    {
        SetMuzzleColor(beforeShootColor);

        currentTime = 0;
        Color color = beforeShootColor;
        float intensityLength = intensity.y - intensity.x;

        while (currentTime < shootTime)
        {
            currentTime += Time.deltaTime;
            float brightness = Mathf.Lerp(0, intensityLength, currentTime / shootTime);

            Color newColor = new Color(
                color.r * Mathf.Pow(2, brightness),
                color.g * Mathf.Pow(2, brightness),
                color.b * Mathf.Pow(2, brightness),
                color.a
            );

            SetMuzzleColor(newColor);
            yield return null;
        }

        yield return new WaitForSeconds(waitSecsBeforeShoot);
        SetMuzzleColor(beforeShootColor);
    }

    private void SetMuzzleColor(Color color) => gunMuzzleMat.SetColor(EMISSIVE_COLOR_NAME, color);
}
