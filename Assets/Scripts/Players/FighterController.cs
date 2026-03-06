using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class FighterController : MonoBehaviour
{
    private Vector2 mouseDelta;
    private Vector3 targetMovePoint;

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

        //This movement is too linear, it will move without damping;
        //Vector3 moveToPosition = transform.position + (Vector3)mouseDelta * movementMagnitude;

        moveToPosition.x = Mathf.Clamp(moveToPosition.x, horizontalLimit.x, horizontalLimit.y);
        moveToPosition.y = Mathf.Clamp(moveToPosition.y, verticalLimit.x, verticalLimit.y);

        targetMovePoint = Vector3.Lerp(transform.position, moveToPosition, Time.deltaTime * movementSpeed);
        transform.position = targetMovePoint;

        if (rotate)
            RotateHandler(moveToPosition);
    }

    private void RotateHandler(Vector3 moveToPosition)
    {
        Vector3 direction = aimingpointTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        Vector3 movementVector = moveToPosition - transform.position;
        targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, -movementVector.x * rollForce);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();
}
