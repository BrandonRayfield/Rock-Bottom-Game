using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_crush_projectile : MonoBehaviour {

    private float lifeTime;

	// Use this for initialization
	void Start () {
		
	}

    void Awake() {
        lifeTime = Time.time + 2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > lifeTime) Destroy(this.gameObject);
	}
}
