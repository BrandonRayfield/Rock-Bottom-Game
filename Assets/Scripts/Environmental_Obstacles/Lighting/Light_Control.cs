using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Control : MonoBehaviour {

    public Light mainLight;
    public bool isDark;
    public int dimSpeed = 5;

    private float maxIntensity = 0.5f;
    private float minIntensity = 0.0f;
    public float currentIntensity;

	// Use this for initialization
	void Start () {
        currentIntensity = maxIntensity;
	}
	
	// Update is called once per frame
	void Update () {

        mainLight.intensity = currentIntensity;

        if (isDark) {
            if (currentIntensity > minIntensity) {
                currentIntensity -= (Time.deltaTime / dimSpeed);
            }
        } else {
            if (currentIntensity < maxIntensity) {
                currentIntensity += (Time.deltaTime / dimSpeed);
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            isDark = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            isDark = false;
        }
    }

}
