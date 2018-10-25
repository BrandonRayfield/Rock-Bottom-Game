using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour {

    // Dialogue Variables
    [Header("Dialogue Variables")]
    public int NpcID;
    public Dialogue[] dialogue;
    public Dialogue[] dialogueDuringQuest;
    public Dialogue[] dialogueAfterQuest;
    public bool canTalk;
    public bool isTalking;
    public int questID;
    public bool isQuestGiver;

    // Completed Quest Objects
    [Header("Reward Variables")]
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
    private GameObject playerObject;

    // Autotrigger Variables
    [Header("Autotrigger Variables")]
    public bool isAutomatic;
    public bool isStageTrigger;
    public bool isStageDialogue;
    private bool hasTriggered;
    private bool autoComplete;

    // Boss Variables
    [Header("Boss Variables")]
    public bool isBossCutscene;
    public GameObject bossObject;
    private bool bossActivate;
    private bool bossComplete;

    //UI Elements
    [Header("UI Variables")]
    public Text interactText;
    public string interactMessage = "Press 'e' to talk";

    // Expression Variables
    public GameObject questionObject;
    public GameObject updateObject;

    private GameObject spawnedQuestionObject;
    private GameObject spawnedUpdateObject;

    private bool hasUpdatedExpression;
    private bool questExpression;
    private int expressionType;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (isProgressor) {
            progressObjectAnimator = progressObject.GetComponent<Animator>();
        }

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        // Spawn expression Icons so we don't have to go through and update all the NPC's
        if(!isAutomatic) {
            Vector3 spawnLocation = new Vector3(transform.position.x + 0.03f, transform.position.y + rewardHeightPosition + 0.15f, transform.position.z);
            spawnedQuestionObject = Instantiate(questionObject, spawnLocation, transform.rotation);
            Vector3 spawnLocation2 = new Vector3(transform.position.x + 0.03f, transform.position.y + rewardHeightPosition - 0.15f, transform.position.z);
            spawnedUpdateObject = Instantiate(updateObject, spawnLocation, transform.rotation);
            updateExpression(0);
        }

        // Show the correct expression depending on the dialogue type
        if(!isAutomatic && !isQuestGiver) {
            updateExpression(2); // Set Exclamation Mark active if lyric NPC or Sign
        } else if (!isAutomatic && isQuestGiver) {
            updateExpression(1); // Set Question Mark active if quest NPC
        } else if (!isAutomatic) {
            updateExpression(0); // Disable Both (Because Auto-trigger)
        }

    }

    public void Update() {

        currentNpcID = FindObjectOfType<DialogueManager>().getCurrentNpcID();
        autoComplete = FindObjectOfType<DialogueManager>().getIsAutoComplete();
        bossComplete = FindObjectOfType<DialogueManager>().getIsBossComplete();

        if(isQuestGiver && !hasUpdatedExpression) {
            questExpression = GameManager.instance.GetIsComplete(questID);
            if(questExpression) {
                updateExpression(2);
                hasUpdatedExpression = true;
            }
        }

        if (canTalk) {
            isTalking = FindObjectOfType<DialogueManager>().getIsTalking();
        }

        if(canTalk && isQuestGiver) {
            acceptedQuest = gameManager.GetComponent<GameManager>().GetHasAccepted(questID);
            finishedQuest = gameManager.GetComponent<GameManager>().GetIsComplete(questID);
        }

        if (!isAutomatic && canTalk && Input.GetKeyDown(KeyCode.E)) {
            gameManager.isTalking = true;
            playerObject.GetComponent<Animator>().Play("Idle");
            StartTalking();
        }

        // Used to continue to next sentence, without allowing player to repeat dialogue after completed
        if(isAutomatic && !autoComplete && canTalk && Input.GetKeyDown(KeyCode.E)) {
            if (!isStageDialogue) {
                gameManager.isTalking = true;
                playerObject.GetComponent<Animator>().Play("Idle");
                StartTalking();
            } else {
                StartTalking();
            }
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

        if(isAutomatic && isStageDialogue) {
            interactText.enabled = false;
        }

        //Remove Update expression once spoken to
        if(!isAutomatic) {
            interactText.enabled = false;
            updateExpression(0);
        }

        //Update variables from Dialogue Manager
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

            if (isAutomatic && !hasTriggered&& !isStageDialogue) {
                gameManager.isTalking = true;
                playerObject.GetComponent<Animator>().Play("Idle");
                StartTalking();
                hasTriggered = true;
            }
            else if (isAutomatic && !hasTriggered && isStageDialogue) {
                StartTalking();
                hasTriggered = true;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if(!isAutomatic && other.gameObject.tag == "Player") {
            if(!isTalking) {
                interactText.enabled = true;
                interactText.text = interactMessage;
            } else {
                interactText.enabled = false;
                interactText.text = "";
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!isAutomatic && other.gameObject.tag == "Player") {
            canTalk = false;
            interactText.enabled = false;
            interactText.text = "";
        }
    }

    private void updateExpression(int expressionType) {
        if (expressionType == 1) {              //Question Mark is Active
            spawnedQuestionObject.SetActive(true);
            spawnedUpdateObject.SetActive(false);
        } else if (expressionType == 2) {       //Exclamation is Active
            spawnedQuestionObject.SetActive(false);
            spawnedUpdateObject.SetActive(true);
        } else {                                //None are Active
            spawnedQuestionObject.SetActive(false);
            spawnedUpdateObject.SetActive(false);
        }
    }

    public void newExpression(int questID) {
        if(NpcID == questID && !isAutomatic) {
            updateExpression(2);
        }
    }

}
