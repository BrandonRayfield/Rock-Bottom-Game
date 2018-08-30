using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avalanche_Script : MonoBehaviour {

    public GameObject fallingObject;
    public int amountSpawned;
    public float objectLifeTime = 3.0f;
    public float objectSpawnRate = 2.0f;
    public int spawnAreaRange = 2;
    private float baseSpawnHeight;
    private int randomHeight;

    private float objectSpawnTime;
    private int randomPosition;
    private float xPosition;
    private float newRandomHeight;

    // Guitar Abiltity 2 Variables 
    private bool disabled;
    private bool canTrigger;
    private float amountFalling;
    private float resetTime = 8.0f;

    // Use this for initialization
    void Start () {
        baseSpawnHeight = gameObject.transform.position.y;
        amountFalling = spawnAreaRange * 2;
        canTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > objectSpawnTime) {

            for(int i = 0; i < amountSpawned; i++) {
                randomPosition = Random.Range(-spawnAreaRange, spawnAreaRange);
                //randomHeight = Random.Range(0, 10);

                xPosition = gameObject.transform.position.x + randomPosition;
                //newRandomHeight = baseSpawnHeight + randomHeight;
                newRandomHeight = baseSpawnHeight;

                Instantiate(fallingObject, new Vector3(xPosition, newRandomHeight, 0), Quaternion.identity);
            }

            objectSpawnTime = Time.time + objectSpawnRate; 
        }

        if(disabled) {
            for (int i = 0; i < amountFalling; i++) {

                xPosition = gameObject.transform.position.x + (-spawnAreaRange + i);
                //newRandomHeight = baseSpawnHeight + randomHeight;
                newRandomHeight = baseSpawnHeight;

                if(i % 2 == 0) {
                    Instantiate(fallingObject, new Vector3(xPosition, newRandomHeight, 0), Quaternion.identity);
                }

            }
            objectSpawnTime = Time.time + resetTime;
            disabled = false;
            Invoke("setCanTriggerTrue", resetTime);
        }
    }

    private void setCanTriggerTrue() {
        canTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if(canTrigger && other.gameObject.tag == "Ability2") {
            disabled = true;
            canTrigger = false;
        }
    }

}
