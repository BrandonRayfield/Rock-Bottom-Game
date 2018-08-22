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

    // Completed Quest Objects
    public bool isRewardGiver;
    public GameObject rewardObject;
    public float rewardHeightPosition = 1.0f;
    public bool isProgressor;
    public GameObject progressObject;
    private Animator progressObjectAnimator;


    private bool acceptedQuest;
    private bool finishedQuest;

    private int currentNpcID;

    private GameManager gameManager;
    private GameObject cameraObject;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        progressObjectAnimator = progressObject.GetComponent<Animator>();

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

    }

    public void Update() {

        currentNpcID = FindObjectOfType<DialogueManager>().getCurrentNpcID();

        if (canTalk) {
            isTalking = FindObjectOfType<DialogueManager>().getIsTalking();

        }

        if(canTalk && isQuestGiver) {
            acceptedQuest = gameManager.GetComponent<GameManager>().GetHasAccepted(questID);
            finishedQuest = gameManager.GetComponent<GameManager>().GetIsComplete(questID);
        }


        if (canTalk && Input.GetKeyDown(KeyCode.E)) {

            FindObjectOfType<DialogueManager>().setIsQuestGiver(isQuestGiver);
            FindObjectOfType<DialogueManager>().setQuestID(questID);

            if(!isTalking) {
                if(isQuestGiver && acceptedQuest && !finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueDuringQuest);
                } else if (isQuestGiver && acceptedQuest && finishedQuest) {
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogueAfterQuest);

                    // If the NPC is supposed to provide the player with a reward for completing the quest, spawn reward item
                    if(isRewardGiver) {
                        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
                        Instantiate(rewardObject, spawnLocation, transform.rotation);
                    }

                    // If the NPC is supposed to unlock the next area for the player, open object
                    if (isProgressor) {
                        progressObjectAnimator.Play("doorOpening");
                        cameraObject.GetComponent<CameraScript>().SetFocusPoint(progressObject);


                    }

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
