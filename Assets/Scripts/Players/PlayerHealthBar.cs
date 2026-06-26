using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image sliderImage;

    [Header("Health Indicator Details")]
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private float warningHealthPoint = 0.5f;
    [SerializeField] private Color warningHealthColor;
    [SerializeField] private float dangerHealthPoint = 0.3f;
    [SerializeField] private Color dangerHealthColor;

    public void UpdateHealthBar(float value)
    {
        hpBar.value = value;

        if (value > warningHealthPoint)
            sliderImage.color = fullHealthColor;
        else if (value > dangerHealthPoint)
            sliderImage.color = warningHealthColor;
        else
            sliderImage.color = dangerHealthColor;
    }
}
