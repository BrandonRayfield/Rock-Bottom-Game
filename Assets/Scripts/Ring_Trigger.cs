using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring_Trigger : MonoBehaviour {

    // Type variables
    public bool isMusicChange;
    public bool isEnemySpawn;
    public bool isDamage;
    public bool isElevatorTrigger;

    // Limit variable
    private bool isTriggered;

    // Elevator variables
    public GameObject elevatorObject;
    public GameObject elevatorTrigger;

    // Fire variables
    public float damage;
    private float currentDisableTime;
    public float disableTime;

    // Spawn Variables
    public GameObject enemyObject;
    public GameObject[] spawnLocation;
    private bool hasSpawned;

    // Music Variables
    public GameObject cameraObject;
    public AudioClip countryMusic;
    public AudioClip rockMusic;

    public GameObject newCheckpoint;
    private string restartAnimationName;
    private string movementAnimationName;



    // Use this for initialization
    void Start () {
        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        currentDisableTime = disableTime;

        movementAnimationName = "Going_Down";
        restartAnimationName = "Reset";

    }
	
	// Update is called once per frame
	void Update () {
        if(isDamage && isTriggered) {
            currentDisableTime += Time.deltaTime;
            if(currentDisableTime >= disableTime) {
                isTriggered = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            if(isElevatorTrigger && !isTriggered) {
                elevatorObject.GetComponent<Animator>().Play(movementAnimationName);
                isTriggered = true;
            } else if(isDamage && !isTriggered) {
                other.GetComponent<Player>().takeDamage(damage);
                isTriggered = true;
                currentDisableTime = 0;
            } else if (isEnemySpawn && !isTriggered) {
                foreach (GameObject spawnPoint in spawnLocation) {
                    Instantiate(enemyObject, spawnPoint.transform.position, spawnPoint.transform.rotation);
                }
                isTriggered = true;
            } else if (isMusicChange) {
                other.GetComponent<Player>().setCheckPoint(newCheckpoint);
                elevatorTrigger.GetComponent<Ring_Trigger>().setMovementAnimationName("Going_Down2");
                other.GetComponent<Player>().setTransitionAnimationName("Reset2");
                cameraObject.GetComponent<AudioSource>().Stop();
                cameraObject.GetComponent<AudioSource>().clip = rockMusic;
                cameraObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (isMusicChange) {
            cameraObject.GetComponent<AudioSource>().Stop();
            cameraObject.GetComponent<AudioSource>().clip = countryMusic;
            cameraObject.GetComponent<AudioSource>().Play();
        }
    }

    public void SetIsTriggered(bool triggered) {
        isTriggered = triggered;
    }

    public void setMovementAnimationName(string newName) {
        movementAnimationName = newName;
    }

}
