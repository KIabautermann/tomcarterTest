using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDialogConsumer : MonoBehaviour
{
    [SerializeField]
    public ObjectPooler Pooler;

    private PopupDialogue dialogue;

    public void OpenDialogue() 
    {
        if (dialogue == null) 
        {
            //dialogue = Pooler.
        }
    }
}
