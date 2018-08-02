using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable_Object_Dialogue : MonoBehaviour {

	//Dailog stuff
	public Dialogue[] dialogue;
	public bool canTalk;
	public bool isTalking;

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
		isTalking = FindObjectOfType<DialogueManager>().getIsTalking();

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

			//Dailog stuff

			if(!isTalking) {
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
			} else {
				FindObjectOfType<DialogueManager>().DisplayNextSentence();
				}
				
			if(!canTalk) {
				FindObjectOfType<DialogueManager>().EndDialogue();
				isTalking = false;
			}

        }
	}

    public void takeDamage(float damage) {
        isDamaged = true;
        currentObjectHealth -= damage;
    }

	public void TriggerDialogue() {
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			canTalk = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			canTalk = false;
		}
	}

}
