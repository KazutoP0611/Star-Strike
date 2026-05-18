using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float currentSpeed = 1.0f;
    private float originalSpeed;

    [SerializeField] private GameObject level;

    [Header("Speed Details")]
    [SerializeField] private float boostSpeed = 3.0f;
    [SerializeField] private float brakeSpeed = 0.5f;

    private void Start()
    {
        originalSpeed = currentSpeed;
    }

    private void Update()
    {
        level.transform.localPosition -= new Vector3(0, 0, Time.deltaTime * currentSpeed);
    }

    public void SpeedChange(bool speedUp) => currentSpeed = speedUp ? boostSpeed : brakeSpeed;

    public void ResetSpeed() => currentSpeed = originalSpeed;
}
