using UnityEngine;

public class Missile_Damager : Entity_Damager
{
    private GameObject m_playerGO;
    private Vector3 m_currentAimingTargetPosition;

    [SerializeField] private float aimingDistanceMultiplier = 5.0f;
    [SerializeField] private float aimingRotationSpeed = 2.2f;
    [SerializeField] private float destroyPositionThreshold = 2.0f;

    public void ShootAt(GameObject playerGO)
    {
        m_playerGO = playerGO;
        //m_currentAimingTargetPosition = m_playerGO.transform.position;
    }

    protected override void Update()
    {
        CheckPosition();
        Turning();

        base.Update();
    }

    private void CheckPosition()
    {
        if (transform.position.z < m_playerGO.transform.position.z - destroyPositionThreshold)
            Destroy(gameObject);
    }

    private void Turning()
    {
        Vector3 vectorToPlayer = (m_playerGO.transform.position - transform.position).normalized;
        m_currentAimingTargetPosition = Vector3.Lerp(transform.forward, vectorToPlayer, Time.deltaTime * aimingRotationSpeed);

        transform.forward = m_currentAimingTargetPosition;
    }

    private Vector3 GetAimingPosition()
    {
        Vector3 aimingPosition = m_playerGO.transform.position + Vector3.forward * aimingDistanceMultiplier;
        return aimingPosition;
    }
}
