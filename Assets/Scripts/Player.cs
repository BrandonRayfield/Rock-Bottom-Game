using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	Animator animator;
	Rigidbody rb;
    public GameObject cameraObject;

    //Time variables
    private float time;
    public float restartTime = 4.0f;

    //Walking
    private float moveSpeed = 2.0f;
	private Quaternion newRotation;

	//Jumping
	public float jumpForce = 200.0f;
	private float distToGround = 1;
	private float jumpCoolDown = 0.5f;
	private float jumpTime;

    // Obstacle Variables
    private bool canPhase;
    private GameObject platformObject;
    private Collider platformCollider;

    //Spawn Variables
    public GameObject startPoint;
    public GameObject spawnPoint;

    //Damage Variables
    public float health;
    public float maxHealth = 100.0f;

    public bool dead = false;
    private float damage = 50.0f;
    private float attackTimer;
    public float attackRate = 0.5f;
	public GameObject damageLocation;
	public GameObject damageHitBox;

    //UI Variables
	public GameObject goldKeyUI;
    public Slider healthBar;
    public Text gameResult;

    //Sound Variables
    public GameObject jumpSound;
    public GameObject attackSound;
    public GameObject deathSound;
    public GameObject keySound;

    //Magic Variables
    public GameObject lightningControl;
    public float magicTimer;
    public float magicRate = 1.5f;
    public GameObject lightningPrefab;

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		distToGround = transform.GetComponent<Collider> ().bounds.extents.y;
		newRotation = transform.rotation;
        health = maxHealth;

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

		if (!dead) {
            Controls();
        }
		else if (dead) {
            animator.Play("Dead");
            gameResult.text = "Game Over.";
            gameResult.enabled = true;
            time += Time.deltaTime;
            if (time > restartTime) {
                gameResult.enabled = false;
                dead = false;
                if (spawnPoint == null) {
                    gameObject.transform.position = startPoint.transform.position;
                    cameraObject.transform.position = startPoint.transform.position;
                    health = maxHealth;
                    time = 0;

                } else {
                    gameObject.transform.position = spawnPoint.transform.position;
                    cameraObject.transform.position = spawnPoint.transform.position;
                    health = maxHealth;
                    time = 0;
                }

                //SceneManager.LoadScene(0); // Return to menu
                //SceneManager.LoadScene(1);
            }
        }


        //Update UI Components
        healthBar.value = health / 100;
    }

	private void Controls(){

		//Jump
		if (Input.GetKeyDown ("space") && IsGrounded () && Time.time > jumpTime) {

			rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Force);
			Instantiate (jumpSound, transform.position, transform.rotation);
			jumpTime = Time.time + jumpCoolDown;
		}
		//attack
		if (Input.GetKeyDown ("f") && Time.time > attackTimer) {

            //animator.Play ("Attack");
            GameObject hitBox = Instantiate(damageHitBox, damageLocation.transform.position, damageLocation.transform.rotation, damageLocation.transform);
            Instantiate(attackSound, transform.position, transform.rotation);
			attackTimer = Time.time + attackRate;
		}
        //Lightning Attack

        //LIGHTNING ATTACK HAS BEEN MOVED TO THUNDER COLLIDER DUE TO ERRORS. MUST BE LOOKED INTO ASAP


        if (Input.GetKey ("s") && canPhase) {
            platformCollider.enabled = false;
        }

		//Movement
		if (Input.GetKey ("d"))
			Walking (1);
		else if (Input.GetKey ("a"))
			Walking (-1);
	}

	private void Walking(int direction){

		if (IsGrounded ())
			animator.Play ("Walk");
		else
			animator.Play ("Idle");

		newRotation.eulerAngles = new Vector3 (newRotation.eulerAngles.x, direction * 90, newRotation.eulerAngles.z); 
		transform.rotation = newRotation;

		if (rb.velocity.x < 35)
			rb.AddForce (new Vector3 (direction * 500 * Time.deltaTime, 0, 0), ForceMode.Force);
	}

	public void Damage(){
		GameObject hitBox = Instantiate (damageHitBox, damageLocation.transform.position, damageLocation.transform.rotation, damageLocation.transform);
        //hitBox.GetComponent<DamageHitBox> ().damage = damage;        
	}

	private bool IsGrounded(){
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}

    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            dead = true;
            Instantiate(deathSound, transform.position, transform.rotation);
        }
    }


	void OnTriggerEnter(Collider otherObject){

		if (otherObject.transform.tag == "GoldKey") {
			GameManager.instance.goldKey = true;
			goldKeyUI.SetActive(true);
			Instantiate (keySound, transform.position, transform.rotation);
			Destroy (otherObject.gameObject);
		}

        if (otherObject.transform.tag == "Falloff") {
            takeDamage(health);
        }

        if (otherObject.transform.tag == "Checkpoint") {
            setCheckPoint(otherObject.gameObject);
        }
	}

    private void OnTriggerStay(Collider other) {
        if (other.tag == "MovingPlatform") {
            transform.parent = other.transform;
        }

        if (other.tag == "GhostPlatform") {
            platformObject = other.gameObject.transform.GetChild(0).gameObject;
            platformCollider = platformObject.GetComponent<Collider>();
            canPhase = true;
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "MovingPlatform") {
            transform.parent = null;
        }

        if (other.tag == "GhostPlatform") {
            platformObject = null;
            platformCollider = null;
            canPhase = false;
        }
    }

    public void setCheckPoint(GameObject spawnlocation) {
        spawnPoint = spawnlocation;
    }

    public GameObject getCheckPoint() {
        return spawnPoint;
    }

    public void setMaxHealth(float moreHealth) {
        maxHealth += moreHealth;
    }

}
