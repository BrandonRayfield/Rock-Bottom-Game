using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Large_Spider : MonoBehaviour {

    //properties
    private Rigidbody rb;
    public EnemyGeneric enemygeneric;
    public Player player;

    //Health Bar Objects
    public int health = 100;
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    public GameObject EnemyHealth;
    private float currentHealthDisTime;
    private bool dead = false;

    //Movement
    private int direction = 1;
    private float turnTimer = 0f;
    public float movementSpeed;

    //Shooting
    public GameObject projectile;
    private float shootTimer = 0f;
    private int shots = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();

        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = 150;

        //health bar
        EnemyHealth = Instantiate(EnemyHealthBar);
        EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyHealth.SetActive(false);

        direction = (int)Mathf.Sign(transform.rotation.y);
        turnTimer = Time.time + 5;
        shootTimer = Time.time + 12f;
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }

    // Update is called once per frame
    void Update () {
        //Health bar update
        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));


        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);

        if (turnTimer < Time.time) {
            transform.Rotate(new Vector3(0f, 180f, 0f));
            turnTimer = Time.time + 5f;
        }

        if (shootTimer < Time.time ) {
            GameObject shot = Instantiate(projectile, transform.position + Vector3.up, transform.rotation);
            if (shots < 3 ) {
                shots++;
                shootTimer = Time.time + 1f;
            } else {
                shots = 0;
                shootTimer = Time.time + 10f;
            }
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
