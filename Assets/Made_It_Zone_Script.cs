using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Made_It_Zone_Script : MonoBehaviour {

    public GameObject playerObject;
    public GameObject triggerObject;
    private bool madeIt;
    private bool isActive;

	// Use this for initialization
	void Start () {
        madeIt = false;
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

        isActive = triggerObject.GetComponent<Timed_Trigger_Event_Script>().getIsActive();

        if (isActive && other.gameObject == playerObject) {
            madeIt = true;
        }
    }

    public bool getMadeItStatus() {
        return madeIt;
    }

    public void setMadeItStatus(bool result) {
        madeIt = result;
    }

}
