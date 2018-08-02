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
    private float speed = 150;
    private Vector3 direction;
    private Rigidbody rb;

    public EnemyGeneric enemygeneric;

    private bool dead = false;
    public float damage = 10.0f;

    //Lightning Tracking
    public int lightningListLocation = -1;

    //Swoop Variables
    public Vector3 swoop_target;
    private float swoop_c;
    public float targetRadius;
    public float rotationSpeed;

    // Use this for initialization
    void Start() {
        try {
            player = GameObject.Find("Player").GetComponent<Player>();
        } catch {
            player = null;
        }

        shotTimer = Time.time + Random.Range(0f, 0.75f);

        rb = GetComponent<Rigidbody>();

        Movement();

        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = 100;

        swoop_target = new Vector3(Random.Range(-4, 4) + transform.position.x,
                Random.Range(-4, 4) + transform.position.y, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        Search();

        Movement();
	}

    public void Search() {
        Vector3 direction = player.transform.position - transform.position;
        direction.z = 0;
        LayerMask mask = 9;

        if (!Physics.Linecast(transform.position, player.transform.position, mask)) {

            //Debug.DrawLine(transform.position, player.transform.position, Color.blue);
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

        if (Vector3.Distance(transform.position, swoop_target) <= targetRadius || moveTimer < Time.time) {

            LayerMask mask = 9;
            
            if (!Physics.Linecast(transform.position, player.transform.position, mask)) {
                if (Mathf.Abs(transform.position.y - player.transform.position.y) <= 1f) {
                    swoop_target = player.transform.position + (player.transform.position - transform.position);
                    swoop_target.y = transform.position.y;
                    moveTimer = Time.time + 0.5f;
                    speed = 500f;
                    print("working boi");
                }
                else {
                    swoop_target = new Vector3(Random.Range(-4, 4) + player.transform.position.x,
                        player.transform.position.y, 0f);
                    moveTimer = Time.time + 0.5f;
                    speed = 150f;
                }

            } else {
                swoop_target = new Vector3(Random.Range(-4, 4) + transform.position.x,
                    player.transform.position.y, 0f);
                moveTimer = Time.time + 0.5f;
                speed = 150f;
            }
                
        } else {
            MoveTowardsTarget(swoop_target);
        }
    }

    public void swoop_variables() {
        //function: y = (x^4)/c
        //y & x = enemy
        float x = transform.position.x - player.transform.position.x;
        float y = transform.position.y - player.transform.position.y;

        swoop_target = player.transform.position;

        swoop_c = (Mathf.Pow(x, 4) / y);
    }

    public void swoop_update() {

    }

    public void takeDamage(float damage) {

        enemygeneric.health -= damage;

        if (enemygeneric.health <= 0) {
            dead = true;
            //Instantiate(deathSound, transform.position, transform.rotation);
            this.transform.tag = "Untagged";
            Destroy(this.gameObject);
        }
    }

    public void LightningChange(int newlocation) {
        lightningListLocation = newlocation;
    }

    private void MoveTowardsTarget(Vector3 targetPos) {
        //Rotate and move towards target if out of range
        if (Vector3.Distance(targetPos, transform.position) > targetRadius) {

            //Lerp Towards target
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
            float adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

            rb.AddRelativeForce(Vector3.forward * speed  * Time.deltaTime);
        }
    }
}
