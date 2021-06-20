using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class NpcDialogue : MonoBehaviour
{
    private const string NEXT_LINE_ENDING = "...";

    [SerializeField]
    private DialogueRepository.NpcNames npcNames;

    [SerializeField]
    private TextMeshProUGUI dialogueDisplay;
    [SerializeField]
    private ObjectPooler dialoguePopupPooler;
    private PopupDialogue popupDialogue;
    private List<string> dialogueLines;
    private int currentLineIndex = 0;

    void Start()
    {
        dialogueLines = DialogueRepository.GetDialogueLines(npcNames).ToList();
    }

    public void Initialize()
    {
        if (popupDialogue == null) {
            popupDialogue = dialoguePopupPooler.GetItem(gameObject.transform.position, Quaternion.identity).GetComponent<PopupDialogue>();
            popupDialogue.LogicStart(dialogueDisplay);
            popupDialogue.PoolCollected = () => popupDialogue = null;
        }
    }
    public bool OutputNextLine()
    {
        bool lastLine = currentLineIndex + 1 == dialogueLines.Count;
        string text = GetNextLine();
        if (!lastLine) { text += NEXT_LINE_ENDING; }

        // Que pasa si alguien le saca el objeto antes de que termine la conversacion? No deberia ser posible, perooo
        popupDialogue.Display(text);
        return lastLine;
    }

    public void ExitDialoge()
    {
        popupDialogue.Display("");
    }

    private string GetNextLine() 
    {
        return dialogueLines[GetAndIncNextIndex()];
    }

    public void ResetLines()
    {
        currentLineIndex = 0;
    }
    private int GetAndIncNextIndex()
    {
        int lineIndex = currentLineIndex;
        currentLineIndex = Math.Min(currentLineIndex + 1, dialogueLines.Count - 1);
        return lineIndex;
    }

    
    
}
