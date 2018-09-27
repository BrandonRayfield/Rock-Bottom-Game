using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flying_Crush_Enemy : MonoBehaviour {

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
    public bool activated = false;

    //health
    public int health = 100;
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    public GameObject EnemyHealth;
    private float currentHealthDisTime;

    //Object Spawn Variables
    private int randomNumberDrop;
    public GameObject healthDrop;

    // Use this for initialization
    void Start() {
        try {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        catch {
            player = null;
        }

        shotTimer = Time.time + Random.Range(0f, 0.75f);

        rb = GetComponent<Rigidbody>();

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
    void Update() {

        if (activated) {
            Movement();
        }
        else {
            Search();
        }

        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));
    }


    public void Search() {
        Vector3 direction = player.transform.position - transform.position;
        direction.z = 0;
        LayerMask mask = 1 << 9;

        if (!Physics.Linecast(transform.position, player.transform.position, mask) &&
            Vector3.Distance(transform.position, player.transform.position) < 7.5f) {

            activated = true;

        }
    }

    public void Movement() {
        if (GameManager.instance.isTalking == false) {
            if (Vector3.Distance(transform.position, swoop_target) <= targetRadius || moveTimer < Time.time) {

                LayerMask mask = 9;

                if (!Physics.Linecast(transform.position, player.transform.position, mask) &&
                    Vector3.Distance(player.transform.position, transform.position) < 5f) {
                    if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 1f &&
                        Mathf.Abs(transform.position.y - player.transform.position.y) <= 5f) {
                        swoop_target = player.transform.position + (player.transform.position - transform.position);
                        swoop_target.x = transform.position.x;
                        moveTimer = Time.time + 0.5f;
                        speed = 250f;
                    }
                    else {
                        swoop_target = new Vector3(player.transform.position.x,
                           Random.Range(-4, 4) + player.transform.position.y, 0f);
                        moveTimer = Time.time + 0.5f;
                        speed = 110f;
                    }

                }
                else {
                    swoop_target = new Vector3(Random.Range(-4, 4) + transform.position.x,
                        Random.Range(-4, 4) + transform.position.y, 0f);
                    moveTimer = Time.time + 0.5f;
                    speed = 110f;
                }

            }
            else {
                MoveTowardsTarget(swoop_target);
            }
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
        if (currentHealthDisTime < Time.time) {
            enemygeneric.health -= damage;

            EnemyHealth.SetActive(true);
            EnemyHealth.GetComponent<Slider>().value = (enemygeneric.health / enemygeneric.maxHealth);

            currentHealthDisTime = Time.time + 0.5f;

            if (enemygeneric.health <= 0) {
                dead = true;
                randomDrop();
                //Instantiate(deathSound, transform.position, transform.rotation);
                this.transform.tag = "Untagged";
                Destroy(EnemyHealth.gameObject);
                Destroy(this.gameObject);
            }
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

            rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider otherObject) {

            if (otherObject.transform.tag == "Player") {
                otherObject.GetComponent<Player>().takeDamage(damage);

            Vector3 direction = swoop_target - transform.position;
            swoop_target = transform.position - direction;
        }
        }
    private void randomDrop() {
        randomNumberDrop = Random.Range(1, 3);

        if (randomNumberDrop == 1) {
            Instantiate(healthDrop, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        }
    }
}
