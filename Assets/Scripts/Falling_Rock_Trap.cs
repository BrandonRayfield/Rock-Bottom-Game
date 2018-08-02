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


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
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
