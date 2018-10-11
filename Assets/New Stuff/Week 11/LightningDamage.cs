using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDamage : MonoBehaviour {

    public float lifeTime = 0.1f;

    public float damage = 25.0f;

    public bool player = true;

    private bool hasDamaged;

    public GameObject guitarPoint;

    public GameObject ChainChecker;

    private bool hasntSpawned;

    // Use this for initialization
    void Start() {
        Destroy(this.gameObject, lifeTime);
        hasDamaged = false;
    }

    private void Update() {

    }

    void OnTriggerStay(Collider otherObject) {

        RaycastHit hit;

        if (player) {
            if (otherObject.transform.tag == "Enemy") {
                if (Physics.Raycast(guitarPoint.transform.position, -(guitarPoint.transform.position - otherObject.transform.position).normalized, out hit,
                                                            Vector3.Distance(guitarPoint.transform.position, otherObject.transform.position))) {
                    if (hit.transform.GetComponent<Enemy>() != null) {
                        hit.transform.GetComponent<Enemy>().takeDamage(damage);
                        GameObject newLightning = Instantiate(ChainChecker, otherObject.transform.position, ChainChecker.transform.rotation);
                        newLightning.GetComponent<LightningChain>().SetParent(gameObject);
                    }


                    else if (hit.transform.GetComponent<Chaser>() != null)
                        hit.transform.GetComponent<Chaser>().takeDamage(damage);
                    else if (hit.transform.GetComponent<Boss>() != null)
                        hit.transform.GetComponent<Boss>().takeDamage(damage);
                    else if (hit.transform.GetComponent<Large_Spider>() != null)
                        hit.transform.GetComponent<Large_Spider>().takeDamage(damage);
                }
            }
            //----------------------------------------------------------------------------------
            else if (otherObject.transform.tag == "FlyingEnemy") {
                if (Physics.Raycast(guitarPoint.transform.position, -(guitarPoint.transform.position - otherObject.transform.position).normalized, out hit,
                                                                Vector3.Distance(guitarPoint.transform.position, otherObject.transform.position))) {
                    if (otherObject.GetComponent<Flying_Enemy>()) {
                        otherObject.GetComponent<Flying_Enemy>().takeDamage(damage);
                        if(!hasntSpawned) {
                            GameObject newLightning = Instantiate(ChainChecker, otherObject.transform.position, ChainChecker.transform.rotation);
                            newLightning.GetComponent<LightningChain>().SetParent(gameObject);
                            hasntSpawned = true;
                        }
                    }
                    else if (otherObject.GetComponent<Flying_Crush_Enemy>())
                        otherObject.GetComponent<Flying_Crush_Enemy>().takeDamage(damage);
                    else if (otherObject.GetComponent<Flying_Kamikaze_Enemy>())
                        otherObject.GetComponent<Flying_Kamikaze_Enemy>().takeDamage(damage);
                }
            }
            //----------------------------------------------------------------------------------
            else if (otherObject.transform.tag == "Ranged_Enemy") {
                if (!Physics.Raycast(guitarPoint.transform.position, -(guitarPoint.transform.position - otherObject.transform.position).normalized, out hit,
                                                                Vector3.Distance(guitarPoint.transform.position, otherObject.transform.position))) {
                    otherObject.GetComponent<Ranged_Enemy>().takeDamage(damage);
                }
            }
            //----------------------------------------------------------------------------------
            else if (otherObject.transform.tag == "Ranged_Enemy2") {
                if (!Physics.Raycast(guitarPoint.transform.position, -(guitarPoint.transform.position - otherObject.transform.position).normalized, out hit,
                                                                Vector3.Distance(guitarPoint.transform.position, otherObject.transform.position))) {
                    otherObject.GetComponent<Ranged_Enemy2>().takeDamage(damage);
                }
            }
            //----------------------------------------------------------------------------------
            else if (otherObject.transform.tag == "Chaser") {
                if (Physics.Raycast(guitarPoint.transform.position, -(guitarPoint.transform.position - otherObject.transform.position).normalized, out hit,
                                                                Vector3.Distance(guitarPoint.transform.position, otherObject.transform.position))) {
                    otherObject.GetComponent<Chaser>().takeDamage(damage);
                }
            }
        }
    }

    public void moveForward(float distance) {
        transform.Translate(new Vector3(distance, 0, 0), Space.Self);
    }
}
