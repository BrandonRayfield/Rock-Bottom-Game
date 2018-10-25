using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserHitBox : MonoBehaviour {
    public float damage = 25.0f;

    public bool hasDamaged = false;

    // Use this for initialization
    void Start() {

        hasDamaged = false;
    }

    void OnTriggerStay(Collider otherObject) {
            if (otherObject.transform.tag == "Player" && hasDamaged) {
                otherObject.GetComponent<Player>().takeDamage(damage);
            hasDamaged = false;
            }
        }

    public void Activate() {
        hasDamaged = true;
    }

    public void Deactivate() {
        hasDamaged = false;
    }
}

