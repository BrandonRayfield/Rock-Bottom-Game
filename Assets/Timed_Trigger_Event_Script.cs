using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timed_Trigger_Event_Script : MonoBehaviour {

    public GameObject playerObject;
    public Text timerText;
    public float maxTime;
    private float currentTime;
    private float restartTime = 0;
    public bool isActive;

    public GameObject lockedObject;
    public bool madeIt;
    public GameObject madeItZone;

    private Animator lockedObjectAnimator;

    // Use this for initialization
    void Start() {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        try {

            playerObject = GameObject.Find("Player");

        } catch {
            playerObject = null;
        }
    }

    // Update is called once per frame
    void Update() {

        if (isActive) {
            // Count down
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
                isActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            currentTime = maxTime;
            timerText.enabled = true;
            lockedObjectAnimator.Play("doorOpening");
            isActive = true;
        }
    }

    public bool getIsActive() {
        return isActive;
    }

}
