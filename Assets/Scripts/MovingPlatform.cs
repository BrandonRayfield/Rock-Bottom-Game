using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject playerObject;

    // Use this for initialization
    void Start () {
        try {
            playerObject = GameObject.Find("Player");
        } catch {
            playerObject = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other) {
        if (other.gameObject == playerObject) {
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            transform.parent = null;
        }
    }


}
