using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitBox : MonoBehaviour {

    public float lifeTime = 0.1f;

    public float damage = 25.0f;

    public bool player = true;

    private bool hasDamaged;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifeTime);
        hasDamaged = false;
    }

    void OnTriggerStay(Collider otherObject) {

        if (player) {
            if (otherObject.transform.tag == "Enemy" && !hasDamaged) {
                otherObject.GetComponent<Enemy>().takeDamage(damage);
                hasDamaged = true;
            } else if (otherObject.transform.tag == "FlyingEnemy" && !hasDamaged) {
                otherObject.GetComponent<Flying_Enemy>().takeDamage(damage);
                hasDamaged = true;
            } else if (otherObject.transform.tag == "Destructable" && !hasDamaged) {
                otherObject.GetComponent<Destructable_Object>().takeDamage(damage);
                hasDamaged = true;
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
