using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying_Enemy : MonoBehaviour {

    public Player player;
    public Projectile shot;

    public float shotRate = 1.5f;
    public float shotTimer = 0f;

    private float moveTimer;
    private float moveTime;
    private float speed = 20f;
    private Vector3 direction;
    private Rigidbody rb;

    private float health = 100.0f;
    private bool dead = false;
    public float damage = 10.0f;
    // Use this for initialization
    void Start () {
        shotTimer = Time.time + Random.Range(0f,0.75f);

        rb = GetComponent<Rigidbody>();

        Movement();
	}
	
	// Update is called once per frame
	void Update () {
        Search();

        if (Time.time > moveTimer) Movement();
	}

    public void Search() {
        Vector3 direction = player.transform.position - transform.position;
        direction.z = 0;
        LayerMask mask = 9;

        if (!Physics.Linecast(transform.position, player.transform.position, mask)) {

            Debug.DrawLine(transform.position, player.transform.position, Color.blue);
            Vector3 angle = player.transform.position - transform.position;

            transform.rotation = Quaternion.Euler(angle);

            if (Time.time > shotTimer) {
                Projectile attack = Instantiate(shot, transform.position, Quaternion.Euler(direction));
                attack.player = player;

                shotTimer = Time.time + shotRate;
            }
        }
    }

    public void Movement() {

        moveTime = Random.Range(3f, 7f);
        moveTimer = Time.time + moveTime;

        Vector3 temp = rb.velocity;
        temp = randomizeDirection();
        rb.velocity = temp;
    }

    public Vector3 randomizeDirection() {
        float x = Random.Range(-10, 10);
        float y = Random.Range(-10, 10);

        float mag = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

        x = speed * Time.deltaTime * x / mag;
        y = speed * Time.deltaTime * y / mag;

        return new Vector3(x, y, 0);
    }

    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            dead = true;
            //Instantiate(deathSound, transform.position, transform.rotation);
            this.transform.tag = "Untagged";
            Destroy(this.gameObject);
        }
    }
}
