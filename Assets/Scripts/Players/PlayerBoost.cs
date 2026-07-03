using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerBoost : MonoBehaviour
{
    private float currentBoost = 0;

    private bool changingSpeed = false;
    private bool onCooldown = false;

    private Coroutine boostCooldownCoroutine;
    private Player_SFX playerSFX;

    [Header("Controller & View Details")]
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private PlayerBoostBar playerBoostBar;

    [Header("Boost Parameter Details")]
    [SerializeField] private float maxBoost = 10.0f;
    [SerializeField] private float speedUpMultiplier = 5.0f;
    [SerializeField] private float slowDownMultiplier = 0.5f;
    [Space]
    [SerializeField] private float boostConsumeMultiplier = 3.5f;
    [SerializeField] private float boostRechargeMultiplier = 2.0f;

    [Header("Effect Details")]
    [SerializeField] private GameObject boostVisual;
    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip boostSound;
    [SerializeField] private AudioClip breakSound;

    #region Player Input
    public void OnBoostNew(InputValue value) => OnBoostHandler(value.isPressed, true);
    public void OnBreak(InputValue value) => OnBoostHandler(value.isPressed, false);
    #endregion

    private void Awake()
    {
        playerSFX = GetComponent<Player_SFX>();
    }

    private void Start()
    {
        currentBoost = maxBoost;
        UpdateBoostValue();
        playerBoostBar.ChangeColor(true);
    }

    private void Update()
    {
        if (changingSpeed == false)
            return;

        Boosting();
    }

    private void OnBoostHandler(bool isPressing, bool isBoosting)
    {
        if (onCooldown)
        {
            // Maybe show warning color in boost bar;

            return;
        }

        if (isPressing != changingSpeed)
        {
            changingSpeed = isPressing;

            if (isPressing == true)
            {
                // Set boost or slow down camera position
                if (isBoosting)
                {
                    boostVisual.SetActive(true);
                    CameraController.instance.CameraToBoostPosition();
                    playerSFX.PlaySpeedChangeSound(ChangeSpeedSound.Boost);
                }
                else
                {
                    CameraController.instance.CameraToBreakPosition();
                    playerSFX.PlaySpeedChangeSound(ChangeSpeedSound.Break);
                }
            }
        }

        if (isPressing == true)
        {
            // Set level speed up
            float speedMultiplier = isBoosting ? speedUpMultiplier : slowDownMultiplier;
            levelGenerator.SetLevelMovementSpeed(speedMultiplier);
        }
        else
        {
            StartBoostCooldown();
        }
    }

    private void Boosting()
    {
        currentBoost -= (Time.deltaTime * boostConsumeMultiplier);
        UpdateBoostValue();

        if (currentBoost <= 0)
            StartBoostCooldown();
    }

    public void StartBoostCooldown()
    {
        changingSpeed = false;

        // Play slow down camera animation
        CameraController.instance.CameraToNormalPosition();

        // Closing boost visual
        boostVisual.SetActive(false);

        // set level speed down
        levelGenerator.SetLevelMovementSpeed(1.0f);

        // start cooldown
        CooldownBoost();
    }

    private void UpdateBoostValue()
    {
        float fill = Mathf.Clamp01(currentBoost / maxBoost);

        playerBoostBar.UpdateBoostBar(fill);
    }

    private void CooldownBoost()
    {
        playerBoostBar.ChangeColor(false);
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
            UpdateBoostValue();

            yield return null;
        }

        // I don't know why, but if I don't wait, boost bar sometimes doesn't change to normal color;
        yield return new WaitForEndOfFrame();

        currentBoost = maxBoost;
        UpdateBoostValue();
        playerBoostBar.ChangeColor(true);

        onCooldown = false;

        CameraController.instance.ResetCameraTriggerParams();
    }
}
