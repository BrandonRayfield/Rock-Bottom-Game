using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject player;

    private Vector3 focusPoint;

    private Vector3 cameraPosition;

    public bool boolNewFocus;
    private float time;
    private float focusTime = 4.0f;
    private bool isFinished;

    //Rotation vars
    private float rotationSpeed = 0.5f;
    private float adjRotSpeed;
    private Quaternion targetRotation;
	
    private void Start() {
        isFinished = false;

        try {

            player = GameObject.Find("Player");

        } catch {

            player = null;
        }
    }

	// Update is called once per frame
	void Update () {

        if (boolNewFocus) {
            player.GetComponent<Player>().setCutscene(true);
            time += Time.deltaTime;
            if (time > focusTime) {
                boolNewFocus = false;
                isFinished = true;
            }
        } else {
            time = 0;
            player.GetComponent<Player>().setCutscene(false);
            focusPoint = player.transform.position;
        }
        

        //Lerp focus towards focusPoint
        //targetRotation = Quaternion.LookRotation(focusPoint - transform.position);
        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

        //Camera Position Setup
        cameraPosition = focusPoint;
        cameraPosition.z = -2.0f;
        cameraPosition.y = cameraPosition.y + 1.00f;

        //Move Y position to mid point
        transform.position = Vector3.MoveTowards(transform.position, cameraPosition, 10.0f * Time.deltaTime);
    }

    public void SetFocusPoint(GameObject newFocus, float newFocusTime) {
        focusTime = newFocusTime;
        focusPoint = newFocus.transform.position;
        isFinished = false;
        boolNewFocus = true;
    }

    public bool getIsFinished() {
        return isFinished;
    }

}
