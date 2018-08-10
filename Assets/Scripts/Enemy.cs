using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    Animator animator;
    Collider myCollider;
    
    public GameObject target;

    public float moveSpeed = 1.0f;

    //Object Spawn Variables
    private int randomNumberDrop;
    public GameObject healthDrop;

    //Health Bar Objects
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    private GameObject EnemyHealth;
    private bool hasBeenDamaged;
    private float healthBarDisappearTime = 3.0f;
    private float currentHealthDisTime;

    //Damage variables
    public EnemyGeneric enemygeneric;
    private bool dead = false;
    private float damage = 15.0f;
    public GameObject damageHitBox;
    public GameObject damageLocation;
    private float attackRange = 1.1f; //0.75f;
    private float attackTimer;
    private float attackRate = 1.0f;

    // Crusher Variables
    private bool touchTopCrush;
    private bool touchBottomCrush;

    //Patrolling Variables
    public float patrolPointDistance = 0.1f;
    public GameObject[] patrolPoints;
    private int currentPatrolPoint = 0;
    public bool isFalling = false;
    public GameObject patrolArea;

    //Sound Variables
    public GameObject attackSound;
    public GameObject deathSound;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = 100;
        enemygeneric.maxHealth = enemygeneric.health;

        EnemyHealth = Instantiate(EnemyHealthBar);
        EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyHealth.SetActive(false);
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }

    // Update is called once per frame
    void Update() {

        if(hasBeenDamaged) {
            currentHealthDisTime += Time.deltaTime;
            if (currentHealthDisTime >= healthBarDisappearTime) {
                EnemyHealth.SetActive(false);
                hasBeenDamaged = false;
                currentHealthDisTime = 0;
            }
        }

        //Updating position of enemy healthbar

        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3 (healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));

        if (touchTopCrush && touchBottomCrush) {
            takeDamage(enemygeneric.health);
            touchTopCrush = false;
            touchBottomCrush = false;
        }

        //Enemy Logic
        if (!dead) { 
            if (!target && !isFalling)
                Patrol();
            else if (target)
                Attack();
        } else if (dead) {
            animator.Play("Dead");
            myCollider.enabled = false;
        }
        

        //Target death check
        if (target != null && target.GetComponent<Player>().dead)
            target = null;
    }

    public void setIsFalling(bool isFall) {
        isFalling = isFall;
    }

	private void Attack(){

		//Get in range
		if (Vector3.Distance (transform.position, target.transform.position) > attackRange) {
			animator.Play ("Walk");

			//look in direction of target
			if (target.transform.position.x < transform.position.x)
				transform.LookAt(patrolPoints[0].transform.position);
			
			else if (target.transform.position.x > transform.position.x)
				transform.LookAt(patrolPoints[1].transform.position);

			//move
			transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed * 2);
					
		} else if (Vector3.Distance (transform.position, target.transform.position) <= attackRange && Time.time > attackTimer) {
            animator.Play("Attack");
            Instantiate(attackSound, transform.position, transform.rotation);
            attackTimer = Time.time + attackRate;
		}
	}

	private void Patrol(){

		//Snap rotate 
		transform.LookAt(patrolPoints[currentPatrolPoint].transform.position);

		animator.Play ("Walk");

		//Move
		transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);

		//close
		if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].transform.position) < patrolPointDistance){
			if (currentPatrolPoint == patrolPoints.Length - 1)
				currentPatrolPoint = 0;
			else
				currentPatrolPoint += 1;
        }
    }

    public void Damage() {
        GameObject hitbox = Instantiate(damageHitBox, damageLocation.transform.position, damageLocation.transform.rotation) as GameObject;
        hitbox.GetComponent<DamageHitBox>().damage = damage;
        hitbox.GetComponent<DamageHitBox>().player = false;

    }

    public void takeDamage(float damage) {

        Debug.Log("Enemy Took Damage: " + damage);

        currentHealthDisTime = 0;
        hasBeenDamaged = true;

        enemygeneric.health -= damage;
        animator.Play("Damage");
        Instantiate(attackSound, transform.position, transform.rotation);
        attackTimer = Time.time + attackRate / 2;

        EnemyHealth.SetActive(true);
        Debug.Log("Enemy health: " + enemygeneric.health);
        Debug.Log("Enemy Max health: " + enemygeneric.maxHealth);
        EnemyHealth.GetComponent<Slider>().value = (enemygeneric.health / enemygeneric.maxHealth);

        if (enemygeneric.health <= 0) {
            dead = true;
            randomDrop();
            Instantiate(deathSound, transform.position, transform.rotation);
            this.transform.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isFalling && other.tag == "Shadow") {
            Debug.Log("Enemy Touched Object");
            patrolArea.GetComponent<Rigidbody>().isKinematic = true;
            isFalling = false;
            Destroy(other.gameObject);
        }

        if (other.transform.tag == "Falloff") {
            takeDamage(enemygeneric.health);
        }

        if (other.transform.tag == "Crusher") {
            touchTopCrush = true;
        }

        if (other.transform.tag == "CrusherFloor") {
            touchBottomCrush = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Crusher") {
            touchTopCrush = false;
        }

        if (other.transform.tag == "CrusherFloor") {
            touchBottomCrush = false;
        }
    }

    private void randomDrop() {
        randomNumberDrop = Random.Range(1, 3);

        if (randomNumberDrop == 1) {
            Instantiate(healthDrop, new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        }
    }

}
