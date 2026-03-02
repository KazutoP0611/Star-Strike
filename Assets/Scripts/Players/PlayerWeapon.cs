using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private bool isFiring = false;

    private void Update()
    {
        FiringHandler();
    }

    private void FiringHandler()
    {
        if (isFiring == false)
            return;

        Debug.LogWarning("Fire!");
    }

    public void OnFire(InputValue value)
    {
        isFiring = value.isPressed;
    }
}
