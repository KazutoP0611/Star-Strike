using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerBoost : MonoBehaviour
{
    private float time = 0;

    private SplineAnimate splineAnimate;
    private bool boosting = false;
    private bool onCooldown = false;

    private Coroutine boostCooldownCoroutine;

    [SerializeField] private float getInMaxSpeedTimeMulitpler = 5.0f;
    [SerializeField] private float boostCooldownTime = 0.5f;
    [SerializeField] private float boostingDuration = 2.0f;

    #region Player Input
    public void OnBoost(InputValue value) => BoostHandler();
    #endregion

    private void BoostHandler() => boosting = true;

    private void Start()
    {
        splineAnimate = GetComponentInParent<SplineAnimate>();
    }

    private void Update()
    {
        if (onCooldown)
            return;

        if (!boosting)
            return;

        Boosting();
    }

    private void Boosting()
    {
        // Speed up spline animate time for certain duration
        time += Time.deltaTime;
        splineAnimate.ElapsedTime += Time.deltaTime * getInMaxSpeedTimeMulitpler;

        // Check for boosting duration
        if (time >= boostingDuration)
        {
            // Reset boost variable values;
            time = 0;
            boosting = false;

            // Start cooldown coroutine;
            StartBoostCooldownCo();
        }
    }

    // Cooldown Coroutine-------------------------------------------------
    private void StartBoostCooldownCo()
    {
        if (boostCooldownCoroutine != null)
            StopCoroutine(boostCooldownCoroutine);

        boostCooldownCoroutine = StartCoroutine(BoostCooldownCoroutine());
    }

    IEnumerator BoostCooldownCoroutine()
    {
        onCooldown = true;

        yield return new WaitForSeconds(boostCooldownTime);

        onCooldown = false;
    }
    //-------------------------------------------------------------------
}
