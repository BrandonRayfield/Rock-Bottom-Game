using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timed_Trigger_Event_Script : MonoBehaviour {

    // Player and Camera Variables
    public GameObject playerObject;
    private GameObject cameraObject;

    // Sound Variables
    public GameObject guitarSound;
    public GameObject doorSound;

    // UI Variables
    public Text interactText;

    //Timer Variables
    public Text timerText;
    public float maxTime;
    private float currentTime;
    private float restartTime = 0;
    public bool isActive;
    private bool isTouching;
    private float time;
    private bool padTriggered;
    private bool cutsceneTriggered;

    // Check if player made it in time Variables
    public GameObject lockedObject;
    public bool madeIt;
    public GameObject madeItZone;

    private Animator playerAnimator;
    private Animator lockedObjectAnimator;

    // Use this for initialization
    void Start() {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        try {

            playerObject = GameObject.Find("Player");
            cameraObject = GameObject.Find("Camera");
            interactText = GameObject.Find("InteractText").GetComponent<Text>();
            timerText = GameObject.Find("CountDown").GetComponent<Text>();
            playerAnimator = playerObject.GetComponent<Animator>();

        } catch {
            playerObject = null;
            cameraObject = null;
            playerAnimator = null;
        }
    }

    // Update is called once per frame
    void Update() {

        if (isTouching && Input.GetKeyDown(KeyCode.E)) {
            playerAnimator.Play("Guitar Playing");
            Instantiate(guitarSound, transform.position, transform.rotation);
            padTriggered = true;
        }

        if (padTriggered) {
            time += Time.deltaTime;
            if (time >= 1) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(lockedObject);
                time = 0;
                padTriggered = false;
                currentTime = maxTime;
                lockedObjectAnimator.Play("doorOpening");
                Instantiate(doorSound, transform.position, transform.rotation);
                cutsceneTriggered = true;
            }
        }

        if (cutsceneTriggered) {
            timerText.enabled = true;
            timerText.text = Mathf.RoundToInt(currentTime).ToString();
            isActive = cameraObject.GetComponent<CameraScript>().getIsFinished();
            Debug.Log(isActive);
        }



        if (isActive) {
            cutsceneTriggered = false;
            // Count down
            timerText.enabled = true;
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.RoundToInt(currentTime).ToString();
            madeIt = madeItZone.GetComponent<Made_It_Zone_Script>().getMadeItStatus();
            if (madeIt) {
                timerText.enabled = false;
                isActive = false;
            } else if (currentTime < restartTime && !madeIt) {
                madeItZone.GetComponent<Made_It_Zone_Script>().setMadeItStatus(false);
                timerText.enabled = false;
                lockedObjectAnimator.Play("doorClosing");
                Instantiate(doorSound, transform.position, transform.rotation);
                isActive = false;
                
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = true;
            interactText.text = "Press 'E' to Rock out!";
            interactText.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = false;
            interactText.enabled = false;
        }
    }

    public bool getIsActive() {
        return isActive;
    }

}
