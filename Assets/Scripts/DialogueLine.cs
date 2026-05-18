using TMPro;
using UnityEngine;

public class DialogueLine : MonoBehaviour
{
    private int currentLine = 0;

    [SerializeField] private string[] textLines;
    [SerializeField] TextMeshProUGUI dialogueText;

    public void NextDialogueLine()
    {
        currentLine++;

        dialogueText.text = textLines[currentLine];
    }
}
