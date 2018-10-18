using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranged_Enemy : MonoBehaviour {

    // Shooting gameobjects
    public GameObject projectile;
    public GameObject attackZone;
    public GameObject shootLocation;
    public GameObject rayCastTarget;

    // Damage Variables
    public float damage;
    public float shootDelay;
    public float bulletSpeed;
    public float fireRate;
    public float currentTime;

    // Player Object
    private GameObject playerObject;

    // Health variables
    public EnemyGeneric enemygeneric;
    private bool dead = false;

    //Health Bar Objects
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    private GameObject EnemyHealth;
    private bool hasBeenDamaged;
    private float healthBarDisappearTime = 3.0f;
    private float currentHealthDisTime;
    public float health = 100;

    //Sound Variables
    public GameObject attackSound;
    public GameObject hurtSound;
    public GameObject deathSound;



    // Bools to check if player is in range
    private bool isPlayerNear;


    // Use this for initialization
    void Start() {
        currentTime = Time.time + fireRate;

        // Enemy Generic Setup (will need to rework this)
        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = health;
        enemygeneric.maxHealth = enemygeneric.health;

        // Healthbar Setup
        EnemyHealth = Instantiate(EnemyHealthBar);
        EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        EnemyHealth.SetActive(false);

        try {
            playerObject = GameObject.Find("Player");
        }
        catch {
            playerObject = null;
        }
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }

    // Update is called once per frame
    void Update() {

        // Make healthbar disappear after set time
        if (hasBeenDamaged)
        {
            currentHealthDisTime += Time.deltaTime;
            if (currentHealthDisTime >= healthBarDisappearTime)
            {
                EnemyHealth.SetActive(false);
                hasBeenDamaged = false;
                currentHealthDisTime = 0;
            }
        }

        //Updating position of enemy healthbar
        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));

        if(!dead) {
            shootLocation.transform.LookAt(playerObject.transform);
            shootLocation.transform.right = (playerObject.transform.position - transform.position).normalized;

            if (isPlayerNear)
            {
                if (Time.time > currentTime)
                {

                    Vector3 relativePos = playerObject.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(playerObject.transform.position);

                    GameObject bullet = Instantiate(projectile, shootLocation.transform.position, shootLocation.transform.rotation);

                    bullet.GetComponent<Projectile_Script>().SetDamage(damage);
                    bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);

                    if (Vector3.Distance(playerObject.transform.position, transform.position) < 12f)
                        Instantiate(attackSound, transform.position, transform.rotation);

                    currentTime = Time.time + fireRate;
                }
            }
        }
        

        //Target death check
        if (playerObject != null && playerObject.GetComponent<Player>().dead) {
            isPlayerNear = false;
        }
    }

    public void takeDamage(float damage) {

        Debug.Log("Enemy Took Damage: " + damage);

        currentHealthDisTime = 0;
        hasBeenDamaged = true;

        enemygeneric.health -= damage;
        //animator.Play("Damage");
        Instantiate(hurtSound, transform.position, transform.rotation);
        //currentTime = Time.time + fireRate / 2;

        EnemyHealth.SetActive(true);
        Debug.Log("Enemy health: " + enemygeneric.health);
        Debug.Log("Enemy Max health: " + enemygeneric.maxHealth);
        EnemyHealth.GetComponent<Slider>().value = (enemygeneric.health / enemygeneric.maxHealth);

        if (enemygeneric.health <= 0) {
            dead = true;
            Instantiate(deathSound, transform.position, transform.rotation);
            this.transform.tag = "Untagged";
            //gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    public void setIsPlayerNear(bool isNear) {
        isPlayerNear = isNear;
    }

    public void updateCurrentTime() {
        currentTime = Time.time + fireRate;
    }

}
