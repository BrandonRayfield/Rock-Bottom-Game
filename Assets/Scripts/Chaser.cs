using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chaser : MonoBehaviour {

    //Movement
    public float moveSpeed;
    public int direction = 1;
    private float turnTimer = 0;
    private bool isJumping = false;

    private LayerMask obstruction = 1 << 9;

    //properties
    private Rigidbody rb;
    public EnemyGeneric enemygeneric;

    //Targeting
    public bool playerSeen = false;
    public float searchTimer = 0f;
    public Player player;

    //Attacking
    public Vector3 damageLocation;

    //Health Bar Objects
    public int health = 100;
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    public GameObject EnemyHealth;
    private float currentHealthDisTime;
    private bool dead = false;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player  = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = 100;

        //health bar
        EnemyHealth = Instantiate(EnemyHealthBar);
        EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyHealth.SetActive(false);
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        //Health bar update
        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));

        //Check if player is visible
        if (playerSeen && direction != (int)Mathf.Sign(player.transform.position.x - transform.position.x) &&
            turnTimer < Time.time) {
                transform.Rotate(new Vector3(0f, 180f, 0f));
                direction = direction * -1;

            turnTimer = Time.time + 0.1f;

        }

        if ((noDrop() && !blocked()) || isJumping) {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            //print("ITS WORKING");
        } else if (!isJumping){
            transform.Rotate(new Vector3(0f, 180f, 0f));
            direction = direction * -1;
        }
        if (isJumping && IsGrounded()) isJumping = false;

        if (platform()) {
            jump();
        }


        

        //Timer updates
        searchTimer -= Time.deltaTime;
        if (searchTimer <= Time.time) {
            playerSeen = false;
            search();
        }

	}
    //is there a long drop?
    private bool noDrop() {
        Vector3 middle = transform.position + Vector3.up * 0.25f;
        //Debug lines to show raycasts
        Debug.DrawLine(middle, middle + 0.75f * Vector3.right * direction, Color.red);
        Debug.DrawLine(middle + 0.75f * Vector3.right * direction,
            middle + 0.75f * Vector3.right * direction + 6 * Vector3.down, Color.red);

        Debug.DrawLine(middle, middle + Vector3.right * direction, Color.green);


        if (Physics.Linecast(middle + 0.75f * Vector3.right * direction,
            middle + 0.75f * Vector3.right * direction + 6 * Vector3.down, obstruction)) {
            return true;

            //Is there another platform across the pit
        } else if (Physics.Linecast(middle + 3f * Vector3.right * direction,
            middle + 3f * Vector3.right * direction + 3 * Vector3.down, obstruction)) {
            jump();
            return true;

        } else if (Physics.Linecast(middle + 2f * Vector3.right * direction + Vector3.up,
         middle + 2f * Vector3.right * direction + Vector3.down, obstruction)) {
            jump();
            return true;

        } else {
            return false;
        }
    }
    //is there a wall in the way?
    private bool blocked() {
        Vector3 middle = transform.position + Vector3.up * 0.25f;
        if (!Physics.Linecast(middle + Vector3.up, middle + 0.75f * Vector3.right * direction, obstruction)) {
            return false;
        } else {
            return true;
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position + Vector3.up * 0.25f, -Vector3.up, 0.35f);
    }

    private void jump() {
        if (IsGrounded()) {
            rb.AddForce(Vector3.up * 90);
            isJumping = true;
        }
    }
    //platform to jump up to
    private bool platform() {
        Vector3 middle = transform.position + Vector3.up * 0.25f;

        //Debug draw raycast lines
        Debug.DrawLine(middle, middle + 0.75f * Vector3.up, Color.blue);
        Debug.DrawLine(middle + 0.75f * Vector3.up,
            middle + 0.75f * Vector3.up + Vector3.right * direction * 1.5f, Color.blue);

        Debug.DrawLine(middle, middle + Vector3.right * direction, Color.green);
        //Empty space above
        if (!Physics.Linecast(middle + 0.75f * Vector3.up,
            middle + 0.75f * Vector3.up + Vector3.right * direction * 1.5f, obstruction) &&
            //blocked space in front
            Physics.Linecast(middle + Vector3.up, middle + 1.5f * Vector3.right * direction, obstruction)) {

            return true;
        }
        else {
            return false;
        }
    }

    private void search() {
        if (!Physics.Linecast(transform.position + 0.25f * Vector3.up, player.transform.position, obstruction) &&
            Vector3.Distance(transform.position + 0.25f * Vector3.up, player.transform.position) <= 8f) {

            Debug.DrawLine(transform.position, player.transform.position, Color.yellow);

            playerSeen = true;
            searchTimer = Time.time + 2f;
        } else {
            playerSeen = false;
            searchTimer = Time.time + 0.25f;
        }
        
    }

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Falloff") {
            Destroy(this.gameObject);
        } else if (other.tag == "Enemy") {
            transform.Rotate(new Vector3(0f, 180f, 0f));
            direction = direction * -1;
        }

    }

    public void takeDamage(float damage) {
        if (currentHealthDisTime < Time.time) {
            enemygeneric.health -= damage;

            EnemyHealth.SetActive(true);
            EnemyHealth.GetComponent<Slider>().value = (enemygeneric.health / enemygeneric.maxHealth);

            currentHealthDisTime = Time.time + 0.5f;

            if (enemygeneric.health <= 0) {
                dead = true;
                //Instantiate(deathSound, transform.position, transform.rotation);
                this.transform.tag = "Untagged";
                Destroy(EnemyHealth.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
