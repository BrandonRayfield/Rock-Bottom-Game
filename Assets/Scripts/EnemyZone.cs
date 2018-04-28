using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour {

    public GameObject thisEnemy;

    void OnTriggerEnter(Collider otherObject) {

        if (otherObject.tag == "Player")
            thisEnemy.GetComponent<Enemy>().target = otherObject.gameObject;
    }

    void OnTriggerExit(Collider otherObject) {

        if (otherObject.tag == "Player")
            thisEnemy.GetComponent<Enemy>().target = null;
    }
}
