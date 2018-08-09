using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

    private float time;
    public float fallTime = 1.0f;
    public float respawnTime = 9.0f;
    private bool playerTouched = false;

    private GameObject fallingPlatform;
    private Vector3 platformStartPosition;
    private Quaternion platformStartRotation;

	// Use this for initialization
	void Start () {
        fallingPlatform = this.gameObject;
        platformStartPosition = fallingPlatform.transform.position;
        platformStartRotation = fallingPlatform.transform.rotation;

    }
	
	// Update is called once per frame
	void Update () {
        if (playerTouched)
        {
            time += Time.deltaTime;

            if (time > fallTime)
            {
                fallingPlatform.GetComponent<Rigidbody>().useGravity = true;
                fallingPlatform.GetComponent<Rigidbody>().isKinematic = false;
            }

            if (time > respawnTime) {
                fallingPlatform.GetComponent<Rigidbody>().useGravity = false;
                fallingPlatform.GetComponent<Rigidbody>().isKinematic = true;
                playerTouched = false;
                fallingPlatform.transform.position = platformStartPosition;
                fallingPlatform.transform.rotation = platformStartRotation;
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
