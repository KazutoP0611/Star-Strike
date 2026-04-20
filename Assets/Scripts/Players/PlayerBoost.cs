using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerBoost : MonoBehaviour
{
    private SplineAnimate splineAnimate;

    #region Player Input
    public void OnBoost(InputValue value) => BoostHandler();
    #endregion

    private void Start()
    {
        splineAnimate = GetComponentInParent<SplineAnimate>();
    }

    private void BoostHandler()
    {
        splineAnimate.MaxSpeed = 5.0f;
    }
}
