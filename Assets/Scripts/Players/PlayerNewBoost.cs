using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerNewBoost : MonoBehaviour
{
    private float currentBoost = 0;

    //private SplineAnimate splineAnimate;
    private bool changingSpeed = false;
    private bool onCooldown = false;

    private Coroutine boostCooldownCoroutine;

    [Header("Controller & View Details")]
    [SerializeField] private Slider boostBar;
    [SerializeField] private LevelGenerator levelGenerator;

    [Header("Boost Details")]
    [SerializeField] private float maxBoost = 10.0f;
    [SerializeField] private float speedUpMultiplier = 5.0f;
    [SerializeField] private float slowDownMultiplier = 0.5f;

    [Header("Boost Multiplier Details")]
    [SerializeField] private float boostConsumeMultiplier = 3.5f;
    [SerializeField] private float boostRechargeMultiplier = 2.0f;

    #region Player Input
    public void OnBoostNew(InputValue value) => OnBoostHandler(value.isPressed, true);
    public void OnBreak(InputValue value) => OnBoostHandler(value.isPressed, false);
    #endregion

    private void Start()
    {
        currentBoost = maxBoost;
        UpdateBoostBar();
    }

    private void Update()
    {
        if (changingSpeed == false)
            return;

        Boosting();
    }

    private void OnBoostHandler(bool isSpeeding, bool isBoosting)
    {
        if (onCooldown)
            return;

        changingSpeed = isSpeeding;
        
        if (isSpeeding == true)
        {
            //set level speed up
            // Speed up level's movement speed;
            float speedMultiplier = isBoosting ? speedUpMultiplier : slowDownMultiplier;
            levelGenerator.SetLevelMovementSpeed(speedMultiplier);
        }
        else
        {
            //set level speed down
            levelGenerator.SetLevelMovementSpeed(1.0f);

            //cooldown
            CooldownBoost();
        }
    }

    private void Boosting()
    {
        currentBoost -= (Time.deltaTime * boostConsumeMultiplier);
        UpdateBoostBar();
    }

    private void UpdateBoostBar()
    {
        float fill = Mathf.Clamp01(currentBoost / maxBoost);
        boostBar.value = fill;
    }

    private void CooldownBoost()
    {
        onCooldown = true;

        if (boostCooldownCoroutine != null)
            StopCoroutine(boostCooldownCoroutine);

        boostCooldownCoroutine = StartCoroutine(CooldownCo());
    }

    private IEnumerator CooldownCo()
    {
        while (currentBoost < maxBoost)
        {
            currentBoost += (Time.deltaTime * boostRechargeMultiplier);
            UpdateBoostBar();

            yield return null;
        }

        currentBoost = maxBoost;
        onCooldown = false;
    }
}
