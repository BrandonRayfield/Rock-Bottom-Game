using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCScript : MonoBehaviour {

    //Game Objects
    public GameObject playerObject;
    public GameObject cameraObject;
    public GameObject keyObject;

    //NPC Object and animator
    public GameObject NPCObject;
    Animator animator;

    //Passageway Object
    public GameObject passageWay;
    Animator passageAnimator;

    //Time Variables
    private float time;
    private float waitTime = 0.5f;

    //UI Variables
    public GameObject goldKeyUI;

    //Sound Variable
    public GameObject keySound;

    //Dialog Text Objects
    public Text interactText;
    public GameObject textBackground;
    public Text talkingText;
    public GameObject questIcon;
    public Text yesText;
    public Text noText;

    //Dialog Variables
    private bool canTalk = false;
    private bool isTalking = false;
    public bool yesSelected;
    public bool decisionTime;
    public bool timeToExit;
    public bool finishedTalking;
    public bool questAccepted = false;
    public bool questCompleted;
    public bool firstTalk = true;
    public bool lateUpdate = false;
    public bool keyRemoved = false;

    //Possible Dialog
    private string[] gateKeeperMessage = new string[] {"Howdy! I'm the gatekeeper of this passageway." +
        "I'd let ya through but one of them spiders took my key while I wasn't looking...", "Could ya go get it for me...? "+
        "The spider crawled off that way. \n \t Yes \n \t No", "Yeehaw! I'd go get it meself but I'm not supposed leave from this spot... I'll be waitin for ya here.",
        "Dangit! I guess you are just too busy. You won't be able to go through till I get that key back... "+
        " Come talk to me again if ya change your mind.", "Hello again! I still be needing that key... you changed your mind? \n \t Yes \n \t No",
        "Got the key yet? The spider crawled over there... I'll wait here...", "That looks like the key! Thank you so much! "+
        "I'll open that passage for you now. stay safe partner!", "Well I'll be, that key ya have there... Can I have a look at it?", "Huh this is it! I hadn't even told anyone about it yet! " +
        "Thank ya so much friend. Here, I'll open this here passageway for you", "Hey, looks like ya went and got it anyway! Thank you so much, I'll let ya through now",
        "Hey it's you! Thanks again!" };

    //Currently Displayed Dialogue Variable
    int currentlyDisplayingText = 0;

    // Use this for initialization
    void Start() {
        try {

            playerObject = GameObject.Find("Player");
            cameraObject = GameObject.Find("Camera");
            animator = NPCObject.GetComponent<Animator>();
            passageAnimator = passageWay.GetComponent<Animator>();
            animator.Play("Idle");

        } catch {

            playerObject = null;
            cameraObject = null;
        }
    }

    // Update is called once per frame
    void Update() {

        //Delayed variable to stop later action also being called
        if (lateUpdate) {
            time += Time.deltaTime;
            if (time > waitTime) {
                lateUpdate = false;
            }
        }

        if (yesSelected && decisionTime) {
            yesText.enabled = true;
            noText.enabled = false;
        } else if (!yesSelected && decisionTime) {
            noText.enabled = true;
            yesText.enabled = false;
        } else {
            yesText.enabled = false;
            noText.enabled = false;
        }

        if (decisionTime) {
            if (yesSelected && (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))) {
                yesSelected = false;
            } else if (!yesSelected && (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))) {
                yesSelected = true;
            }

            if (yesSelected && (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))) {
                yesSelected = false;
            } else if (!yesSelected && (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))) {
                yesSelected = true;
            }

            if (!finishedTalking && (Input.GetKeyDown("e"))) {
                SkipToNextText();
                finishedTalking = true;

                // Player accepts quest
            } else if (finishedTalking && yesSelected && (Input.GetKeyDown("e") || Input.GetKey(KeyCode.Return))) {
                currentlyDisplayingText = 2;
                decisionTime = false;
                timeToExit = true;
                questAccepted = true;
                StartCoroutine(AnimateText());

                //Delayed variable to stop later action also being called
                lateUpdate = true;

                // Player declines quest
            } else if (finishedTalking && !yesSelected && (Input.GetKeyDown("e") || Input.GetKey(KeyCode.Return))) {
                currentlyDisplayingText = 3;
                decisionTime = false;
                timeToExit = true;
                StartCoroutine(AnimateText());

                //Delayed variable to stop later action also being called
                lateUpdate = true;

            }
        }

        if (canTalk && !decisionTime && !isTalking && Input.GetKeyDown("e")) {
            //Talk to NPC Stop Time
            Time.timeScale = 0f;
            print("Time slowed");
            // Player talks to NPC for first time
            if (firstTalk && GameManager.instance.goldKey == false) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                questIcon.SetActive(false);
                isTalking = true;
                interactText.enabled = false;
                firstTalk = false;
                StartCoroutine(AnimateText());

                // Player talks to NPC after declining quest
            } else if (!firstTalk && !questAccepted && GameManager.instance.goldKey == false) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;
                interactText.enabled = false;
                yesSelected = true;
                decisionTime = true;
                questIcon.SetActive(false);
                currentlyDisplayingText = 4;
                StartCoroutine(AnimateText());

                // Player talks to NPC while still needing key
            } else if (questAccepted && GameManager.instance.goldKey == false) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;
                interactText.enabled = false;
                currentlyDisplayingText = 5;
                timeToExit = true;

                cameraObject.GetComponent<CameraScript>().SetFocusPoint(keyObject, 4.0f);

                StartCoroutine(AnimateText());
                

                // Player declined quest but still got key
            } else if (!questCompleted && !firstTalk && !questAccepted && GameManager.instance.goldKey == true) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;
                interactText.enabled = false;

                questIcon.SetActive(false);

                questCompleted = true;
                timeToExit = true;

                goldKeyUI.SetActive(false);
                Instantiate(keySound, transform.position, transform.rotation);
                keyRemoved = true;

                currentlyDisplayingText = 9;
                StartCoroutine(AnimateText());

                // Player gets key before talking to NPC
            } else if (!questCompleted && firstTalk && GameManager.instance.goldKey == true) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;

                questIcon.SetActive(false);

                questCompleted = true;

                currentlyDisplayingText = 7;
                StartCoroutine(AnimateText());

                // Player talks to NPC after obtaining key
            } else if (!questCompleted && questAccepted && GameManager.instance.goldKey == true) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;
                interactText.enabled = false;

                goldKeyUI.SetActive(false);
                Instantiate(keySound, transform.position, transform.rotation);
                keyRemoved = true;

                questCompleted = true;
                timeToExit = true;

                currentlyDisplayingText = 6;
                StartCoroutine(AnimateText());

                // Player talks to NPC after quest
            } else if (questCompleted) {
                textBackground.SetActive(true);
                talkingText.enabled = true;
                isTalking = true;
                interactText.enabled = false;
                currentlyDisplayingText = 10;
                StartCoroutine(AnimateText());
                timeToExit = true;
            }
        } else if (isTalking && !decisionTime && !lateUpdate && Input.GetKeyDown("e")) {
            if (!finishedTalking) {
                SkipToNextText();
            } else if (finishedTalking && timeToExit) {
                ExitText();
            } else if (finishedTalking && !timeToExit) {
                NextMessage();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            interactText.enabled = true;
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            if (decisionTime) {
                yesSelected = false;
            }
            interactText.enabled = false;
            isTalking = false;
            canTalk = false;
            textBackground.SetActive(false);
            talkingText.enabled = false;
            isTalking = false;

            if (!keyRemoved && GameManager.instance.goldKey == true) {
                questCompleted = true;

                goldKeyUI.SetActive(false);
                Instantiate(keySound, transform.position, transform.rotation);
                keyRemoved = true;

                OpenDoor();
            } else if (keyRemoved && GameManager.instance.goldKey == true) {
                OpenDoor();
            }
        }
    }

    //This is a function for a button you press to skip to the next text
    public void SkipToNextText() {
        StopAllCoroutines();
        talkingText.text = gateKeeperMessage[currentlyDisplayingText];
        finishedTalking = true;
    }

    public void NextMessage() {
        if (!decisionTime && !timeToExit) {
            currentlyDisplayingText++;
        }

        //If we've reached the end of the array, do anything you want. I just restart the example text
        if (currentlyDisplayingText > gateKeeperMessage.Length) {
            textBackground.SetActive(false);
            talkingText.enabled = false;
            isTalking = false;
            interactText.enabled = true;
            currentlyDisplayingText = gateKeeperMessage.Length;
        }

        if (currentlyDisplayingText == 1) {
            cameraObject.GetComponent<CameraScript>().SetFocusPoint(keyObject, 4.0f);
            yesSelected = true;
            decisionTime = true;
        }

        if (currentlyDisplayingText == 8) {
            StartCoroutine(AnimateText());

            goldKeyUI.SetActive(false);
            Instantiate(keySound, transform.position, transform.rotation);

            timeToExit = true;

        } else {
            StartCoroutine(AnimateText());
        }

    }

    public void ExitText() {
        if (questCompleted) {
            OpenDoor();
        }

        textBackground.SetActive(false);
        talkingText.enabled = false;
        isTalking = false;
        decisionTime = false;
        finishedTalking = false;
        timeToExit = false;
        interactText.enabled = true;

        if (yesSelected) {
            currentlyDisplayingText = 5;
        } else if (!questCompleted && !yesSelected) {
            currentlyDisplayingText = 4;
            questIcon.SetActive(true);
        }

        //Restart Time
        Time.timeScale = 1.0f;
        print("time started again");
    }

    IEnumerator AnimateText() {
        finishedTalking = false;

        for (int i = 0; i < (gateKeeperMessage[currentlyDisplayingText].Length + 1); i++) {
            talkingText.text = gateKeeperMessage[currentlyDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(.03f);
        }

        finishedTalking = true;
    }


    public void OpenDoor() {
        passageAnimator.Play("doorOpening");
    }
}

