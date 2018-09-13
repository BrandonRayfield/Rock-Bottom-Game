using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Zone : MonoBehaviour {

    public bool inRange, isDisabled;
    public GameObject bossBoundaries;
    public GameObject doorEnter;
    public GameObject doorExit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!inRange) {
            doorEnter.GetComponent<Animator>().Play("doorOpening");
            doorExit.GetComponent<Animator>().Play("doorOpening");
            bossBoundaries.SetActive(false);
        } else {
            doorEnter.GetComponent<Animator>().Play("doorClosing");
            doorExit.GetComponent<Animator>().Play("doorClosing");
            bossBoundaries.SetActive(true);
        }

        if(isDisabled) {
            inRange = false;
        }

	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !isDisabled) {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player" && !isDisabled) {
            inRange = false;
        }
    }

    public void setIsDisabled(bool disabled) {
        isDisabled = disabled;
    }

}
