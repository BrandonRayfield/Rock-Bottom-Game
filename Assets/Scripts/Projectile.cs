using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Player player;
    public float movementSpeed = 100f;
    public Rigidbody rb;
	// Use this for initialization
	void Start () {
        Vector3 direction = player.transform.position - transform.position;
        float magnatude = Mathf.Pow(Mathf.Sqrt(Mathf.Abs(direction.x)) + Mathf.Sqrt(Mathf.Abs(direction.y)), 2);
        print(magnatude.ToString());
        rb.AddForce(new Vector3((direction.x) * movementSpeed/ magnatude, direction.y*movementSpeed / magnatude, 0), ForceMode.Force);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter( Collider other) {

        if (other.tag == "Wall") Destroy(this.gameObject);
    }
}
