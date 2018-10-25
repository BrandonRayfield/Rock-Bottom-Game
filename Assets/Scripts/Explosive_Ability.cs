using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Ability : MonoBehaviour {

    private float time = 0.0f;
    public float lifeTime = 3.0f;
    private float damage;

    // Use this for initialization
    void Start() {
        damage = 100;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= lifeTime)
            Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider otherObject) {
        if (otherObject.transform.tag == "Enemy") {
            if (otherObject.transform.GetComponent<Enemy>() != null) {
                otherObject.transform.GetComponent<Enemy>().takeDamage(damage);
            } else if (otherObject.transform.GetComponent<Chaser>() != null) {
                otherObject.transform.GetComponent<Chaser>().takeDamage(damage);
            } else if (otherObject.transform.GetComponent<Boss>() != null) {
                otherObject.transform.GetComponent<Boss>().takeDamage(damage);
            } else if (otherObject.transform.GetComponent<Large_Spider>() != null) {
                otherObject.transform.GetComponent<Large_Spider>().takeDamage(damage);
            }
        }
            //----------------------------------------------------------------------------------
            else if (otherObject.transform.tag == "FlyingEnemy") {
            if (otherObject.GetComponent<Flying_Enemy>()) {
                otherObject.GetComponent<Flying_Enemy>().takeDamage(damage);
            } else if (otherObject.GetComponent<Flying_Crush_Enemy>()) {
                otherObject.GetComponent<Flying_Crush_Enemy>().takeDamage(damage);
            } else if (otherObject.GetComponent<Flying_Kamikaze_Enemy>()) {
                otherObject.GetComponent<Flying_Kamikaze_Enemy>().takeDamage(damage);
            }
        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Ranged_Enemy") {
            otherObject.GetComponent<Ranged_Enemy>().takeDamage(damage);
        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Ranged_Enemy2") {
            otherObject.GetComponent<Ranged_Enemy2>().takeDamage(damage);
        }
        //----------------------------------------------------------------------------------
        else if (otherObject.transform.tag == "Chaser") {
            otherObject.GetComponent<Chaser>().takeDamage(damage);
        }
    }
}
