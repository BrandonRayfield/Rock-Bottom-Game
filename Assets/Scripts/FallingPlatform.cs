using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

    private float time;
    public float fallTime = 1.0f;
    public float respawnTime = 9.0f;
    private bool playerTouched = false;
    private bool isShaking;

    private GameObject fallingPlatform;
    private Animator platformAnimator;
    private Vector3 platformStartPosition;
    private Quaternion platformStartRotation;

	// Use this for initialization
	void Start () {
        fallingPlatform = gameObject.transform.GetChild(0).gameObject;
        platformAnimator = fallingPlatform.GetComponent<Animator>();
        platformStartPosition = transform.position;
        platformStartRotation = transform.rotation;

    }
	
	// Update is called once per frame
	void Update () {

        if(isShaking) {
            platformAnimator.Play("shaking_platform");
        } else {
            platformAnimator.Play("stable");
        }

        if (playerTouched)
        {
            isShaking = true;
            time += Time.deltaTime;
            if (time > fallTime)
            {
                isShaking = false;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }

            if (time > respawnTime) {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().isKinematic = true;
                playerTouched = false;
                isShaking = false;
                transform.position = platformStartPosition;
                transform.rotation = platformStartRotation;
                time = 0;
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player") {
            playerTouched = true;
        }
    }

}
