using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch_Script : MonoBehaviour {

    public GameObject fireObject;
    public Light fireLight;

    private bool isLit, isDying, isDead;

    public float dimSpeed = 5;
    public float lightSpeed = 1;

    public float fireLifeTime = 8.0f;
    private float currentTime;

    public GameObject lightSound;
    public GameObject fizzleSound;

    private float maxIntensity = 12.0f;
    private float dieIntensity = 5.0f;
    private float minIntensity = 0.0f;
    public float currentIntensity;

    // Use this for initialization
    void Start () {
        currentIntensity = 0;
        isDead = true;
	}
	
	// Update is called once per frame
	void Update () {
        fireLight.intensity = currentIntensity;

        if (isLit) {
            if (currentIntensity < maxIntensity) {
                currentIntensity += (Time.deltaTime / lightSpeed);
                isDying = true;
            }
        } else {
            if (currentIntensity > minIntensity) {
                currentIntensity -= (Time.deltaTime / dimSpeed);
            }
        }

        if(!isLit && currentIntensity <= dieIntensity && !isDead) {
            fireObject.SetActive(false);
            Instantiate(fizzleSound, transform.position, transform.rotation);
            isDead = true;
        }

        if(isDying) {
            currentTime += Time.deltaTime;
            if (currentTime >= fireLifeTime) {
                isLit = false;
                currentTime = 0;
            }
        }

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "projectile") {
            LightTorch();
            Destroy(other.gameObject);
        }
    }

    public void LightTorch() {
        if (!isLit) {
            Instantiate(lightSound, transform.position, transform.rotation);
        }
        isLit = true;
        isDead = false;
        currentTime = 0;
        fireObject.SetActive(true);
    }

}
