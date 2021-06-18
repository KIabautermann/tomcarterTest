using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupDialogue : PoolableObject
{
    private TextMeshProUGUI _dialogueText;
    
    // borra giladas como referencias a otras cosas
    public override void ResetSceneReferences()
    {
        _dialogueText.text = "";   
        _dialogueText = null;
    }

    public void LogicStart(TextMeshProUGUI dialogueText)
    {
        _dialogueText = dialogueText;   
    }

    public void Display(string text)
    {
        _dialogueText.text = text;
    }

}
