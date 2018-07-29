using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Platform : MonoBehaviour {

    public GameObject playerObject;
    public GameObject platformObject;
    private Collider platformCollider;

    // Use this for initialization
    void Start () {
        try {

            playerObject = GameObject.Find("Player");
            platformObject = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
            platformCollider = platformObject.GetComponent<Collider>();

        } catch {
            playerObject = null;
            platformObject = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            platformCollider.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            platformCollider.enabled = true;
        }
    }

}
