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

    private void Update()
    {
        //Debug.Log(movement);
        //Debug.Log(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical"));
    }

    private void StartBarrelRoll()
    {
        
    }

    #region PlayerInput
    public void OnMove(InputValue value) => movement = value.Get<Vector2>();

    public void OnBoost(InputValue value) => boost = value.isPressed;

    public void OnRoll(InputValue value) => StartBarrelRoll();
    #endregion
}
