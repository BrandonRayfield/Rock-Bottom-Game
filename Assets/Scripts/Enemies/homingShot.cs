using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingShot : MonoBehaviour {

    public Player player;
    public float rotationSpeed = 12;
    public Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        float adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime * 1.5f, 1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

        rb.AddRelativeForce(Vector3.forward * 50 * Time.deltaTime);
    }
}
