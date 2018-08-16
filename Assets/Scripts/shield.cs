using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour {

    public float deathTimer;
    public Player player;
	// Use this for initialization
	void Start () {
        deathTimer = 5f;
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0f) {
            player.invulnerable = false;
            Destroy(this.gameObject);
        }
	}
}
