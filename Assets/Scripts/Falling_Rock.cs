using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Rock : MonoBehaviour {

    public float damage;
    public float lifeTime;
    public GameObject rockObject;

	// Use this for initialization
	void Start () {
        Destroy(rockObject, lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Player>().takeDamage(damage);
            Destroy(rockObject);
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Destructable") {
            Destroy(gameObject);
        }
    }
}
