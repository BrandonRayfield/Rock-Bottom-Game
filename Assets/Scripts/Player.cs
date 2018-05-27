using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	Animator animator;
	Rigidbody rb;
    public GameObject cameraObject;
    public GameObject playerModel;

    //Guitar Objects
    public GameObject guitarBack;
    public GameObject guitarFront;
    public GameObject guitarSwing;

    private int guitarStance; // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing


    //Time variables
    private float time;
    public float restartTime = 4.0f;

    //Walking
    private float moveSpeed = 2.0f;
	private Quaternion newRotation;
    private int currentDirection;
    public int movementSpeed;
    private int walkSpeed = 300;
    private int runSpeed = 600;

    //Running
    private bool isRunning;

	//Jumping
	public float jumpForce = 200.0f;
	private float distToGround = 1;
	private float jumpCoolDown = 0.5f;
	private float jumpTime;

    // Obstacle Variables
    private bool canPhase;
    private GameObject platformObject;
    private Collider platformCollider;

    // Cutscene Variables
    private bool isCutscene = false;

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

    // Obstacle Variables
    private bool touchTopCrush;
    private bool touchBottomCrush;

    //UI Variables
    public GameObject goldKeyUI;
    public Slider healthBar;
    public Text gameResult;
    public Text shardText;
    public Text currencyText;

    //Item Variables
    public int shardsNeeded = 3;
    public int currencyCount;
    public int shardCount;
    private int itemValue;
    private int itemID;

    //Sound Variables
    public GameObject jumpSound;
    public GameObject attackSound;
    public GameObject deathSound;
    public GameObject keySound;
    public GameObject musicNoteSound;
    public bool randomPickupPitch; // Toggle on and off to determine if pitch is random or scaling
    public GameObject shardSound;
    public GameObject burnSound;
    public GameObject guitarSound;
    public GameObject squashSound;
    public GameObject checkpointSound;

    

    private float minPitch = 0.5f;
    private float maxPitch = 1.5f;
    private float currentPitch;

    //Magic Variables
    public GameObject lightningControl;
<<<<<<< HEAD
    public GameObject lightningHitbox;
    public GameObject lightningParticles;
=======
>>>>>>> 4919591e33ba0c355a3a111cee3bd576dc95cf8e
    public float magicTimer;
    public float magicRate = 1.5f;
    public GameObject lightningPrefab;
    public float currentChannelTime;
    private float channelTime = 1.0f;
    public bool channelAbility;
    public Vector3 target;
    private Vector3 nullTarget = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		distToGround = transform.GetComponent<Collider> ().bounds.extents.y;
		newRotation = transform.rotation;
        health = maxHealth;
        currentDirection = 1;

        currentPitch = minPitch;

        guitarStance = 0; // Back guitar starts enabled

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

    }
	
	// Update is called once per frame
	void Update () {

        // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing
        if (guitarStance == 1) {
            guitarBack.SetActive(false);
            guitarFront.SetActive(true);
            guitarSwing.SetActive(false);
        } else if (guitarStance == 2) {
            guitarBack.SetActive(false);
            guitarFront.SetActive(false);
            guitarSwing.SetActive(true);
        } else {
            guitarBack.SetActive(true);
            guitarFront.SetActive(false);
            guitarSwing.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

        if (touchTopCrush && touchBottomCrush) {
            takeDamage(health);
            Instantiate(squashSound, transform.position, transform.rotation);
            touchTopCrush = false;
            touchBottomCrush = false;
        }

		if (!dead && !isCutscene) {
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

    private void Controls() {

		//Jump
		if (Input.GetKeyDown ("space") && IsGrounded () && Time.time > jumpTime) {

            rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Force);
			Instantiate (jumpSound, transform.position, transform.rotation);
			jumpTime = Time.time + jumpCoolDown;
            
        }

        if (!IsGrounded()) {
            animator.Play("Jump");
            animator.SetBool("isJumping", true);
        } else {
            animator.SetBool("isJumping", false);
        }

        //attack
<<<<<<< HEAD
        if (Input.GetMouseButtonDown(0) && Time.time > attackTimer) {
            guitarStance = 2;
            animator.Play("Attack");
=======
        if (Input.GetKeyDown ("f") && Time.time > attackTimer) {

            animator.Play ("Attack");
>>>>>>> 4919591e33ba0c355a3a111cee3bd576dc95cf8e
            Instantiate(attackSound, transform.position, transform.rotation);
			attackTimer = Time.time + attackRate;
		}
        //Lightning Attack
        if (Input.GetKeyDown("r") && Time.time > magicTimer) {

            target = lightningControl.GetComponent<ThunderCollider>().findClosest();

            if (target != nullTarget) {
                playerModel.transform.localEulerAngles = new Vector3(0, currentDirection * 90, 0);
                animator.Play("Guitar Playing");
                guitarStance = 1;
                Instantiate(guitarSound, transform.position, transform.rotation);
                // Channel Ability
                channelAbility = true;
                magicTimer = Time.time + magicRate;
            }
        }

        if (channelAbility) {
            currentChannelTime += Time.deltaTime;
            if (currentChannelTime >= channelTime) {
                target = lightningControl.GetComponent<ThunderCollider>().findClosest();
                target.y += 8.5f;
                lightningAttack(target);
                currentChannelTime = 0;
                playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
                channelAbility = false;
                guitarStance = 0;
            }
        } else {
            playerModel.transform.localEulerAngles = new Vector3(0, 0, 0);
            currentChannelTime = 0;
            channelAbility = false;
        }

        if (Input.GetKey ("s") && canPhase) {
            platformCollider.enabled = false;
        }

		//Movement

        if (Input.GetKey("left shift")) {
            isRunning = true;     
        } else {
            isRunning = false;     
        }

        if (Input.GetKey("d")) {
            Walking(1);
            if (isRunning) {
                animator.SetBool("isRunning", true);
            }
        } else if (Input.GetKey("a")) {
            Walking(-1);
            if (isRunning) {
                animator.SetBool("isRunning", true);
            }
        } else {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
	}

	private void Walking(int direction){

        currentDirection = direction;

        if (IsGrounded ()) {
            if (isRunning) {
                movementSpeed = runSpeed;
                animator.Play("Run");
            } else {
                movementSpeed = walkSpeed;
                animator.Play("Walk");
                animator.SetBool("isWalking", true);
            }
        }
			
		//else
			//animator.Play ("Idle");
            //animator.SetBool("isWalking", false);

        newRotation.eulerAngles = new Vector3 (newRotation.eulerAngles.x, direction * 90, newRotation.eulerAngles.z); 
		transform.rotation = newRotation;



		if (rb.velocity.x < 35)
			rb.AddForce (new Vector3 (direction * movementSpeed * Time.deltaTime, 0, 0), ForceMode.Force);
	}

	public void Damage(){
		GameObject hitBox = Instantiate (damageHitBox, damageLocation.transform.position, damageLocation.transform.rotation, damageLocation.transform);
        hitBox.GetComponent<DamageHitBox> ().damage = damage;        
	}

    public void Idle() {
        guitarStance = 0;
    }

    public void PlayGuitar() {
        guitarStance = 1;
    }

    public void Attack() {
        guitarStance = 2;
    }

	private bool IsGrounded(){
        return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}

    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0 && !dead) {
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

        // Placeholder for inventory system. Will implement better system during holidays
        if(otherObject.transform.tag == "Collectable") {
            itemID = otherObject.GetComponent<ItemPickupScript>().getItemID();
            itemValue = otherObject.GetComponent<ItemPickupScript>().getValue();
            

            if (itemID == 0) {
                currencyCount += itemValue;
<<<<<<< HEAD
                currencyText.text = "Beat Coins: " + currencyCount.ToString();

                if (randomPickupPitch) {
                    musicNoteSound.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.5f);  // Changes pitch of sound every time a new note is picked up
                } else {
                    currentPitch += 0.1f;
                    if (currentPitch >= maxPitch) {
                        currentPitch = minPitch;
                    }
                    musicNoteSound.GetComponent<AudioSource>().pitch = currentPitch;
                }

                Instantiate(musicNoteSound, transform.position, transform.rotation);
=======
                currencyText.text = "Coins: " + currencyCount.ToString();
>>>>>>> 4919591e33ba0c355a3a111cee3bd576dc95cf8e
            } else if (itemID == 1) {
                shardCount += itemValue;
                shardText.text = "Shards: " + shardCount.ToString() + " / " + shardsNeeded;
                Instantiate(shardSound, transform.position, transform.rotation);
            } else {
                Debug.Log("Invalid Item ID");
            }

            Destroy(otherObject.gameObject);

        }

        if (otherObject.transform.tag == "Falloff") {
<<<<<<< HEAD
            Instantiate(burnSound, transform.position, transform.rotation);
=======
>>>>>>> 4919591e33ba0c355a3a111cee3bd576dc95cf8e
            takeDamage(health);
        }

        if (otherObject.transform.tag == "Crusher") {
            touchTopCrush = true;
        }

        if (otherObject.transform.tag == "CrusherFloor") {
            touchBottomCrush = true;
        }

        if (otherObject.transform.tag == "Checkpoint") {
            setCheckPoint(otherObject.gameObject);
            Instantiate(checkpointSound, transform.position, transform.rotation);
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

        if (other.transform.tag == "Crusher") {
            touchTopCrush = false;
        }

        if (other.transform.tag == "CrusherFloor") {
            touchBottomCrush = false;
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

    public void lightningAttack(Vector3 target) {
        if (target != null) {
            GameObject prefab = Instantiate(lightningHitbox, target, transform.rotation);
            prefab.GetComponent<DamageHitBox>().damage = damage;
            prefab.GetComponent<DamageHitBox>().player = true;
            //Instantiate(attackSound, transform.position, transform.rotation);
        }
    }

    // Used to check if the player has collected enough items
    public int getItemCount() {
        return shardCount;
    }

    public void setCutscene(bool cutscenePlaying) {
        isCutscene = cutscenePlaying;
    }

    public void setGuitarStance(int newStance) {
        guitarStance = newStance;
    }

}
