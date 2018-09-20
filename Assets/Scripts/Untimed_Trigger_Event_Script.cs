using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Untimed_Trigger_Event_Script : MonoBehaviour {

    // General Variables
    public GameObject playerObject;
    public GameObject lockedObject;
    public float focusTime = 4.0f;
    private GameObject cameraObject;

    // Sound Variables
    public GameObject guitarSound;
    public GameObject doorSound;
    public GameObject errorSound;

    // UI Variables
    public Text interactText;

    //Trigger Variables
    private bool isTouching;
    private float time;
    private bool padTriggered;
    private bool hasUsed;
    public float songLength = 1.0f;

    // Animation Variables
    private Animator playerAnimator;
    private Animator lockedObjectAnimator;
	public string animationName; 

    // Currency Variables
    public int costAmount;
    private int currentAmount;
    public Text costText;

    // Quest variables
    public bool isQuestStage;
    public int questID;
    private int stageValue = 1;
    public bool isGuitarOnly;
    private bool isTalking;
    private bool isFirstAttemmpt = true;
    private GameObject currentGuitarObject;
    public GameObject errorDialogueTrigger;
    public GameObject workingDialogueTrigger;

    // Use this for initialization
    void Start () {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        if(costText == null) {
            costText = interactText.gameObject.transform.GetChild(0).GetComponent<Text>();
        }

        try {

            playerObject = GameObject.Find("Player");
            cameraObject = GameObject.Find("Camera");
            interactText = GameObject.Find("InteractText").GetComponent<Text>();
            playerAnimator = playerObject.GetComponent<Animator>();

        } catch {
            playerObject = null;
            cameraObject = null;
            playerAnimator = null;
        }
    }

    private void activatePad() {
        currentAmount = playerObject.GetComponent<Player>().getCurrencyAmount();
        if (currentAmount >= costAmount) {
            //pause enemies
            GameManager.instance.isTalking = true;
            playerObject.GetComponent<PauseMenu>().selectGuitar();
            playerAnimator.Play("Guitar Playing");
            Instantiate(guitarSound, transform.position, transform.rotation);
            interactText.enabled = false;
            padTriggered = true;
            hasUsed = true;

            playerObject.GetComponent<Player>().updateCurrencyAmount(costAmount);
            costText.enabled = false;

            if (isQuestStage) {
                GameManager.instance.AddCounter(questID, stageValue);
            } else {
                Instantiate(errorSound, transform.position, transform.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (isGuitarOnly) {
            isTalking = errorDialogueTrigger.GetComponent<DialogueTrigger>().isTalking;
        }

        currentGuitarObject = playerObject.GetComponent<Player>().currentGuitarObject;

        if (isTouching && !hasUsed && Input.GetKeyDown(KeyCode.E)) {
            if (isGuitarOnly && currentGuitarObject != playerObject.GetComponent<Player>().guitarObject) {
                if(isFirstAttemmpt) {
                    Instantiate(errorSound, transform.position, transform.rotation);
                    errorDialogueTrigger.SetActive(true);
                    interactText.gameObject.SetActive(false);
                    isFirstAttemmpt = false;
                }

                if(!isTalking) {
                    Instantiate(errorSound, transform.position, transform.rotation);
                }

            } else if (isGuitarOnly && currentGuitarObject == playerObject.GetComponent<Player>().guitarObject) {
                activatePad();
                workingDialogueTrigger.SetActive(true);
                interactText.gameObject.SetActive(false);

            } else if (!isGuitarOnly) {
                activatePad();
            }
        }


        if (padTriggered) {
            time += Time.deltaTime;
            if (time >= songLength) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(lockedObject, focusTime);
				lockedObjectAnimator.Play(animationName);
                Instantiate(doorSound, transform.position, transform.rotation);
                time = 0;
                padTriggered = false;
                //resume enemies
                GameManager.instance.isTalking = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = true;
            if(!hasUsed) {
                interactText.gameObject.SetActive(true);
                interactText.text = "Press 'E' to Rock out!";
                interactText.enabled = true;
                costText.text = "Required: " + costAmount + " Beat Coins";
                costText.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = false;
            interactText.enabled = false;
            costText.enabled = false;
        }
    }
}
