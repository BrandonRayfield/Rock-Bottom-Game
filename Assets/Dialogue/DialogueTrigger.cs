using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour {

    // Dialogue Variables
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
    public float focusTime = 4.0f;
    private Animator progressObjectAnimator;


    private bool acceptedQuest;
    private bool finishedQuest;
    private bool isUnlocked;
    private bool isRewarded;

    private int currentNpcID;

    private GameManager gameManager;
    private GameObject cameraObject;

    // Autotrigger Variables
    public bool isAutomatic;
    private bool hasTriggered;
    private bool autoComplete;

    // Boss Variables
    public bool isBossCutscene;
    public GameObject bossObject;
    private bool bossActivate;
    private bool bossComplete;

    //UI Elements
    public Text interactText;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();

        if(isProgressor) {
            progressObjectAnimator = progressObject.GetComponent<Animator>();
        }

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

    }

    public void Update() {

        currentNpcID = FindObjectOfType<DialogueManager>().getCurrentNpcID();
        autoComplete = FindObjectOfType<DialogueManager>().getIsAutoComplete();
        bossComplete = FindObjectOfType<DialogueManager>().getIsBossComplete();

        if (canTalk) {
            isTalking = FindObjectOfType<DialogueManager>().getIsTalking();
        }

        if(canTalk && isQuestGiver) {
            acceptedQuest = gameManager.GetComponent<GameManager>().GetHasAccepted(questID);
            finishedQuest = gameManager.GetComponent<GameManager>().GetIsComplete(questID);
        }

        if (!isAutomatic && canTalk && Input.GetKeyDown(KeyCode.E)) {
            StartTalking();
        }

        // Used to continue to next sentence, without allowing player to repeat dialogue after completed
        if(isAutomatic && !autoComplete && canTalk && Input.GetKeyDown(KeyCode.E)) {
            StartTalking();
        }

        if (currentNpcID == NpcID && !canTalk) {
            FindObjectOfType<DialogueManager>().EndDialogue();
            isTalking = false;
        }

        if(isBossCutscene && bossComplete && !bossActivate) {
            bossObject.GetComponent<Boss>().setIsEngaged(true);
            bossActivate = true;
        }

    }

    public void TriggerDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    // Code for triggering the dialogue interaction
    private void StartTalking() {
        interactText.enabled = false;

        FindObjectOfType<DialogueManager>().setIsQuestGiver(isQuestGiver);
        FindObjectOfType<DialogueManager>().setQuestID(questID);

        if (!isTalking) {
            if (isQuestGiver && acceptedQuest && !finishedQuest) {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueDuringQuest);
            } else if (isQuestGiver && acceptedQuest && finishedQuest) {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogueAfterQuest);

                // If the NPC is supposed to provide the player with a reward for completing the quest, spawn reward item
                if (isRewardGiver && !isRewarded) {
                    Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + rewardHeightPosition, transform.position.z);
                    Instantiate(rewardObject, spawnLocation, transform.rotation);
                    isRewarded = true;
                }

                // If the NPC is supposed to unlock the next area for the player, open object
                if (isProgressor && !isUnlocked) {
                    progressObjectAnimator.Play("doorOpening");
                    cameraObject.GetComponent<CameraScript>().SetFocusPoint(progressObject, focusTime);
                    isUnlocked = true;
                }

            } else {
                FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            }
        } else {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            FindObjectOfType<DialogueManager>().setCurrentNpcID(NpcID);
            FindObjectOfType<DialogueManager>().setIsAutomatic(isAutomatic);
            FindObjectOfType<DialogueManager>().setIsBoss(isBossCutscene);
            canTalk = true;

            if (isAutomatic && !hasTriggered) {
                StartTalking();
                hasTriggered = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(!isAutomatic && other.gameObject.tag == "Player") {
            if(!isTalking) {
                interactText.enabled = true;
                interactText.text = "Press 'E' to talk!";
            } else {
                interactText.enabled = false;
                interactText.text = "";
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            canTalk = false;
            interactText.enabled = false;
            interactText.text = "";
        }
    }
}
