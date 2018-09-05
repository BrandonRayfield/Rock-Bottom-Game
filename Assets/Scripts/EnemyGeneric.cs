using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour {

    public float health;
    public float maxHealth;

    private Rigidbody rb;

    //Lightning Tracking
    public int lightningListLocation = -1;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F6)) {
            CopyrightNeutralBoop(GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>().transform.position);
        }
    }


    public void LightningChange(int newlocation) {
        lightningListLocation = newlocation;
    }

    public void CopyrightNeutralBoop(Vector3 playerLocation) {
        int direction = (int)Mathf.Sign(transform.position.x - playerLocation.x);

        Vector3 angle = new Vector3(direction * 5f, 2f, 0f);
        angle = Vector3.Normalize(angle);
        rb.AddForce( angle * 500);
    }

}
