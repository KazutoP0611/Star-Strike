using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    private Coroutine textTypingCoroutine;
    private string fullTextToShow;

    [Header("Display Details")]
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typing Details")]
    [SerializeField] private float typingSpeedDelay = 0.01f;

    public void PlayDialogueLine(DialogueLineSO line)
    {
        speakerPortrait.sprite = line.speaker.speakerPortrait;
        speakerName.text = line.speaker.speakerName;

        StartTypeTextCoroutine(line);
    }
    private void StartTypeTextCoroutine(DialogueLineSO line)
    {
        if (textTypingCoroutine != null)
        {
            FinishDialogueTextRightNow();
            textTypingCoroutine = null;

            return;
            //StopCoroutine(textTypingCoroutine);
        }

        fullTextToShow = line.GetRandomLine();
        textTypingCoroutine = StartCoroutine(TypeTextCo(fullTextToShow));
    }

    public void FinishDialogueTextRightNow()
    {
        StopCoroutine(textTypingCoroutine);
        dialogueText.text = fullTextToShow;
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(typingSpeedDelay);
        }
    }
}
