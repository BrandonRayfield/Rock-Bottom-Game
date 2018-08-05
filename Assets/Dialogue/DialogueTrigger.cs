using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue[] dialogue;
    public Dialogue[] dialogueDuringQuest;
    public Dialogue[] dialogueAfterQuest;
    public bool canTalk;
    public bool isTalking;
    public int questID;
    public bool isQuestGiver;
    private bool acceptedQuest;
    private bool finishedQuest;

    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Update() {

        isTalking = FindObjectOfType<DialogueManager>().getIsTalking();
        acceptedQuest = FindObjectOfType<DialogueManager>().getAcceptedQuest();
        finishedQuest = gameManager.GetComponent<GameManager>().GetIsComplete(questID);

        if (canTalk && Input.GetKeyDown(KeyCode.E)) {

            FindObjectOfType<DialogueManager>().setIsQuestGiver(isQuestGiver);
            FindObjectOfType<DialogueManager>().setQuestID(questID);

            Debug.Log("Talking");

            if(!isTalking) {
                if(acceptedQuest && !finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueDuringQuest);
                } else if (acceptedQuest && finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueAfterQuest);
                } else {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
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
