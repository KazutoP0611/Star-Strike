using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;

    private Vector3 playerViewportPosition;
    private Vector3 moveToPosition;
    private Vector2 mouseDelta;
    private float currentRotate;

    [SerializeField] private Transform aimingpointTransform;

    #region Movements
    [Header("Movement Details")]
    [SerializeField] private bool move = true;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float movementMagnitude = 0.1f;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;
    #endregion

    #region Rotation
    [Header("Rotation Details")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 10f;
    [Space]
    [SerializeField] private bool tilt = true;
    [SerializeField] private float rotateForce = 50f;
    [SerializeField] private float rotateReturnForce = 30f;
    [SerializeField] private float rotateAngleLimit = 20f;
    #endregion

    #region Rolling
    [Header("Roll Details")]
    [SerializeField] private float moveWhileRollSpeed = 10f;
    [SerializeField] private float moveWhileRollDistance = 0.55f;

    private Vector3 moveToVector;
    private Vector3 moveToPositionWhileRoll;
    private float rollDirection;
    private bool isRolling = false;
    #endregion

    [Header("Debug")]
    [SerializeField] private GameObject debugSphere;

    #region Player's Input
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
    public void OnRoll(InputValue value) => RollHandler();
    #endregion

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (move)
            MovementHandler();

        if (isRolling)
            MoveWhileRolling();
    }

    #region Movements
    private void MovementHandler()
    {
        moveToPosition = new Vector3(aimingpointTransform.localPosition.x, aimingpointTransform.localPosition.y, transform.localPosition.z);

        // Vector3 moveToPosition = transform.position + (Vector3)mouseDelta * movementMagnitude; // This movement is too linear, it will move without damping;
        // So I used below « calculation instead;
        Vector3 targetMovePoint = Vector3.Lerp(transform.localPosition, moveToPosition, Time.deltaTime * movementSpeed); // Lerp fighter's movement;

        // Clamp position;
        GetClampedMovementPosition(targetMovePoint);

        // Apply calculated new position to player;
        transform.localPosition = GetMovementPositionFromCamView();

        if (rotate)
            RotateHandler();
    }

    private void MoveWhileRolling()
    {
        Vector3 targetMovePoint = Vector3.Lerp(transform.localPosition, moveToPositionWhileRoll, Time.deltaTime * moveWhileRollSpeed);
        GetClampedMovementPosition(targetMovePoint);
        transform.localPosition = GetMovementPositionFromCamView();

        //debugSphere.transform.position = moveToPositionWhileRoll;
    }
    #endregion

    //------- Clamp position -------
    // With this clamp calculation, designer can set only 2 numbers, and it will works with every view point;
    private void GetClampedMovementPosition(Vector3 targetMovePoint)
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
        playerInScenePosition.z = transform.localPosition.z;

        return playerInScenePosition;
    }

    private void RotateHandler()
    {
        Vector3 direction = aimingpointTransform.localPosition - transform.localPosition;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Implement Rolling
        if (tilt)
        {
            // This will limit roll value when player is at edge of screen, or player will roll even the plane doesn't go left or right;
            float mouseDeltaHorizontal = playerViewportPosition.x == horizontalLimit.x || playerViewportPosition.x == horizontalLimit.y ? 0 : mouseDelta.x;

            // Limit roll angle;
            float rotateVolumn = Mathf.Clamp(-mouseDeltaHorizontal * rotateForce, -rotateAngleLimit, rotateAngleLimit);

            // Define roll speed, if there is no movement, player will return from rolling faster, this will give more feedback roll feel while playing;
            float rollSpeed = Mathf.Abs(mouseDeltaHorizontal) > 0.01f ? rotateForce : rotateReturnForce;

            // Apply calculated values to currentRoll;
            currentRotate = Mathf.Lerp(currentRotate, rotateVolumn, Time.deltaTime * rollSpeed);
            Vector3 currentEuler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, currentRotate);
        }
        //------------------

        // Add calculated Rotation to player
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void RollHandler()
    {
        if (isRolling)
            return;

        // Enable roll sequence, stop movement
        isRolling = true;
        move = false;

        // Set movement while roll variables
        moveToVector = moveToPosition - transform.localPosition;
        moveToPositionWhileRoll = transform.localPosition + (moveToVector.normalized * moveWhileRollDistance);

        // Set Rolling trigger animations;
        rollDirection = moveToPosition.x - transform.localPosition.x;
        string triggerText = rollDirection < 0 ? "RollLeft" : "RollRight";
        anim.SetTrigger(triggerText);
    }

    public void StopRolling()
    {
        isRolling = false;
        move = true;

        // may be set cooldown later, with UI
    }
}
