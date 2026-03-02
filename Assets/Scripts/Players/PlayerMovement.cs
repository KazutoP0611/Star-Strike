using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 movement;
    private float currentRoll = 0f;

    [Header("Movement Details")]
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private Vector2 xLimitMoveRange;
    [SerializeField] private Vector2 yLimitMoveRange;

    [Header("Rotate Details")]
    [SerializeField] private bool controlBothRollSpeed = true;
    [Space]
    [SerializeField] private float rollSpeed = 10.0f;
    [SerializeField] private float returnRollSpeed = 3.0f;
    [SerializeField] private float limitRollAngle = 20.0f;
    [Space]
    [SerializeField] private float pitchSpeed = 10.0f;
    [SerializeField] private float returnPitchSpeed = 3.0f;
    [SerializeField] private float limitPitchAngle = 20.0f;

    private void Update()
    {
        // Move player methods
        ProcessTranslation();
        ProcessRotation();
    }

    private void ProcessTranslation()
    {
        // Get and calculate input value to x and y movement values;
        float xOffset = movement.x * movementSpeed * Time.deltaTime;
        float yOffset = movement.y * movementSpeed * Time.deltaTime;

        // Add calculated result to player's position, these values will represent current points player should move to;
        float rawXPos = transform.localPosition.x + xOffset;
        float rawYPos = transform.localPosition.y + yOffset;

        // Clamp x and y position values to limited set values, so player object would never move out of camera's view;
        float clampedXPos = Mathf.Clamp(rawXPos, xLimitMoveRange.x, xLimitMoveRange.y);
        float clampedYPos = Mathf.Clamp(rawYPos, yLimitMoveRange.x, yLimitMoveRange.y);

        // Assign clamped position to player's localPosition;
        transform.localPosition = new Vector3(clampedXPos, clampedYPos, 0);
    }

    

    private void ProcessRotation()
    {
        // Either above or below calculations are fine;
        // But the shorter one seems more smoother;

        if (controlBothRollSpeed)
        {
            // This gives more control in rotation and speed, but may take a few calculation;
            #region Learn how to think in euler and quaternion
            float rollOffset = movement.x * -rollSpeed;
            float pitchOffset = movement.y * -pitchSpeed;

            // Convert 0-360 degree to -180 to 180 degree points;
            float currentRollAngle = ((transform.localEulerAngles.z + 180) % 360) - 180;
            float currentPitchAngle = ((transform.localEulerAngles.x + 180) % 360) - 180;

            // If input or result of calculated movement == 0, this will show target rotation;
            float targetRollAngle = rollOffset == 0 ? 0 : currentRollAngle + rollOffset;
            float targetPitchAngle = pitchOffset == 0 ? 0 : currentPitchAngle + pitchOffset;

            // Lerp rotation in EulerAngles;
            float LerpedRollAngle = Mathf.Lerp(currentRollAngle, targetRollAngle, Time.deltaTime * returnRollSpeed);
            float LerpedPitchAngle = Mathf.Lerp(currentPitchAngle, targetPitchAngle, Time.deltaTime * returnPitchSpeed);

            // Clamp angle to limited angle;
            float clampedLerpedRollAngle = Mathf.Clamp(LerpedRollAngle, -limitRollAngle, limitRollAngle);
            float clampedLerpedYawAngle = Mathf.Clamp(LerpedRollAngle, -20.0f, 20.0f);
            float clampedLerpedPitchAngle = Mathf.Clamp(LerpedPitchAngle, -limitPitchAngle, limitPitchAngle);

            // Assign result angle to player;
            transform.localRotation = Quaternion.Euler(clampedLerpedPitchAngle, -clampedLerpedYawAngle, clampedLerpedRollAngle);
            #endregion
        }
        else
        {
            #region A short version
            // But this can not control both of roll speed;
            Quaternion targetRotation = Quaternion.Euler(-pitchSpeed * movement.y, 0f, -rollSpeed * movement.x); // rollSpeed & pitchSpeed become limit angle instead;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * returnRollSpeed); // returnRollSpeed and returnPitchSpeed become speed of rolling;
            #endregion
        }
    }

    #region PlayerInput
    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
    #endregion
}
