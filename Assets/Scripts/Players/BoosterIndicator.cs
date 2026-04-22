using UnityEngine;
using UnityEngine.UI;

public class BoosterIndicator : MonoBehaviour
{
    [SerializeField] private Image boosterImg;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    public void SetBoosterSprite(bool on)
    {
        boosterImg.color = on ? onColor : offColor;
    }
}
