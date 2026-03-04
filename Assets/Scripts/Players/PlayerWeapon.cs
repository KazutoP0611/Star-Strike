using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    private bool isFiring = false;
    private Vector2 mousePosition;

    [SerializeField] private ParticleSystem[] laserParticles;
    [SerializeField] private RectTransform crosshairRectTransform;
    [SerializeField] private Transform aimingPointTransform;
    [SerializeField] private float aiminDistance;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        AimingPointHandler();
        CrosshairMovementHandler();
    }

    private void CrosshairMovementHandler()
    {
        crosshairRectTransform.position = mousePosition;
        //crosshairRectTransform.position += new Vector3(mousePostion.x, mousePostion.y) * 100.0f * Time.deltaTime;
    }

    private void FiringHandler(bool fire)
    {
        foreach (var particle in laserParticles)
        {
            var laserEmission = particle.emission;
            laserEmission.enabled = fire;
        }
    }

    private void AimingPointHandler()
    {
        Vector3 aimingPosition = new Vector3(mousePosition.x, mousePosition.y, aiminDistance);
        aimingPointTransform.position = Camera.main.ScreenToWorldPoint(aimingPosition);
    }

    public void OnMouseMove(InputValue value) => mousePosition = value.Get<Vector2>();

    //public void OnMove(InputValue value) => CrosshairMovementHandler(value.Get<Vector2>());

    // Get "Fire" input from InputAction
    public void OnFire(InputValue value) => FiringHandler(value.isPressed);
}
