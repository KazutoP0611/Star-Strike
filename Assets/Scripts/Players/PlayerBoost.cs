using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class PlayerBoost : MonoBehaviour
{
    private float time = 0;
    private int currentBoostCount;

    //private SplineAnimate splineAnimate;
    private bool boosting = false;
    private bool onCooldown = false;

    private Coroutine boostCooldownCoroutine;
    private Coroutine rechargeBoostCoroutine; // don't have any chance to use it yet;

    [Header("Controller & View Details")]
    [SerializeField] private BoosterView boosterView;
    [SerializeField] private LevelGenerator levelGenerator;

    [Header("Boost Details")]
    [Tooltip("How many time you can boost.")]
    [SerializeField] private int maxBoost = 2;
    [Space]
    [SerializeField] private float speedUpMultiplier = 5.0f;
    [SerializeField] private float boostCooldownTime = 0.5f;
    [SerializeField] private float boostingDuration = 2.0f;
    [Space]
    [Tooltip("Penalty waiting time if player used up all of boost. (Player has to wait longer than usual cool down time.)")]
    [SerializeField]
    private float boostRechargeTime = 2.5f;
    [Space]
    [SerializeField] private GameObject boostingPrefab;

    #region Player Input
    public void OnBoost(InputValue value) => BoostHandler();
    #endregion

    private void BoostHandler()
    {
        if (onCooldown)
            return;

        if (boosting)
            return;

        if (currentBoostCount <= 0)
            return;

        // Reduce booster count, update booster view
        currentBoostCount--;
        boosterView.SetBoostIndicators(currentBoostCount);

        // Set camera to zoom out
        //cameraController.CameraToBoostPosition();
        CameraController.instance.CameraToBoostPosition();

        // TODO : may be add some truster effect a little animation, like a little blib, a litte feedback to feel that fighter is accelerating;

        // Start boost sequence
        boosting = true;
        boostingPrefab.SetActive(true);

        // Speed up level's movement speed;
        levelGenerator.SetLevelMovementSpeed(speedUpMultiplier);
    }

    private void Start()
    {
        //splineAnimate = GetComponentInParent<SplineAnimate>();

        currentBoostCount = maxBoost;
        boosterView.Intialize(maxBoost);
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
        time += Time.deltaTime;

        // Check for boosting duration
        // If "true", stop boosting and get to cooldown
        if (time >= boostingDuration)
        {
            // Set camera to normal position
            //cameraController.CameraToNormalPosition();
            CameraController.instance.CameraToNormalPosition();

            // Reset boost variable values;
            time = 0;
            boosting = false;
            boostingPrefab.SetActive(false);

            // Reverse level's movement speed;
            levelGenerator.SetLevelMovementSpeed(1.0f);

            // Start cooldown coroutine;
            StartBoostCooldownCo();
        }
    }

    #region Cooldown Coroutine
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

        if (currentBoostCount < maxBoost)
            StartRechargeBoostCo();
    }
    #endregion

    #region Boost-Recharge Coroutine
    private void StartRechargeBoostCo()
    {
        // Start recharge
        StartCoroutine(RechargeCoroutine());
    }

    IEnumerator RechargeCoroutine()
    {
        yield return new WaitForSeconds(boostRechargeTime);

        currentBoostCount++;
        boosterView.SetBoostIndicators(currentBoostCount);
    }
    #endregion
}
