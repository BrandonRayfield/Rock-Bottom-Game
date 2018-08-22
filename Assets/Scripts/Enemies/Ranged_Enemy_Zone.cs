using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_Enemy_Zone : MonoBehaviour {

    public GameObject rangedEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            rangedEnemy.GetComponent<Ranged_Enemy>().setIsPlayerNear(true);
            rangedEnemy.GetComponent<Ranged_Enemy>().updateCurrentTime();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            rangedEnemy.GetComponent<Ranged_Enemy>().setIsPlayerNear(false);
        }
    }

}
