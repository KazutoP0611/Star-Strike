using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public static event Action OnDead; // The "event" keyword restricts external classes from forces-firing or clearing it.

    //public Vector2 mouseDelta { get; private set; }
    public bool IsDead { get; private set; }

    [Header("Immortal Details")]
    public float immortalTime = 2.0f;

    //private StarStikeControl input;
    private PlayerInput inputSet;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        //input = new StarStikeControl();
        IsDead = false;
        inputSet = GetComponent<PlayerInput>();
    }

    //private void OnEnable()
    //{
    //    input.Enable();
    //}

    //private void OnDisable()
    //{
    //    input.Disable();
    //}

    public void PlayerStartDying()
    {
        //input.Disable();
        IsDead = true;

        EnablePlayerInput(false);
        OnDead?.Invoke();

        UI_Manager.instance.SetActiveGameOverScreen(true);
    }

    public void EnablePlayerInput(bool enableInput) => inputSet.enabled = enableInput;
}
