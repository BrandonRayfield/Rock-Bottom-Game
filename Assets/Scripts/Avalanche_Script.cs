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

    // Use this for initialization
    void Start () {
        baseSpawnHeight = gameObject.transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time > objectSpawnTime) {

            for(int i = 0; i < amountSpawned; i++) {
                randomPosition = Random.Range(-spawnAreaRange, spawnAreaRange);
                randomHeight = Random.Range(0, 10);

                xPosition = gameObject.transform.position.x + randomPosition;
                //newRandomHeight = baseSpawnHeight + randomHeight;
                newRandomHeight = baseSpawnHeight;

                Instantiate(fallingObject, new Vector3(xPosition, newRandomHeight, 0), Quaternion.identity);
            }

            objectSpawnTime = Time.time + objectSpawnRate; 
        }
	}
}
