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
    private GameObject Player;

    [SerializeField]
    private DialogueRepository.NpcNames npcNames;

    [SerializeField]
    private ObjectPooler dialoguePopupPooler;

    [SerializeField]
    private float playerDetectionDistance = 5f;

    [SerializeField]
    private ObjectPooler hoverTextPooler;

    private HoverInteractable hoverInteractable;
    private PopupDialogue popupDialogue;
    private List<string> dialogueLines;
    private int currentLineIndex = 0;
    private Collider interactableObjectCollider;

    void Start()
    {
        dialogueLines = DialogueRepository.GetDialogueLines(npcNames).ToList();
        interactableObjectCollider = GetComponent<BoxCollider>();
        StartCoroutine(CheckForPlayerProximity());
    }
    
    private IEnumerator CheckForPlayerProximity()
    {
        while (true) {
            float distanceToPlayer = Vector3.Distance(Player.transform.position, this.gameObject.transform.position);

            float secondsUntilNextCheck = (distanceToPlayer - playerDetectionDistance) / (playerDetectionDistance * 5);
            secondsUntilNextCheck = Mathf.Max(0.1f, secondsUntilNextCheck);
            secondsUntilNextCheck = Mathf.Min(30f, secondsUntilNextCheck);
            yield return new WaitForSeconds(secondsUntilNextCheck);

            if (distanceToPlayer <= playerDetectionDistance) {
                ShowHoverText();
            } else if (distanceToPlayer > playerDetectionDistance) {
                HideHoverText();
            }
        }
    }

    private void ShowHoverText()
    {
        if (hoverInteractable == null) {
            hoverTextPooler.GetItem(Vector3.zero, Quaternion.identity).GetInstance(typeof(HoverInteractable), out MonoBehaviour tmp);
            hoverInteractable = tmp as HoverInteractable;
            hoverInteractable.PoolCollected = () => hoverInteractable = null;
            hoverInteractable.LogicStart(interactableObjectCollider.bounds.max);
        }
    }
    private void HideHoverText()
    {
        if (hoverInteractable != null) {
            hoverInteractable.Dispose();
        }
    }
    public void Initialize()
    {
        if (popupDialogue == null) {
            dialoguePopupPooler.GetItem(gameObject.transform.position, Quaternion.identity).GetInstance(typeof(PopupDialogue), out MonoBehaviour tmp);
            popupDialogue = tmp as PopupDialogue;
            Vector3 textPos = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 1.5f, 0));
            popupDialogue.LogicStart(textPos);
            popupDialogue.PoolCollected = () => popupDialogue = null;
        }
    }
    // Esto devuelve un booleano que indica si el dialogo ha terminado. Por ahi es mas prolijo mover este check a otra funcion
    // auxiliar y que quien llama sea responsable de consultar eso
    public bool OutputNextLine()
    {
        // Fijarse si ya llegamos a la ultima linea de dialogo
        bool lastLine = currentLineIndex == dialogueLines.Count;
        // Si es el caso y el pop up no esta lockeado por estar escribiendo output, entonces el dialogo termino
        // el indice de las lineas se retrocede un paso para poder seguir respondiendo la ultima linea
        // que devolvio antes el personaje
        // Edit: por ahi estaria bueno hacer que sea dinamico/seralizable, si queremos npcs que loopeen entre
        // dialogos finales
        if (lastLine && !popupDialogue.IsLocked()) {
            currentLineIndex--;
            return true;
        }
        // Si el popup esta trabado, forza a que termine el output de texto
        else if (popupDialogue.IsLocked()) {
            popupDialogue.EndLine();
            return false;
        }
        // Cualquier otro caso, pasale al popup el texto que queremos mostrar 
        else {
            string text = GetNextLine();
            if (!lastLine) { text += NEXT_LINE_ENDING; }
            Vector3 textPos = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 1.5f, 0));
            // Que pasa si alguien le saca el objeto antes de que termine la conversacion? No deberia ser posible, perooo
            popupDialogue.Display(text, textPos);
            return false;
        }
    }

    public void ExitDialoge()
    {
        popupDialogue.LogicEnd();
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
        currentLineIndex = Math.Min(currentLineIndex + 1, dialogueLines.Count);
        return lineIndex;
    }

    
    
}
