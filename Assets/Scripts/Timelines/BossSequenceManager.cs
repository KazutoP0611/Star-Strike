using UnityEngine;
using UnityEngine.Playables;

public class BossSequenceManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector bossAppearanceTimeline;
    [SerializeField] private GameObject boss;

    [ContextMenu("Play Boss Appear")]
    public void PlayBossAppear()
    {
        bossAppearanceTimeline.Play();
    }

    public void OnBossAppeared()
    {
        Debug.LogWarning("Boss appear ended");
    }

    public void SetActiveBoss(bool active) => boss.SetActive(active);
}
