using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private bool isBarrelRolling;
    private float barrelDirection; // -1 left, 1 right

    private Vector2 movement;
    private Vector2 mouseDelta;
    private bool boost;

    [Header("Movement Details")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveLength = 0.1f;
    [SerializeField] private Vector2 limitHorizontal;
    [SerializeField] private Vector2 limitVertical;

    [Header("Rotation Details")]
    [SerializeField] private float maxRoll = 20f;
    [SerializeField] private float rotateSpeed = 20f;

    private void Update()
    {
        MovementHandler();
        RotateHandler();
    }

    private void MovementHandler()
    {
        Vector3 newPosition = transform.position + (new Vector3(mouseDelta.x, mouseDelta.y, 0) * moveLength * Time.deltaTime);

        newPosition.x = Mathf.Clamp(newPosition.x, limitHorizontal.x, limitHorizontal.y);
        newPosition.y = Mathf.Clamp(newPosition.y, limitVertical.x, limitVertical.y);

        //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
        transform.position = newPosition;
    }

    private void RotateHandler()
    {
        Vector3 targetEulerAngels = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -mouseDelta.x * maxRoll, Time.deltaTime * rotateSpeed));
    }

    private void StartBarrelRoll()
    {
        
    }

    #region PlayerInput
    public void OnMove(InputValue value) => mouseDelta = value.Get<Vector2>();

    public void OnMouseMove(InputValue value) => mouseDelta = value.Get<Vector2>();

    public void OnBoost(InputValue value) => boost = value.isPressed;

    public void OnRoll(InputValue value) => StartBarrelRoll();
    #endregion
}
