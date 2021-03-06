﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<Dialogue> sentences;
    public Text nameText;
    public Text dialogueText;
    public Image characterPortrait;
    public Animator animator;
    public GameObject cameraObject;

    private Dialogue sentence;
    public bool isTalking;
    public bool finishedTalking;

    private int questID;
    private bool isQuestGiver;
    private bool acceptedQuest;
    private bool finishedQuest;

    private int currentNpcID;

    private GameObject playerObject;

    // Automatic Trigger Variables
    private bool isAutomatic;
    private bool autoComplete;

    // Boss Cutscene Variables
    private bool isBossScene;
    private bool bossComplete;

    // GameManager Object for quests
    private GameManager gameManager;

    // Sound Variable
    public GameObject[] dialogueOpenSound;

    // Use this for initialization
    void Start() {

        gameManager = FindObjectOfType<GameManager>();

        sentences = new Queue<Dialogue>();
        finishedTalking = true;
        try {
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        } catch {
            cameraObject = null;
        }
    }

    public void StartDialogue(Dialogue[] dialogue) {

        isTalking = true;

        if (isAutomatic) {
            autoComplete = false;
        }

        if (isBossScene) {
            bossComplete = false;
        }

        animator.SetBool("IsOpen", true);
        Instantiate(dialogueOpenSound[Random.Range(0,dialogueOpenSound.Length)], transform.position, transform.rotation);
        //Debug.Log("Starting Coversation with " + dialogue.name);
        sentences.Clear();

        foreach (Dialogue sentence in dialogue) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (finishedTalking && sentences.Count == 0) {

            if (isAutomatic) {
                autoComplete = true;
            }

            if (isBossScene) {
                bossComplete = true;
            }

            EndDialogue();
            return;
        }
        StopAllCoroutines();
        if(finishedTalking) {
            sentence = sentences.Dequeue();
            characterPortrait.sprite = sentence.characterPortrait;
            nameText.text = sentence.name;

            if(sentence.newFocus != null) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(sentence.newFocus, sentence.focusTime);
            }

            StartCoroutine(TypeSentence(sentence.sentences));
        } else {
            skipTextAnimation(sentence.sentences);
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        finishedTalking = false;

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.02f);
        }

        finishedTalking = true;

    }

    public void skipTextAnimation(string sentence) {
        dialogueText.text = sentence;
        finishedTalking = true;
    }

    public void EndDialogue() {
        StopAllCoroutines();
        isTalking = false;
        finishedTalking = true;
        animator.SetBool("IsOpen", false);

        gameManager.isTalking = false;

        if(isQuestGiver) {
            gameManager.AcceptQuest(questID);
        }

    }

    public bool getIsTalking() {
        return isTalking;
    }

    public bool getFinishedTalking() {
        return finishedTalking;
    }

    public void setIsQuestGiver(bool isQuest) {
        isQuestGiver = isQuest;
    }

    public void setQuestID(int newQuestID) {
        questID = newQuestID;
    }

    public void setCurrentNpcID(int newID) {
        currentNpcID = newID;
    }

    public int getCurrentNpcID() {
        return currentNpcID;
    }

    //--------------------------------------------------------------------------------------
    // Automatic Functions

    public void setIsAutomatic(bool automatic) {
        isAutomatic = automatic;
    }

    public bool getIsAutoComplete() {
        return autoComplete;
    }

    //--------------------------------------------------------------------------------------
    // Boss Functions
    public void setIsBoss(bool boss) {
        isBossScene = boss;
    }

    public bool getIsBossComplete() {
        return bossComplete;
    }

}
