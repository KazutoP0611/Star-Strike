using UnityEngine;

public class BossNexus_Missile : MonoBehaviour
{
    private GameObject m_playerGO;
    private float m_lastShootTime;
    private bool m_isActivated = false;

    [SerializeField] private float shootMissleDotThreshold = 0.9f;
    [SerializeField] private float shootDelay = 2.0f;

    public void Initialize(GameObject playerGO)
    {
        m_playerGO = playerGO;
    }

    private void Update()
    {
        if (m_isActivated == false)
            return;

        if (Time.time - shootDelay < m_lastShootTime)
            return;

        CheckShootMissile();
    }

    private void CheckShootMissile()
    {
        Vector3 vectorToPlayer = m_playerGO.transform.position - transform.position;
        float dotValue = Vector3.Dot(transform.forward, vectorToPlayer.normalized);

        if (dotValue > shootMissleDotThreshold)
        {
            Debug.LogWarning($"Shoot Missile! {gameObject} {dotValue}");
            m_lastShootTime = Time.time;
        }
    }

    public void SetActivateMissile(bool activate) => m_isActivated = activate;
}
