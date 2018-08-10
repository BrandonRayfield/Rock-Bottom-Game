﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Object : MonoBehaviour {

    public GameObject baseVersion;
    public GameObject damagedVersion;
    public GameObject destroyedVersion;
    public float maxObjectHealth;
    public float currentObjectHealth;
    private bool isDamaged;

    public bool isRock;

	// Use this for initialization
	void Start () {
        currentObjectHealth = maxObjectHealth;
        baseVersion.SetActive(true);
        damagedVersion.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (isDamaged && (currentObjectHealth / maxObjectHealth) <= 0.5 && (currentObjectHealth / maxObjectHealth) > 0) {
            isDamaged = false;
            //Instantiate(damagedVersion, transform.position, transform.rotation);
            baseVersion.SetActive(false);
            damagedVersion.SetActive(true);
        } else if (isDamaged && (currentObjectHealth / maxObjectHealth) <= 0) {
            isDamaged = false;

            if(isRock) {
                destroyedVersion.SetActive(true);
                damagedVersion.SetActive(false);
            } else {
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Crusher") {
            Destroy(gameObject);
        }
    }

    public void takeDamage(float damage) {
        isDamaged = true;
        currentObjectHealth -= damage;
    }

}
