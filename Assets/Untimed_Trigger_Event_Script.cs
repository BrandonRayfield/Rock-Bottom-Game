using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untimed_Trigger_Event_Script : MonoBehaviour {

    public GameObject playerObject;
    public GameObject lockedObject;

    private Animator lockedObjectAnimator;

    // Use this for initialization
    void Start () {
        lockedObjectAnimator = lockedObject.GetComponent<Animator>();

        try {

            playerObject = GameObject.Find("Player");

        } catch {
            playerObject = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            lockedObjectAnimator.Play("doorOpening");
        }
    }
}
