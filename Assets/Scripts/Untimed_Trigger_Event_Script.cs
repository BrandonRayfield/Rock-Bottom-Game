using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Untimed_Trigger_Event_Script : MonoBehaviour {

    // General Variables
    public GameObject playerObject;
    public GameObject lockedObject;
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
	public string animationName = "doorOpening"; 

    // Currency Variables
    public int costAmount;
    private int currentAmount;
    public Text costText;

    // Quest variables
    public bool isQuestStage;
    public int questID;
    private int stageValue = 1;

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

    // Update is called once per frame
    void Update() {

        if (isTouching && !hasUsed && Input.GetKeyDown(KeyCode.E)) {

            currentAmount = playerObject.GetComponent<Player>().getCurrencyAmount();

            if(currentAmount >= costAmount) {
                playerAnimator.Play("Guitar Playing");
                Instantiate(guitarSound, transform.position, transform.rotation);
                interactText.enabled = false;
                padTriggered = true;
                hasUsed = true;

                playerObject.GetComponent<Player>().updateCurrencyAmount(costAmount);
                costText.enabled = false;

                if (isQuestStage) {
                    GameManager.instance.AddCounter(questID, stageValue);
                }
            } else {
                Instantiate(errorSound, transform.position, transform.rotation);
            }


        }

        if (padTriggered) {
            time += Time.deltaTime;
            if (time >= songLength) {
                cameraObject.GetComponent<CameraScript>().SetFocusPoint(lockedObject);
				lockedObjectAnimator.Play(animationName);
                Instantiate(doorSound, transform.position, transform.rotation);
                time = 0;
                padTriggered = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = true;
            if(!hasUsed) {
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
