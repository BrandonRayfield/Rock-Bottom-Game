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
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F2)) {
                currentCheckpoint = 1;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F3)) {
                currentCheckpoint = 2;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F4)) {
                currentCheckpoint = 3;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F5)) {
                currentCheckpoint = 4;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F6)) {
                currentCheckpoint = 5;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F7)) {
                currentCheckpoint = 6;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }

            if (Input.GetKey(KeyCode.F8)) {
                currentCheckpoint = 7;
                if (checkpoint.Length > currentCheckpoint) {
                    gameObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                    cameraObject.transform.position = checkpoint[currentCheckpoint].transform.position;
                }
            }
        }
    }      
}
