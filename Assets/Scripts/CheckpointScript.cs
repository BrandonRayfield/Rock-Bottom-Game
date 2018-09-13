using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointScript : MonoBehaviour {

    private bool checkpointActive;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player" && !checkpointActive) {
            checkpointActive = true;
            other.GetComponent<Player>().gainHealth(100);
        }
    }
}
