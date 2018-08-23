using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Control : MonoBehaviour {

    public Light mainLight;
    public GameObject mainCamera;
    public bool isDark;
    public int dimSpeed = 5;

    private float maxIntensity = 0.5f;
    private float minIntensity = 0.0f;
    public float currentIntensity;

    private float maxVolume;
    private float minVolume;
    public float currentVolume;

    // Use this for initialization
    void Start () {
        try {
            mainCamera = GameObject.Find("Camera");
        } catch {
            mainCamera = null;
        }

        maxVolume = mainCamera.GetComponent<AudioSource>().volume;
        minVolume = maxVolume / 2;
        currentIntensity = maxIntensity;
        currentVolume = maxVolume;
    }
	
	// Update is called once per frame
	void Update () {

        mainLight.intensity = currentIntensity;
        mainCamera.GetComponent<AudioSource>().volume = currentVolume;

        if (isDark) {
            currentVolume = minVolume;
            if (currentIntensity > minIntensity) {
                currentIntensity -= (Time.deltaTime / dimSpeed);
            }
        } else {
            currentVolume = maxVolume;
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
