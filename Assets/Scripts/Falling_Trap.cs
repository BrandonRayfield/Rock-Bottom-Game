using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Trap : MonoBehaviour {

    public GameObject fallingObject;
    public GameObject fallingObjectTrigger;
    private Vector3 fallingObjectStartPos;
    private Quaternion fallingObjectStartRot;

    public float respawnTime;
    public float disappearTime;
    private float currentTime;

    private bool hitObject;

	// Use this for initialization
	void Start () {
        fallingObjectStartPos = fallingObject.transform.position;
        fallingObjectStartRot = fallingObject.transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {

        hitObject = fallingObjectTrigger.GetComponent<Falling_Rock_Trap>().getHitObject();

		if (hitObject) {
            currentTime = currentTime += Time.deltaTime;

            if (currentTime >= disappearTime) {
                fallingObject.SetActive(false);
            }

            if (currentTime >= respawnTime) {
                fallingObject.transform.position = fallingObjectStartPos;
                fallingObject.transform.rotation = fallingObjectStartRot;
                fallingObject.SetActive(true);
                fallingObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
                fallingObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                fallingObjectTrigger.GetComponent<Falling_Rock_Trap>().setHitObject(false);
                currentTime = 0;
            }
        }
	}

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            fallingObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
            fallingObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
