using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 mouseDelta;

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
    [SerializeField] private float rollForce = 50f;

    private void Update()
    {
        if (move)
            MovementHandler();
    }

    private void MovementHandler()
    {
        Vector3 moveToPosition = new Vector3(aimingpointTransform.position.x, aimingpointTransform.position.y, 0);

        // This movement is too linear, it will move without damping;
        //Vector3 moveToPosition = transform.position + (Vector3)mouseDelta * movementMagnitude;

        // Fighter's movement;
        Vector3 targetMovePoint = Vector3.Lerp(transform.position, moveToPosition, Time.deltaTime * movementSpeed);
        transform.position = targetMovePoint;

        // This works too but if you change camera view, you have to change the limit numbers too.
        // So I changed to below clamp calculation that use camera's viewport instead;
        //moveToPosition.x = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        //moveToPosition.y = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);

        //------- Clamp position -------
        // With this clamp calculation, designer can set only 2 numbers, and it will works with every view point;
        Vector3 playerViewport = Camera.main.WorldToViewportPoint(transform.position);
        playerViewport.x = Mathf.Clamp(playerViewport.x, horizontalLimit.x, horizontalLimit.y);
        playerViewport.y = Mathf.Clamp(playerViewport.y, verticalLimit.x, verticalLimit.y);
        Vector3 playerWorldPosition = Camera.main.ViewportToWorldPoint(playerViewport);
        transform.position = playerWorldPosition;
        //------------------------------

        if (rotate)
            RotateHandler();
    }

    private void RotateHandler()
    {
        Vector3 direction = aimingpointTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Rotate z axis for better looking transition;
        //Vector3 movementVector = moveToPosition - transform.position;
        targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, -mouseDelta.x * rollForce);
        //---------------------------------------------

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
}
