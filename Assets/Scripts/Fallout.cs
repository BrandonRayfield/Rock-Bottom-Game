using UnityEngine;
using System.Collections;

public class Fallout : MonoBehaviour {

    public float postionX;
    public float postionY;
    public float postionZ;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "Player") {
            other.transform.position = new Vector3(postionX, postionY, postionZ);
        }
    }

}
