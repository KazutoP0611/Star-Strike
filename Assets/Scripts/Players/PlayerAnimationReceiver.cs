using UnityEngine;

public class PlayerAnimationReceiver : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void StopRolling() => playerMovement.StopRolling();
}
