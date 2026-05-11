using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    private Entity_Sequence[] sequences;
    private Entity_Sequence currentSequence;

    private void Start()
    {
        sequences = GetComponentsInChildren<Entity_Sequence>();

        foreach (var seq in sequences)
        {
            seq.Init(OnSequenceEnter);
        }
    }

    private void OnSequenceEnter(Entity_Sequence sequence)
    {
        if (currentSequence != null)
            currentSequence.EndSequence();

        currentSequence = sequence;
    }
}
