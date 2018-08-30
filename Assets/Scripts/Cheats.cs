using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {

    public GameObject[] checkpoint;
    private GameObject cameraObject;

    private int currentCheckpoint;

    public bool isAdmin;
    
	// Use this for initialization
	void Start () {
        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        checkpoint = GameObject.FindGameObjectsWithTag("Checkpoint");

    }

    // Update is called once per frame
    void Update () {
		
        if(isAdmin) {
            if (Input.GetKey(KeyCode.F1)) {
                currentCheckpoint = 0;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F2)) {
                currentCheckpoint = 1;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F3)) {
                currentCheckpoint = 2;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F4)) {
                currentCheckpoint = 3;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F5)) {
                currentCheckpoint = 4;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F6)) {
                currentCheckpoint = 5;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F7)) {
                currentCheckpoint = 6;
                teleportPlayer(currentCheckpoint);
            }

            if (Input.GetKey(KeyCode.F8)) {
                currentCheckpoint = 7;
                teleportPlayer(currentCheckpoint);
            }
        }
    }

    private void teleportPlayer(int newCheckpoint) {
        if (checkpoint.Length > newCheckpoint) {
            gameObject.transform.position = new Vector3(checkpoint[newCheckpoint].transform.position.x, checkpoint[newCheckpoint].transform.position.y, gameObject.transform.position.z);
            cameraObject.transform.position = checkpoint[newCheckpoint].transform.position;
        }
    }
          
}
