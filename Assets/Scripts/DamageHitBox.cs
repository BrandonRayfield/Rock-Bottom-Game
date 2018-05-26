using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitBox : MonoBehaviour {

    public float lifeTime = 0.1f;

    public float damage = 25.0f;

    public bool player = true;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifeTime);
	}

    void OnTriggerStay(Collider otherObject) {

        if (player) {
            if (otherObject.transform.tag == "Enemy") {
                otherObject.GetComponent<Enemy>().takeDamage(damage);
                Destroy(this.gameObject);
            }
        } else if (!player) {
            if (otherObject.transform.tag == "Player") {
                otherObject.GetComponent<Player>().takeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }

    public void moveForward(float distance) {

           transform.Translate(new Vector3(distance,0,0),Space.Self);
    }
}
