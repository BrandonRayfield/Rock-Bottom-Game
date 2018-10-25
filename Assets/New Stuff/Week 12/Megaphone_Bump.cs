using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone_Bump : MonoBehaviour {

    private float currentDirection;
    private GameObject playerObject;
    private Vector3 playerLocation;

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

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Chaser" || other.gameObject.tag == "Flying_Enemy") {

            playerLocation = playerObject.transform.position;

            int direction = (int)Mathf.Sign(transform.position.x - playerLocation.x);

            Vector3 angle = new Vector3(direction * 5f, 2f, 0f);
            angle = Vector3.Normalize(angle);
            other.GetComponent<Rigidbody>().AddForce(angle * 250);
        }
    }


}
