using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 playerViewportPosition;
    private Vector3 moveToPosition;
    private Vector2 mouseDelta;
    private float currentRoll;
    private float rollDirection;

    [SerializeField] private Transform aimingpointTransform;

    [Header("Movement Details")]
    [SerializeField] private bool move = true;
    [Space]
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float movementMagnitude = 0.1f;
    [Space]
    [SerializeField] private Vector2 horizontalLimit;
    [SerializeField] private Vector2 verticalLimit;

    [Header("Rotation Details")]
    [SerializeField] private bool rotate = true;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool roll = true;
    [SerializeField] private float rollForce = 50f;
    [SerializeField] private float rollReturnForce = 50f;
    [SerializeField] private float rollAngleLimit = 30f;

    #region Player's Input
    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
    public void OnRoll(InputValue value) => RollHandler();
    #endregion

    private void Update()
    {
        if (move)
            MovementHandler();
    }

    private void MovementHandler()
    {
        moveToPosition = new Vector3(aimingpointTransform.position.x, aimingpointTransform.position.y, transform.position.z);
        // This movement is too linear, it will move without damping;
        //Vector3 moveToPosition = transform.position + (Vector3)mouseDelta * movementMagnitude;

        // Lerp fighter's movement;
        Vector3 targetMovePoint = Vector3.Lerp(transform.position, moveToPosition, Time.deltaTime * movementSpeed);

        // This works too but if you change camera view or position in player gameobject, you have to change the limit numbers too.
        // So I changed to below clamp calculation that use camera's viewport instead;
        //moveToPosition.x = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        //moveToPosition.y = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);

        //------- Clamp position -------
        // With this clamp calculation, designer can set only 2 numbers, and it will works with every view point;
        playerViewportPosition = Camera.main.WorldToViewportPoint(targetMovePoint);
        playerViewportPosition.x = Mathf.Clamp(playerViewportPosition.x, horizontalLimit.x, horizontalLimit.y);
        playerViewportPosition.y = Mathf.Clamp(playerViewportPosition.y, verticalLimit.x, verticalLimit.y);
        //------------------------------

        Vector3 playerWorldPosition = Camera.main.ViewportToWorldPoint(playerViewportPosition);

        // ToViewport will make Z transform unstable, so I have to make it return to usual position;
        // That's why I didn't apply the new transform to player before this Z axis setting;
        playerWorldPosition.z = transform.position.z;

        // Apply calculated new position to player;
        transform.position = playerWorldPosition;

        if (rotate)
            RotateHandler();
    }

    private void RotateHandler()
    {
        Vector3 direction = aimingpointTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Implement Rolling
        if (roll)
        {
            // This will limit roll value when player is at edge of screen, or player will roll even the plane doesn't go left or right;
            float mouseDeltaHorizontal = playerViewportPosition.x == horizontalLimit.x || playerViewportPosition.x == horizontalLimit.y ? 0 : mouseDelta.x;

            // Limit roll angle;
            float rollVolumn = Mathf.Clamp(-mouseDeltaHorizontal * rollForce, -rollAngleLimit, rollAngleLimit);

            // Define roll speed, if there is no movement, player will return from rolling faster, this will give more feedback roll feel while playing;
            float rollSpeed = Mathf.Abs(mouseDeltaHorizontal) > 0.01f ? rollForce : rollReturnForce;

            // Apply calculated values to currentRoll;
            currentRoll = Mathf.Lerp(currentRoll, rollVolumn, Time.deltaTime * rollSpeed);
            Vector3 currentEuler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, currentRoll);
        }
        //------------------

        // Add calculated Rotation to player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void RollHandler()
    {
        rollDirection = moveToPosition.x - transform.position.x;
        Debug.Log(rollDirection < 0 ? "Roll Left" : "Roll Right");
    }
}
