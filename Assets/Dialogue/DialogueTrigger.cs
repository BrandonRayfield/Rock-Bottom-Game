using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public int NpcID;
    public Dialogue[] dialogue;
    public Dialogue[] dialogueDuringQuest;
    public Dialogue[] dialogueAfterQuest;
    public bool canTalk;
    public bool isTalking;
    public int questID;
    public bool isQuestGiver;
    private bool acceptedQuest;
    private bool finishedQuest;

    private int currentNpcID;

    private GameManager gameManager;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Update() {

        currentNpcID = FindObjectOfType<DialogueManager>().getCurrentNpcID();

        if (canTalk) {
            isTalking = FindObjectOfType<DialogueManager>().getIsTalking();
            acceptedQuest = FindObjectOfType<DialogueManager>().getAcceptedQuest();
            finishedQuest = gameManager.GetComponent<GameManager>().GetIsComplete(questID);
        }

        if(canTalk && isQuestGiver) {

        }


        if (canTalk && Input.GetKeyDown(KeyCode.E)) {

            FindObjectOfType<DialogueManager>().setIsQuestGiver(isQuestGiver);
            FindObjectOfType<DialogueManager>().setQuestID(questID);

            if (isQuestGiver) {
                
            }

            if(!isTalking) {
                if(isQuestGiver && acceptedQuest && !finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueDuringQuest);
                } else if (isQuestGiver && acceptedQuest && finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueAfterQuest);
                } else {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
            } else {
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
        }

        if (currentNpcID == NpcID && !canTalk) {
            FindObjectOfType<DialogueManager>().EndDialogue();
            isTalking = false;
        }

    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            FindObjectOfType<DialogueManager>().setCurrentNpcID(NpcID);
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            canTalk = false;
        }
    }
}
