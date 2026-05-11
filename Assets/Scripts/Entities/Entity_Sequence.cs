using UnityEngine;

public class Entity_Sequence : MonoBehaviour
{
    public delegate void OnSequenceEnter(Entity_Sequence sequence);
    private OnSequenceEnter onSequenceEnter;

    [SerializeField] private string triggerTag = "Player";

    public void Init(OnSequenceEnter onSequenceEnterCallback)
    {
        onSequenceEnter = onSequenceEnterCallback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
            SequenceTrigger();
    }

    public virtual void SequenceTrigger()
    {
        onSequenceEnter?.Invoke(this);
        Debug.LogWarning($"Player has entered {gameObject.name}");
    }

    public virtual void EndSequence()
    {
        Debug.LogWarning($"{gameObject.name} sequence has ended");
    }
}
