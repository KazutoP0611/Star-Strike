using UnityEngine;
using UnityEngine.UI;

public class PlayerBoostBar : MonoBehaviour
{
    [SerializeField] private Slider boostBar;
    [SerializeField] private Image sliderImage;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color rechargeColor;


    public void UpdateBoostBar(float fill)
    {
        boostBar.value = fill;
    }

    public void ChangeColor(bool normal)
    {
        sliderImage.color = normal ? normalColor : rechargeColor;
    }
}
