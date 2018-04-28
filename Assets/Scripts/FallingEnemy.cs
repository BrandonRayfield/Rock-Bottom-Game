using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingEnemy : MonoBehaviour {

    public GameObject playerObject;
    public GameObject enemyPatrol;
    public GameObject enemyObject;
    public GameObject enemySpawnPoint;

    private bool enemySpawned = false;
    public float spawnRate;
    private float spawnTimer;

    // Use this for initialization
    void Start() {
        try {
            playerObject = GameObject.Find("Player");
        } catch {
            playerObject = null;
        }
    }

    // Update is called once per frame
    void Update() {




        }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject && !enemySpawned) {
            Instantiate(enemyPatrol, enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation);
            enemyPatrol.GetComponent<Rigidbody>().isKinematic = false;
            enemySpawned = true;
            enemyObject.GetComponent<Enemy>().setIsFalling(true);
        }
    }
}

