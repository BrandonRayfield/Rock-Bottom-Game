using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	Animator animator;
	Rigidbody rb;

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

    //Damage Variables
    public float health = 100.0f;
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

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();

		distToGround = transform.GetComponent<Collider> ().bounds.extents.y;

		newRotation = transform.rotation;
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
                //SceneManager.LoadScene(0); // Return to menu
                SceneManager.LoadScene(1);
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

}
