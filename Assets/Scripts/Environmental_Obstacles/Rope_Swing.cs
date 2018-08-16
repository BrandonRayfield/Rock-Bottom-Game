using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope_Swing : MonoBehaviour {


    public bool isPitchChange;
    public GameObject player;
    public float jumpForce = 200.0f;
    private GameObject cameraObject;
    private Rigidbody playerRB;
    private Quaternion playerRot;

    //Test Stuff
    private Vector3 acceleration;
    private Vector3 lastVelocity;

    private float speed;
    private Vector3 mLastPosition;
    private float elapsedTime;

    // Pitch stuff
    private float currentPitch;
    private float newPitch;
    public float normalPitch = 1.0f;
    public float minPitch = 0.7f;
    public float maxPitch = 1.3f;

    public GameObject ropeObject;

    public float swingForce;

    private Quaternion ropeRotation;
    private float ropeRotZ;

    private float currentDirection;
    private Quaternion newRotation;

    private bool isSwinging;
    private bool hasCreated;

	// Use this for initialization
	void Start () {

        currentPitch = normalPitch;

        try {
            player = GameObject.Find("Player");
            cameraObject = GameObject.Find("Camera");
        } catch {
            player = null;
            cameraObject = null;
        }

        playerRB = player.GetComponent<Rigidbody>();
        playerRot = player.transform.rotation;

    }
	
	// Update is called once per frame
	void Update () {

        currentDirection = player.GetComponent<Player>().getPlayerDirection();
        cameraObject.GetComponent<AudioSource>().pitch = currentPitch;

        if (isSwinging) {

            //speed = playerRB.magnitude / elapsedTime;
            //mLastPosition = player.transform.position;
            //elapsedTime = Time.deltaTime;

            //currentPitch = speed;

            //if (newPitch > minPitch && newPitch < maxPitch) {
            //    currentPitch = newPitch;
            //}

            if (isPitchChange)
            {
                currentPitch = mapValue(playerRB.velocity.sqrMagnitude, 0, 6, 1f, 1.3f);
            }

            acceleration = (playerRB.velocity - lastVelocity) / Time.fixedDeltaTime;
            lastVelocity = playerRB.velocity;

            player.GetComponent<Player>().setCanMove(false);

            ropeRotation = ropeObject.transform.rotation;
            ropeRotZ = ropeRotation.eulerAngles.z;

            newRotation.eulerAngles = new Vector3(ropeRotation.eulerAngles.z * -currentDirection, playerRot.eulerAngles.y * currentDirection, playerRot.eulerAngles.z);
            player.transform.rotation = newRotation;

            //newRotation.eulerAngles = new Vector3(newRotation.eulerAngles.x, direction * 90, newRotation.eulerAngles.z);

            if (!hasCreated)
            {
                player.AddComponent<HingeJoint>();
                player.GetComponent<HingeJoint>().connectedBody = ropeObject.GetComponent<Rigidbody>();
                player.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
                player.GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, 0, 0);
                player.GetComponent<HingeJoint>().anchor = new Vector3(0, 0.54f, 0.15f);
                player.GetComponent<HingeJoint>().axis = new Vector3(0, 0, 0);
                hasCreated = true;
            }
            
            

            if(Input.GetKeyDown(KeyCode.A)) {
                ropeObject.GetComponent<Rigidbody>().AddForce(-transform.right * swingForce, ForceMode.Force);
                playerRB.velocity = playerRB.velocity + acceleration * Time.deltaTime;
                playerRB.position = playerRB.position + playerRB.velocity * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                ropeObject.GetComponent<Rigidbody>().AddForce(transform.right * swingForce, ForceMode.Force);
                playerRB.velocity = playerRB.velocity + acceleration * Time.deltaTime;
                playerRB.position = playerRB.position + playerRB.velocity * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                isSwinging = false;
                hasCreated = false;

                //Dismount player with force
                playerRB.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);

                // Reset player angle
                newRotation.eulerAngles = new Vector3(playerRot.eulerAngles.x, playerRot.eulerAngles.y * currentDirection, playerRot.eulerAngles.z);
                player.transform.rotation = newRotation;

                // Reset BG Music Pitch
                currentPitch = normalPitch;

                player.GetComponent<Player>().setCanSwing(false);
                player.GetComponent<Player>().setCanMove(true);
                Destroy(player.GetComponent<HingeJoint>());
                player.transform.parent = null;
            }

        }
		
	}

    float mapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax) {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }

    public void setIsSwinging(bool canSwing) {
        isSwinging = canSwing;
    }

}
