﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public bool isQuestObject;

    public GameObject targetManager;
    public GameObject targetObject;

    private bool isTriggered, hasUpdated;

    public GameObject triggerSound;

    private Renderer objectRender;

    private int questID;

    // Use this for initialization
    void Start () {
        objectRender = targetObject.GetComponent<Renderer>();
        objectRender.material.color = Color.red;
        if (isQuestObject) {
            questID = targetManager.GetComponent<Target_Manager>().getQuestID();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "projectile") {
            if (!isTriggered) {
                isTriggered = true;
                if(isQuestObject) {
                    GameManager.instance.AcceptQuest(questID);
                    targetManager.GetComponent<Target_Manager>().setHasUpdated(true);
                }
                Instantiate(triggerSound, transform.position, transform.rotation);
                objectRender.material.color = Color.green;
                Destroy(other.gameObject);
            } else {
                Instantiate(triggerSound, transform.position, transform.rotation);
                Destroy(other.gameObject);
            }
        }
    }

    public bool getIsTriggered() {
        return isTriggered;
    }

    public bool getHasUpdated() {
        return hasUpdated;
    }
}