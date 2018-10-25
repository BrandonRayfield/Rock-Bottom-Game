using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Script : MonoBehaviour {

    public bool isFriendly;
    private float damage;

    public bool isExplosive;
    public bool upgradedExplosive;
    public GameObject explosion;
    public GameObject explosionSound;

    private float time;
    private float lifeTime = 3;

	// Use this for initialization
	void Start () {

    }

    private void Awake() {
        time = 0;
    }

    // Update is called once per frame
    void Update () {
        if(isExplosive) {
            time += Time.deltaTime;
            if (time >= lifeTime) {
                Detonate();
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (!isFriendly && other.transform.tag == "Player") {
            other.GetComponent<Player>().takeDamage(damage);
            Destroy(gameObject);
        } else if (isFriendly && other.transform.tag == "Enemy") {
            if (other.GetComponent<Enemy>() != null) {
                if (isExplosive) {
                    Detonate();
                } else {
                    other.GetComponent<Enemy>().takeDamage(damage);
                    Destroy(gameObject);
                }
            } else if (other.GetComponent<Chaser>() != null) {
                if (isExplosive) {
                    Detonate();
                } else {
                    other.GetComponent<Chaser>().takeDamage(damage);
                    Destroy(gameObject);
                }
            } else if (other.GetComponent<Boss>() != null) {
                if (isExplosive) {
                    Detonate();
                } else {
                    other.GetComponent<Boss>().takeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }

        //----------------------------------------------------------------------------------
        // These are 'work arounds' for now. Delete after reworking enemy class.
        else if (isFriendly && other.transform.tag == "FlyingEnemy") {
            if (isExplosive) {
                Detonate();
            } else {
                other.GetComponent<Flying_Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
        } else if (isFriendly && other.transform.tag == "Ranged_Enemy") {
            if (isExplosive) {
                Detonate();
            } else {
                other.GetComponent<Ranged_Enemy>().takeDamage(damage);
                Destroy(gameObject);
            }
        } else if (isFriendly && other.transform.tag == "Ranged_Enemy2") {
            if (isExplosive) {
                Detonate();
            } else {
                other.GetComponent<Ranged_Enemy2>().takeDamage(damage);
                Destroy(gameObject);
            }
        } else if (isFriendly && other.transform.tag == "Chaser") {
            if (isExplosive) {
                Detonate();
            } else {
                other.GetComponent<Chaser>().takeDamage(damage);
                Destroy(gameObject);
            }
        }
        //----------------------------------------------------------------------------------

        else if (other.transform.tag == "Destructable") {
            other.GetComponent<Destructable_Object>().takeDamage(damage);
            Destroy(gameObject);
        } else if (other.transform.tag == "Wall" || other.transform.tag == "Obstacle") {
            if (isExplosive && !upgradedExplosive) {
                GetComponent<Rigidbody>().AddForce(transform.up * 50);
                Detonate();
                //Instantiate(explosionSound, transform.position, transform.rotation);
            } else if (isExplosive && upgradedExplosive) {
                //GetComponent<Rigidbody>().AddForce(transform.up * 50);
            } else {
                Destroy(gameObject);
            }
        }



    }

    public void Detonate() {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void SetDamage(float damageAmount) {
        damage = damageAmount;
    }
}
