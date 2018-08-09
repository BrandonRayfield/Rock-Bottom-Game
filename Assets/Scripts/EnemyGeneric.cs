using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour {

    public float health;
    public float maxHealth;


    //Lightning Tracking
    public int lightningListLocation = -1;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void LightningChange(int newlocation) {
        lightningListLocation = newlocation;
    }

}
