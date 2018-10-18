using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Ghost : MonoBehaviour {

    public GameObject playerObject;
    public GameObject parentObject;

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
        if (other.gameObject == playerObject) {
            parentObject.GetComponent<Disappear_Script>().setTrigger(true);
        }
    }
}
