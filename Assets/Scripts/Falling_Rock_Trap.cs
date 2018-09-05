using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Rock_Trap : MonoBehaviour {

    public float respawnTime;
    private float currentTime;

    private bool hitObject;

    public float damage;
    public GameObject rockObject;
    public GameObject rockModel;

    public Vector3 com;
    public Rigidbody rb;

    public void Start() {
        //com = new Vector3 (rockModel.transform.position.x, rockModel.transform.position.y - 0.3f, rockModel.transform.position.z);

        rb = rockObject.GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }

    private void OnTriggerEnter(Collider other) {
        if (!hitObject && other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().takeDamage(damage);
            rockObject.SetActive(false);
            hitObject = true;
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Destructable") {
            hitObject = true;
        }
    }

    public bool getHitObject() {
        return hitObject;
    }

    public void setHitObject(bool hasHit) {
        hitObject = hasHit;
    }

}
