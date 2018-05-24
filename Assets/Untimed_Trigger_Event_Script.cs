using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untimed_Trigger_Event_Script : MonoBehaviour {

    public GameObject playerObject;
    public GameObject lockedObject;
    private GameObject cameraObject;

    private bool isTouching;
    private float time;
    private bool padTriggered;

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
            padTriggered = true;
        }

        if (padTriggered) {
            time += Time.deltaTime;
            if (time >= 1) {
                cameraObject.GetComponent<Camera>().SetFocusPoint(lockedObject);
                lockedObjectAnimator.Play("doorOpening");
                time = 0;
                padTriggered = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            isTouching = false;
        }
    }
}
