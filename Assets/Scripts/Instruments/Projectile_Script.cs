using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Script : MonoBehaviour {

    public bool isFriendly;
    private float damage;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(!isFriendly && other.transform.tag == "Player") {
            other.GetComponent<Player>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        } else if(isFriendly && other.transform.tag == "Enemy") {
            other.GetComponent<Enemy>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        }

        //----------------------------------------------------------------------------------
        // These are 'work arounds' for now. Delete after reworking enemy class.
        else if (isFriendly && other.transform.tag == "FlyingEnemy") {
            other.GetComponent<Flying_Enemy>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        } else if (isFriendly && other.transform.tag == "Ranged_Enemy") {
            other.GetComponent<Ranged_Enemy>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        } else if (isFriendly && other.transform.tag == "Ranged_Enemy2") {
            other.GetComponent<Ranged_Enemy2>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        } else if (isFriendly && other.transform.tag == "Chaser") {
            other.GetComponent<Chaser>().takeDamage(damage);
            //Create explosion effect?
            Destroy(gameObject);
        }
        //----------------------------------------------------------------------------------

        else if (other.transform.tag == "Destructable") {
            other.GetComponent<Destructable_Object>().takeDamage(damage);
            Destroy(gameObject);
        } else if (other.transform.tag == "Wall" || other.transform.tag == "Obstacle") {
            Destroy(gameObject);
        }



    }

    public void SetDamage(float damageAmount) {
        damage = damageAmount;
    }

}
