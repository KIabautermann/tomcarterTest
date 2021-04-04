using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class NpcDialogue : MonoBehaviour
{
    private const string NEXT_LINE_ENDING = "...";

    [SerializeField]
    private DialogueRepository.NpcNames npcNames;

    [SerializeField]
    private TextMeshProUGUI dialogueDisplay;
    private List<string> dialogueLines;
    private int currentLineIndex = 0;

    void Start()
    {
        dialogueLines = DialogueRepository.GetDialogueLines(npcNames).ToList();
    }

    public bool OutputNextLine()
    {
        bool lastLine = currentLineIndex + 1 == dialogueLines.Count;
        dialogueDisplay.text = GetNextLine();
        if (!lastLine) { dialogueDisplay.text += NEXT_LINE_ENDING; }
        return lastLine;
    }

    public void ExitDialoge()
    {
        dialogueDisplay.text = "";
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
