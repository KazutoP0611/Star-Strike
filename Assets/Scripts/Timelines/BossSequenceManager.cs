using UnityEngine;
using UnityEngine.Playables;

public class BossSequenceManager : MonoBehaviour
{
    private BossNexus bossNexus;

    [Header("General Details")]
    [SerializeField] private GameObject boss;

    [Header("Playable Director Details")]
    [SerializeField] private PlayableDirector bossAppearanceTimeline;
    [SerializeField] private PlayableDirector bossDeadTimeline;

    private void Awake()
    {
        bossNexus = boss.GetComponent<BossNexus>();
    }

    [ContextMenu("Start Boss")]
    public void StartSequence()
    {
        SetActiveBoss(true);
        bossAppearanceTimeline.Play();
    }

    public void OnSequenceStart()
    {
        bossNexus.Initialize(StartBossDieSequence);

        UI_Manager.instance.SetActiveUIIndicators(false);
        Player.Instance.EnablePlayerInput(false);
    }

    public void OnSequenceEnd()
    {
        UI_Manager.instance.SetActiveUIIndicators(true);
        Player.Instance.EnablePlayerInput(true);
    }

    public void OnGameOver()
    {
        Player.Instance.EnablePlayerInput(false);
        UI_Manager.instance.SetActiveGameOverScreen(true);
    }

    public void StartBossDieSequence()
    {
        bossDeadTimeline.Play();

        //OnSequenceEnd();
    }

    public void SetActiveBoss(bool active) => boss.SetActive(active);
}
