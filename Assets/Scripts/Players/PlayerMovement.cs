using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private Coroutine rollingCooldownCoroutine;

    // Movement's variables;
    private Vector3 playerViewportPosition;
    private Vector3 moveToPosition;
    private Vector2 mouseDelta;
    private float currentRoll;

    // Rolling calculation variables;
    private Vector3 moveToVector;
    private Vector3 moveToPositionWhileRoll;
    private float rollDirection;
    private bool isRolling = false;
    private bool rollingOnCooldown = false;
    private bool isTurning = false;

    // Aiming Components;
    [SerializeField] private Transform aimingpointTransform;

    #region Movements
    [Header("Movement Details")]
    [SerializeField] private bool move = true;
    [SerializeField] private float movementSpeed = 2.6f;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;
    #endregion

    #region Rotation
    [Header("Rotation Details")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 10f;
    [Space]
    [SerializeField] private bool roll = true;
    [SerializeField] private float rollForce = 50f;
    [SerializeField] private float rollReturnForce = 30f;
    [SerializeField] private float rollAngleLimit = 20f;
    #endregion

    #region Turning
    [Header("Turn Details")]
    [SerializeField] private float turningSpeed = 30.0f; 
    [SerializeField] private float turnLimitAngle = 75.0f;
    #endregion

    #region Rolling
    [Header("Rolling Movement Details")]
    [SerializeField] private float moveWhileRollSpeed = 10f;
    [SerializeField] private float moveWhileRollDistance = 0.55f;
    [SerializeField] private float rollingCooldownTime = 1.0f;
    #endregion

    //[Header("Debug")]
    //[SerializeField] private GameObject debugSphere;

    #region Player's Input
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
    public void OnRoll(InputValue value) => OnRollHandler();
    public void OnTurnRight(InputValue value) => OnTurnHandler(value.isPressed, true);
    public void OnTurnLeft(InputValue value) => OnTurnHandler(value.isPressed, false);
    #endregion

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (move)
            MovementHandler();

        //if (isRolling)
        //    MoveWhileRolling();
    }

    #region Movements
    private void MovementHandler()
    {
        moveToPosition = new Vector3(aimingpointTransform.localPosition.x, aimingpointTransform.localPosition.y, transform.localPosition.z);

        //------- Clamp Position --------
        float horizontalPosition = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        float verticalPosition = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);
        Vector3 moveToLocalPosition = new Vector3(horizontalPosition, verticalPosition, transform.localPosition.z);
        //-------------------------------

        Vector3 targetMovePoint = Vector3.Lerp(transform.localPosition, moveToLocalPosition, Time.deltaTime * movementSpeed); // Lerp fighter's movement;
        transform.localPosition = targetMovePoint;

        if (rotate)
            RotateHandler();

        if (isTurning)
            TurnHandler();
    }

    private void MoveWhileRolling()
    {
        Vector3 targetMovePoint = Vector3.Lerp(transform.localPosition, moveToPositionWhileRoll, Time.deltaTime * moveWhileRollSpeed);

        Transform tempTransform = transform;
        tempTransform.localPosition = targetMovePoint;

        //ClampedMovementPosition(tempTransform.position);
        transform.localPosition = GetMovementPositionFromCamView();
    }
    #endregion

    //------- Clamp position -------
    // With this clamp calculation, designer can set only 2 numbers, and it will works with every view point;
    private void ClampedMovementPosition(Vector3 targetMovePoint)
    {
        // This works too but if you change camera view or position in player gameobject, you have to change the limit numbers too.
        // So I changed to below clamp calculation that use camera's viewport instead;
        //moveToPosition.x = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        //moveToPosition.y = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);

        playerViewportPosition = Camera.main.WorldToViewportPoint(targetMovePoint);
        playerViewportPosition.x = Mathf.Clamp(playerViewportPosition.x, horizontalLimit.x, horizontalLimit.y);
        playerViewportPosition.y = Mathf.Clamp(playerViewportPosition.y, verticalLimit.x, verticalLimit.y);
    }

    // Calculate position from Camera Viewport
    private Vector3 GetMovementPositionFromCamView()
    {
        // ToViewport will make Z transform unstable, so I have to make it return to usual position;
        // That's why I didn't apply the new transform to player before this Z axis setting;
        Vector3 playerInScenePosition = Camera.main.ViewportToWorldPoint(playerViewportPosition);

        Vector3 localPosition = transform.parent.InverseTransformPoint(playerInScenePosition);
        localPosition.z = transform.localPosition.z;

        return localPosition;
    }

    private void RotateHandler()
    {
        Vector3 direction = aimingpointTransform.localPosition - transform.localPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (isTurning)
            return;

        // Implement Rolling
        if (roll)
        {
            // Limit roll value at edge of the screen;
            // This will limit roll value when player is at edge of screen, or player will roll even the plane doesn't go left or right;
            float mouseDeltaHorizontal = playerViewportPosition.x <= horizontalLimit.x || playerViewportPosition.x >= horizontalLimit.y ? 0 : mouseDelta.x;

            // Limit roll angle;
            float rollVolume = Mathf.Clamp(-mouseDeltaHorizontal * rollForce, -rollAngleLimit, rollAngleLimit);

            // Define roll speed, if there is no movement, player will return from rolling faster, this will give more feedback roll feel while playing;
            float rollSpeed = Mathf.Abs(mouseDeltaHorizontal) > 0.01f ? rollForce : rollReturnForce;

            // Apply calculated values to currentRoll;
            currentRoll = Mathf.Lerp(currentRoll, rollVolume, Time.deltaTime * rollSpeed);
            Vector3 currentEuler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, currentRoll);
        }
        //------------------

        // Add calculated Rotation to player
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnRollHandler()
    {
        if (rollingOnCooldown)
            return;

        if (isRolling)
            return;

        // Enable roll sequence, stop movement
        isRolling = true;
        //move = false;

        // Set move to target while roll variables
        //moveToVector = moveToPosition - transform.localPosition;
        //moveToPositionWhileRoll = transform.localPosition + (moveToVector.normalized * moveWhileRollDistance);

        // Set Rolling trigger animations;
        rollDirection = moveToPosition.x - transform.localPosition.x;
        string triggerText = rollDirection < 0 ? "RollLeft" : "RollRight";
        anim.SetTrigger(triggerText);
    }

    private void OnTurnHandler(bool isPressed, bool turningRight)
    {
        if (isPressed == isTurning)
            return;

        isTurning = isPressed;
        string text = turningRight ? "right" : "left";
        Debug.LogWarning($"Turning {text}");
    }

    private void TurnHandler()
    {
        Vector3 direction = aimingpointTransform.localPosition - transform.localPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, turnLimitAngle);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * turningSpeed);
    }

    public void StopRolling()
    {
        isRolling = false;
        //move = true;

        // may be add UI later;
        StartRollingCooldown();
    }

    private void StartRollingCooldown()
    {
        if (rollingCooldownCoroutine != null)
            StopCoroutine(rollingCooldownCoroutine);

        rollingCooldownCoroutine = StartCoroutine(RollingCooldownCo());
    }

    IEnumerator RollingCooldownCo()
    {
        rollingOnCooldown = true;

        yield return new WaitForSeconds(rollingCooldownTime);

        rollingOnCooldown = false;
    }
}
