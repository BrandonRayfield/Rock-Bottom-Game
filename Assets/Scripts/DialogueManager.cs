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
    private bool isTalking;
    private bool finishedTalking;

    // Use this for initialization
    void Start() {
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

        animator.SetBool("IsOpen", true);
        //Debug.Log("Starting Coversation with " + dialogue.name);
        sentences.Clear();

        foreach (Dialogue sentence in dialogue) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (finishedTalking && sentences.Count == 0) {
            EndDialogue();
            return;
        }
        StopAllCoroutines();
        if(finishedTalking) {
            sentence = sentences.Dequeue();
            characterPortrait.sprite = sentence.characterPortrait;
            nameText.text = sentence.name;

            if(sentence.newFocus != null) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(sentence.newFocus);
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
    }

    public bool getIsTalking() {
        return isTalking;
    }

    public bool getFinishedTalking() {
        return finishedTalking;
    }

}