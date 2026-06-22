using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public static event Action OnDead; // The "event" keyword restricts external classes from forces-firing or clearing it.

    //public Vector2 mouseDelta { get; private set; }
    public bool IsDead { get; private set; }

    //private StarStikeControl input;
    private PlayerInput inputSet;

    private void Awake()
    {
        //input = new StarStikeControl();
        IsDead = false;
        inputSet = GetComponent<PlayerInput>();
    }

    //private void OnEnable()
    //{
    //    input.Enable();

    //    input.Fighter.MouseMove.performed += value => mouseDelta = value.ReadValue<Vector2>();
    //    input.Fighter.MouseMove.canceled += value => mouseDelta = Vector2.zero;
    //}

    //private void OnDisable()
    //{
    //    input.Disable();
    //}

    public void PlayerStartDying()
    {
        //input.Disable();
        IsDead = true;
        inputSet.enabled = false;
        OnDead?.Invoke();

        UI_Manager.instance.SetActiveGameOverScreen(true);
    }
}
