using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce_Script_Temp : MonoBehaviour {

    // Bounce Variables
    public bool bounce;
    public float bounceForce = 10;
    public GameObject playerObject;
    private Rigidbody rb;
    private Vector3 v3;

    // Sounds
    public GameObject bounceSound;

    // Parent Object
    public GameObject parentObject;

    // Use this for initialization
    void Start() {
        try {
            playerObject = GameObject.Find("Player");
            rb = playerObject.GetComponent<Rigidbody>();
        }
        catch {
            playerObject = null;
        }
    }

    // Update is called once per frame
    void Update() {
        if (bounce) {
            v3 = rb.velocity;
            v3.y = 0;
            rb.velocity = v3;
            rb.AddForce(0, bounceForce, 0, ForceMode.Impulse);
            bounce = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            bounce = true;
            parentObject.GetComponent<Disappear_Script>().setTrigger(true);
            bounceSound.GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.3f);
            Instantiate(bounceSound, transform.position, transform.rotation);
        }
    }
}
