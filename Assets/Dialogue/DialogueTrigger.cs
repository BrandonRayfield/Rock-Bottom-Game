using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue[] dialogue;
    public bool canTalk;
    public bool isTalking;

    public void Update() {

        isTalking = FindObjectOfType<DialogueManager>().getIsTalking();

        if (canTalk && Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("Talking");

            if(!isTalking) {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            } else {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
        }

        if(!canTalk) {
            FindObjectOfType<DialogueManager>().EndDialogue();
            isTalking = false;
        }

    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            canTalk = false;
        }
    }
}
