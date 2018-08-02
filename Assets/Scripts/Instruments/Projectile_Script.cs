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



    }

    public void SetDamage(float damageAmount) {
        damage = damageAmount;
    }

}
