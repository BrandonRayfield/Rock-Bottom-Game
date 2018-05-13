using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    Animator animator;
    Collider myCollider;

    public GameObject target;

    public float moveSpeed = 1.0f;

    //Damage variables
    private float health = 100.0f;
    private bool dead = false;
    public float damage = 10.0f;
    public GameObject damageHitBox;
    public GameObject damageLocation;
    private float attackRange = 1.1f; //0.75f;
    private float attackTimer;
    private float attackRate = 1.0f;

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
    }

    // Update is called once per frame
    void Update() {

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
            Debug.Log(Vector3.Distance(transform.position, target.transform.position));
			animator.Play ("Walk");

			//look in direction of target
			if (target.transform.position.x < transform.position.x)
				transform.LookAt(patrolPoints[0].transform.position);
			
			else if (target.transform.position.x > transform.position.x)
				transform.LookAt(patrolPoints[1].transform.position);

			//move
			transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed * 2);
					
		} else if (Vector3.Distance (transform.position, target.transform.position) <= attackRange && Time.time > attackTimer) {
            Debug.Log(Vector3.Distance(transform.position, target.transform.position));
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
        Debug.Log("Enemy Attacked");
        hitbox.GetComponent<DamageHitBox>().damage = damage;
        hitbox.GetComponent<DamageHitBox>().player = false;

    }

    public void takeDamage(float damage) {

        health -= damage;
        animator.Play("Damage");
        attackTimer = Time.time + attackRate / 2;

        if (health <= 0) {
            dead = true;
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
    }

}
