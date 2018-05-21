using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Enemy : MonoBehaviour {

    public Player player;
    public Projectile shot;

    public float shotRate = 1.5f;
    public float shotTimer = 0f;
	// Use this for initialization
	void Start () {
        shotTimer = Time.time + Random.Range(0f,0.75f);
	}
	
	// Update is called once per frame
	void Update () {
        Search();
	}

    public void Search() {
        Vector3 direction = player.transform.position - transform.position;
        LayerMask mask = 8;

        if (!Physics.Linecast(transform.position, player.transform.position, mask) && Time.time > shotTimer) {
            Projectile attack = Instantiate(shot, transform.position, Quaternion.Euler(direction));
            attack.player = player;   

            shotTimer = Time.time + shotRate;
        }
    }
}
