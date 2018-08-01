using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Object : MonoBehaviour {

    public GameObject baseVersion;
    public GameObject damagedVersion;
    public GameObject destroyedVersion;
    public float maxObjectHealth;
    public float currentObjectHealth;
    private bool isDamaged;

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
            //Instantiate(destroyedVersion, transform.position, transform.rotation);
            //damagedVersion.SetActive(false);
            Destroy(gameObject);
        }
	}

    public void takeDamage(float damage) {
        isDamaged = true;
        currentObjectHealth -= damage;
    }

}
