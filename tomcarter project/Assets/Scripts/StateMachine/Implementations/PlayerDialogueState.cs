using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueState : PlayerInteractionState
{
    private NpcDialogue npcDialogueComponent;
    private bool requestNextLine;
    private bool finishedLines;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dialogueTrigger;
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();

        finishedLines = false;
        requestNextLine = true;
        _target.removeSubState();

        if(Physics.Raycast(_target.transform.position, direction, out RaycastHit rayCastHit, stats.npcDetectionLenght, stats.npc))
        {
            Component npcDialogue = rayCastHit.transform.gameObject.GetComponent<NpcDialogue>();
            if (npcDialogue == null) 
            {
                Debug.LogWarning("Npc game object does not have an NpcDialogue component. Exiting state");
                interactionFinished = true;   
            } 
            else
            {
                npcDialogueComponent = npcDialogue as NpcDialogue;
            }
        }
        else
        {
            Debug.LogWarning("Player is in Dialogue State without an NPC in front of it");
            interactionFinished = true;
        }
    }

    protected override void DoLogicUpdate()
    {
        if (requestNextLine)
        {
            finishedLines = npcDialogueComponent.OutputNextLine();
            requestNextLine = false;
        }
        else if (inputs.InteractionInput)
        {
            inputs.UsedInteraction();
            if (finishedLines) { interactionFinished = true; }
            else { requestNextLine = true; }
        }

        base.DoLogicUpdate();
    }

    protected override void DoTransitionOut()
    {
        finishedLines = false;
        requestNextLine = false;
        npcDialogueComponent.ExitDialoge();
        npcDialogueComponent = null;
        base.DoTransitionOut();
    }
}
