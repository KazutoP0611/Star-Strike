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

    [SerializeField] private CameraController cameraController;

    [Header("Boost Details")]
    [SerializeField] private float getInMaxSpeedTimeMulitpler = 5.0f;
    [SerializeField] private float boostCooldownTime = 0.5f;
    [SerializeField] private float boostingDuration = 2.0f;
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

        // Set camera to zoom out
        cameraController.CameraToBoostPosition();

        // TODO : may be add some truster effect a little animation, like a little blib, a litte feedback to feel that fighter is accelerating;

        // Start boost sequence
        boosting = true;
        boostingPrefab.SetActive(true);
    }

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
        // If "true", stop boosting and get to cooldown
        if (time >= boostingDuration)
        {
            // Set camera to normal position
            cameraController.CameraToNormalPosition();

            // Reset boost variable values;
            time = 0;
            boosting = false;
            boostingPrefab.SetActive(false);

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
    }
    #endregion
}
