using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear_Script : MonoBehaviour {

    // Disappear Variables
    private bool triggered;
    private bool hasDisappeared;
    public float disappearTime = 1.0f;
    private float currentDisappearTime;

    public float respawnTime = 3.0f;
    private float currentRespawnTime;

    public GameObject disappearTarget;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (triggered) {
            currentDisappearTime += Time.deltaTime;
            if (currentDisappearTime >= disappearTime) {
                disappearTarget.gameObject.SetActive(false);
                triggered = false;
                hasDisappeared = true;
                currentDisappearTime = 0;
            }
        }

        if (hasDisappeared) {
            currentRespawnTime += Time.deltaTime;
            if (currentRespawnTime >= respawnTime) {
                disappearTarget.gameObject.SetActive(true);
                hasDisappeared = false;
                currentRespawnTime = 0;
            }
        }
    }

    public void setTrigger(bool hasTriggered) {
        triggered = hasTriggered;
    }

}
