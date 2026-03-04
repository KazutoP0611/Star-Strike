using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private bool isBarrelRolling;
    private float barrelDirection; // -1 left, 1 right

    private Vector2 movement;
    private bool boost;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private float moveSmoothness = 6f;
    [SerializeField] private Vector2 movementBounds = new Vector2(8f, 4f);

    [Header("Rotation")]
    [SerializeField] private float maxRollAngle = 45f;
    [SerializeField] private float maxPitchAngle = 20f;
    [SerializeField] private float maxYawAngle = 15f;
    [SerializeField] private float rotationSmoothness = 6f;

    [Header("Barrel Roll")]
    [SerializeField] private float barrelRollSpeed = 720f;
    [SerializeField] private float doubleTapTime = 0.3f;

    private void Update()
    {
        // Move player methods
        MovementHandler();
        RotationHandler();
        BarrelRollHandler();
    }

    private void MovementHandler()
    {
        float horizontal = movement.x;
        float vertical = movement.y;
        float boostValue = boost ? boostMultiplier : 1f;

        Vector3 input = new Vector3(horizontal, vertical, 0f);
        targetPosition += input * moveSpeed * boostValue * Time.deltaTime;

        targetPosition.x = Mathf.Clamp(targetPosition.x, -movementBounds.x, movementBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -movementBounds.y, movementBounds.y);

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPosition,
            ref currentVelocity,
            1f / moveSmoothness
        );
    }

    private void RotationHandler()
    {
        if (isBarrelRolling) return;

        float horizontal = movement.x;
        float vertical = movement.y;

        float targetRoll = -horizontal * maxRollAngle;
        float targetPitch = -vertical * maxPitchAngle;
        float targetYaw = horizontal * maxYawAngle;

        Quaternion targetRot = Quaternion.Euler(targetPitch, targetYaw, targetRoll);

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRot,
            rotationSmoothness * Time.deltaTime
        );
    }

    private void BarrelRollHandler()
    {
        if (!isBarrelRolling) return;

        transform.Rotate(Vector3.forward, barrelDirection * barrelRollSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.localEulerAngles.z) < 5f)
        {
            isBarrelRolling = false;
            transform.localRotation = Quaternion.identity;
        }
    }

    private void StartBarrelRoll()
    {
        isBarrelRolling = true;
        barrelDirection = -movement.x;
    }

    #region PlayerInput
    public void OnMove(InputValue value) => movement = value.Get<Vector2>();

    public void OnBoost(InputValue value) => boost = value.isPressed;

    public void OnRoll(InputValue value) => StartBarrelRoll();
    #endregion
}
