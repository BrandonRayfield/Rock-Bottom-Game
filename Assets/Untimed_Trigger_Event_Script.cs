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

    // UI Variables
    public Text interactText;

    //Trigger Variables
    private bool isTouching;
    private float time;
    private bool padTriggered;

    // Animation Variables
    private Animator playerAnimator;
    private Animator lockedObjectAnimator;

    // Use this for initialization
    void Start () {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        try {

            playerObject = GameObject.Find("Player");
            cameraObject = GameObject.Find("Camera");
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
                lockedObjectAnimator.Play("doorOpening");
                time = 0;
                padTriggered = false;
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
}
