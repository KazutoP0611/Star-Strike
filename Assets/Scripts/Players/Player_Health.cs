using UnityEngine;
using UnityEngine.UI;

public class Player_Health : Entity_Health
{
    [SerializeField] private Slider hpBar;

    protected override void Initialize()
    {
        UpdateHealthBar();
    }

    public override void TakeDamage(Collider hitObject)
    {
        base.TakeDamage(hitObject);

        UpdateHealthBar();

        //camera shake
        CameraController.instance.CameraShake();
    }

    private void UpdateHealthBar()
    {
        float hpRatio = (float)m_currentHealth / maxHealth;
        hpBar.value = hpRatio;
    }
}
