using UnityEngine;

public class PlayerFlightControl : MonoBehaviour
{
    private Vector2 lastAimingPointPosition;
    private Vector3 moveVector;

    [SerializeField] private Transform aimingPoint;

    [Header("Movement Details")]
    [SerializeField] private float movementMultiplier = 0.5f;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private Vector2 limitHorizontal;
    [SerializeField] private Vector2 limitVertical;

    [Header("Rotation Details")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxRoll = 45.0f;

    void Update()
    {
        HandleMovement();
        RotationHandler();
    }

    void HandleMovement()
    {
        Vector2 moveDirection = (Vector2)aimingPoint.position - lastAimingPointPosition;
        //Vector2 normalizedMoveDirection = moveDirection.normalized;

        Vector3 targetPosition = transform.position + ((Vector3)moveDirection * movementMultiplier);
        targetPosition.x = Mathf.Clamp(targetPosition.x, limitHorizontal.x, limitHorizontal.y);
        targetPosition.y = Mathf.Clamp(targetPosition.y, limitVertical.x, limitVertical.y);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    void RotationHandler()
    {
        moveVector = aimingPoint.position - transform.position;
        //Vector3 normalizedLookAt = moveVector.normalized;

        Quaternion lookAtRotation = Quaternion.LookRotation(moveVector, Vector3.up);
        Vector3 flightRotation = lookAtRotation.eulerAngles;
        flightRotation.z = Mathf.Clamp(-moveVector.x * 10.0f, - maxRoll, maxRoll);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(flightRotation), Time.deltaTime * rotationSpeed);

        lastAimingPointPosition = aimingPoint.position;
    }
}