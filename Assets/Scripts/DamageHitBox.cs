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
                if (otherObject.GetComponent<Enemy>() != null)
                    otherObject.GetComponent<Enemy>().takeDamage(damage);
                else if (otherObject.GetComponent<Chaser>() != null)
                    otherObject.GetComponent<Chaser>().takeDamage(damage);
                hasDamaged = true;
            }
            //----------------------------------------------------------------------------------
            // These are 'work arounds' for now. Delete after reworking enemy class.
            else if (otherObject.transform.tag == "FlyingEnemy")
            {
                otherObject.GetComponent<Flying_Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
            else if (otherObject.transform.tag == "Ranged_Enemy")
            {
                otherObject.GetComponent<Ranged_Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
            else if (otherObject.transform.tag == "Ranged_Enemy2")
            {
                otherObject.GetComponent<Ranged_Enemy2>().takeDamage(damage);
                Destroy(gameObject);
            }
            //----------------------------------------------------------------------------------

            else if (otherObject.transform.tag == "Destructable" && !hasDamaged) {
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
