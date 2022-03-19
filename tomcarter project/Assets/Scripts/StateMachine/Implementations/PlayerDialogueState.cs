using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogueState : PlayerInteractionState
{
    private NpcDialogue npcDialogueComponent;

    public override void Init(PlayerStateMachine target)
    {
        base.Init(target);
        animationTrigger = stats.dialogueID;
    }

    protected override void DoTransitionIn()
    {
        base.DoTransitionIn();


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
                npcDialogueComponent.Initialize();
                npcDialogueComponent.OutputNextLine();
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
        if (inputs.InteractionInput)
        {
            inputs.UsedInteraction();
            if (npcDialogueComponent.OutputNextLine()) { interactionFinished = true; }
        }

        base.DoLogicUpdate();
    }

    protected override void DoTransitionOut()
    {
        npcDialogueComponent.ExitDialoge();
        npcDialogueComponent = null;
        base.DoTransitionOut();
    }
}
