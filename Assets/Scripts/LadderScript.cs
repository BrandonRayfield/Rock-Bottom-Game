using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderScript : MonoBehaviour {

    public GameObject playerObject;
    private bool canClimb = false;
    private float speed = 3f;
    private bool isClimb = false;
    public Text climbText;

    // Use this for initialization
    void Start() {
        try {

            playerObject = GameObject.Find("Player");

        } catch {

            playerObject = null;
        }
    }

    // Update is called once per frame
    void Update() {
        if (canClimb && !isClimb && Input.GetKeyDown("e")) {
            playerObject.GetComponent<Rigidbody>().useGravity = false;
            playerObject.GetComponent<Rigidbody>().isKinematic = true;
            playerObject.GetComponent<Rigidbody>().isKinematic = false;
            climbText.enabled = false;
            isClimb = true;
        } else if (isClimb && Input.GetKeyDown("e")) {
            playerObject.GetComponent<Rigidbody>().useGravity = true;
            climbText.enabled = true;
            isClimb = false;
        }

        if (isClimb && Input.GetKey("w")) {
            float v = Input.GetAxis("Vertical");
            if (v != 0) {
                playerObject.transform.Translate(Vector3.up * v * Time.deltaTime * speed);
            }
        } else if (isClimb && Input.GetKey("s")) {
            float v = Input.GetAxis("Vertical");
            if (v != 0) {
                playerObject.transform.Translate(Vector3.down * -v * Time.deltaTime * speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            climbText.enabled = true;
            canClimb = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject == playerObject) {
            other.attachedRigidbody.useGravity = true;
            climbText.enabled = false;
            playerObject.GetComponent<Player>().enabled = true;
            isClimb = false;
            canClimb = false;
        }
    }


}
