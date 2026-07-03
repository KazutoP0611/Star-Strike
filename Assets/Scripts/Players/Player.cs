using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Player : MonoBehaviour
{
    public static Player instance { get; private set; }
    public static event Action OnDead; // The "event" keyword restricts external classes from forces-firing or clearing it.

    private PlayerInput m_inputSet;

    #region Player Components
    private Player_Weapon m_playerWeapon;
    private PlayerBoost m_playerBoost;
    #endregion

    //public Vector2 mouseDelta { get; private set; }
    public bool IsDead { get; private set; }

    [Header("Immortal Details")]
    public float immortalTime = 2.0f;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        IsDead = false;
        m_inputSet = GetComponent<PlayerInput>();

        m_playerWeapon = GetComponent<Player_Weapon>();
        m_playerBoost = GetComponent<PlayerBoost>();
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

    public void EnablePlayerInput(bool enableInput)
    {
        m_inputSet.enabled = enableInput;
    }

    public void ForceStopActions()
    {
        m_playerWeapon.ForceShutdownFiring();
        m_playerBoost.StartBoostCooldown();
    }
}
