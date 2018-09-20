using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chaser : MonoBehaviour {

    //Movement
    public float moveSpeed;
    public int direction = 1;
    private float turnTimer = 0;
    public bool isJumping = false;

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
    public GameObject damageHitBox;
    public float attackRange;
    private float attackTimer = 0;
    private bool attacked = false;
    private float stupidAttackLandingTimer = 0;

    //Health Bar Objects
    public int health = 100;
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    public GameObject EnemyHealth;
    private float currentHealthDisTime;
    private bool dead = false;

    //Object Spawn Variables
    private int randomNumberDrop;
    public GameObject healthDrop;

    //Quest Variables
    public bool isQuestEnemy;
    public int questID;

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

        direction = (int)Mathf.Sign(transform.rotation.y);
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        //Health bar update
        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));

        if (GameManager.instance.isTalking == false) {
            //Check if player is visible
            if (playerSeen && direction != (int)Mathf.Sign(player.transform.position.x - transform.position.x) &&
                turnTimer < Time.time) {
                transform.Rotate(new Vector3(0f, 180f, 0f));
                direction = direction * -1;

                turnTimer = Time.time + 0.1f;

            }

            if (platform()) {
                jump();
            }

            if (playerSeen && Vector3.Distance(transform.position, player.transform.position) < attackRange
                && IsGrounded() && attackTimer < Time.time) {
                Vector3 location = (player.transform.position + Vector3.up) - transform.position;
                location = Vector3.Normalize(location);
                rb.AddForce(location * 300);
                isJumping = true;
                GameObject damageBox = Instantiate(damageHitBox, transform.position, transform.rotation);
                damageBox.transform.parent = transform;

                playerSeen = false;
                searchTimer = Time.time + 1f;


                //attacked = true;
                stupidAttackLandingTimer = Time.time + 0.1f;

            }

            if ((noDrop() && !blocked()) || isJumping) {
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
                //print("ITS WORKING");
            }
            else if (!isJumping) {
                transform.Rotate(new Vector3(0f, 180f, 0f));
                direction = direction * -1;
            }
            if (IsGrounded()) {
                isJumping = false;
            }


        }
        

        //Timer updates
        //searchTimer -= Time.deltaTime;
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

        //if (attackTimer > Time.time) playerSeen = false;
    }

    public void OnTriggerEnter(Collider other) {
        if(other.tag == "Falloff") {
            if (isQuestEnemy) {
                GameManager.instance.AddCounter(questID, 1);
            }
            Destroy(this.gameObject);
        } else if (other.tag == "Enemy") {
            transform.Rotate(new Vector3(0f, 180f, 0f));
            direction = direction * -1;
        } 

    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "MovingPlatform") {
            transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "MovingPlatform") {
            transform.parent = null;
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
                if (isQuestEnemy) {
                    GameManager.instance.AddCounter(questID, 1);
                }
                randomDrop();
                //Instantiate(deathSound, transform.position, transform.rotation);
                this.transform.tag = "Untagged";
                Destroy(EnemyHealth.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    private void randomDrop() {
        randomNumberDrop = Random.Range(1, 3);

        if (randomNumberDrop == 1) {
            Instantiate(healthDrop, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        }
    }
}
