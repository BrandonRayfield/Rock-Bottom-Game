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

    // Guitar Objects
    public GameObject banjoObject;
    public GameObject guitarObject;
    public GameObject currentGuitarObject;

    // Wind Objects
    public GameObject harmonicaObject;
    public GameObject megaphoneObject;
    public GameObject currentHarmonicaObject;

    // Instument Unlocks
    public GameObject harmonicaSelectImage;
    public bool harmonicaUnlocked;
    public bool guitarUnlocked;
    public bool megaphoneUnlocked;

    // Skeleton Objects used for repositioning instruments
    public GameObject guitarParentSpine;
    public GameObject guitarParentHand;

    private int guitarStance; // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing

    //Time variables
    private float time = 3.0f;
    private float restartTime = 0;


    private bool canMove; // Disables player movement. Mainly used for pausing and dialogue
    private bool canJump; // Work around for weird bug we are having
    private bool canSwing; // Used for rope swings
    private GameObject ropeObject;

    public bool unlockedDoubleJump = false;
    private bool canDoubleJump;


    //Walking
    private float moveSpeed = 2.0f;
	private Quaternion newRotation;
    private int currentDirection;
    public int movementSpeed;
    private int walkSpeed = 10;
    private int runSpeed = 20;

    //Running
    private bool isRunning;

	//Jumping
	public float jumpForce = 200.0f;
	private float distToGround = 1;
	private float jumpCoolDown = 0.6f;
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
    private int livesLeft;
    public bool invulnerable = false;

    public bool dead = false;
    private float damage = 50.0f;
    private float attackTimer;
    private float attackRate = 1.5f;
	public GameObject damageLocation;
	public GameObject damageHitBox;

    private float lightningDamage = 150.0f;

    private float damageTimer;
    private float damageTime = 0.5f;

    // Debuff Variables
    private int slowDebuff;
    private int minSlowDebuff = 1;
    private int maxSlowDebuff = 2;
    private bool isSlowed;

    private float slowTime;
    private float slowDuration = 2.0f;

    // Obstacle Variables
    private bool touchTopCrush;
    private bool touchBottomCrush;

    //UI Variables
    public GameObject goldKeyUI;
    public Slider healthBar;
    public Text gameResult;
    public Text gameResult2;
    public Text shardText;
    public Text currencyText;
    public Text livesText;

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
    public GameObject guitarSound;
    public GameObject squashSound;
    public GameObject checkpointSound;

    private float minPitch = 0.5f;
    private float maxPitch = 1.5f;
    private float currentPitch;

    //Magic Variables
    public GameObject lightningControl;
    public GameObject lightningHitbox;
    public GameObject lightningParticles;
    public float magicTimer;
    private float magicRate = 2f;
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

        livesLeft = 3;
        currentPitch = minPitch;
        guitarStance = 0; // Back guitar starts enabled
        slowDebuff = minSlowDebuff;

        livesText.text = "Lives: " + livesLeft;

        //Sets current weapon
        if (!guitarUnlocked && !megaphoneUnlocked) {
            currentGuitarObject = banjoObject;
            currentHarmonicaObject = harmonicaObject;
            gameObject.GetComponent<PauseMenu>().setGuitarWeapon(currentGuitarObject);
            gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
        }

        if(!harmonicaUnlocked) {
            harmonicaSelectImage.SetActive(false);
        }

        canMove = true;
        canJump = false;
        canDoubleJump = false;

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

    }

    // Update is called once per frame
    void Update() {

        if (canSwing && Input.GetKey(KeyCode.E)) {

            //gameObject.transform.parent = ropeObject.transform;
            //gameObject.transform.position = ropeObject.transform.position; 
            ropeObject.GetComponent<Rope_Swing>().setIsSwinging(true);
        }

        movementSpeed = movementSpeed / slowDebuff;

        if (isSlowed) {
            slowDebuff = maxSlowDebuff;
            slowTime += Time.deltaTime;
            if (slowTime >= slowDuration) {
                isSlowed = false;
                slowDebuff = minSlowDebuff;
                slowTime = 0f;
            }
        }

        gameResult2.text = gameResult.text;
        gameResult2.enabled = gameResult.enabled;

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

            time -= Time.deltaTime;

            if (livesLeft < 0) {
                gameResult.text = "Game Over.";
                gameResult.enabled = true;
            } else {
                gameResult.text = Mathf.RoundToInt(time).ToString();
                gameResult.enabled = true;
            }

            if (time <= restartTime) {
                if (livesLeft < 0) {
                    SceneManager.LoadScene(0);
                } else {
                    gameResult.enabled = false;
                    dead = false;
                    if (spawnPoint == null) {
                        gameObject.transform.position = startPoint.transform.position;
                        cameraObject.transform.position = startPoint.transform.position;
                        health = maxHealth;
                        time = 3f;

                    } else {
                        gameObject.transform.position = spawnPoint.transform.position;
                        cameraObject.transform.position = spawnPoint.transform.position;
                        health = maxHealth;
                        time = 3f;
                    }
                }
            }
        }


        //Update UI Components
        healthBar.value = health / 100;
    }

    private void Controls() {

        if (!IsGrounded()) {
            animator.Play("Jump");
            animator.SetBool("isJumping", true);
        } else {
            animator.SetBool("isJumping", false);
        }

        if (canMove) {
            //Jumping
            if(canJump) {
                if (Input.GetKeyDown("space") && IsGrounded() && Time.time > jumpTime) {
                    rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
                    Instantiate(jumpSound, transform.position, transform.rotation);
                    canDoubleJump = true;
                    jumpTime = Time.time + jumpCoolDown;
                }

                if(unlockedDoubleJump) {
                    if (Input.GetKeyDown("space") && !IsGrounded() && canDoubleJump) {
                        animator.SetBool("isJumping", false);
                        animator.Play("Jump");
                        animator.SetBool("isJumping", true);
                        rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
                        Instantiate(jumpSound, transform.position, transform.rotation);
                        canDoubleJump = false;
                    }
                }

            }
            
            if (Input.GetKey("s") && canPhase) {
                platformCollider.enabled = false;
            }

            //Movement
            if (Input.GetKey("left shift")) {
                isRunning = true;
            }
            else {
                isRunning = false;
            }

            if (Input.GetKey("d")) {
                Walking(1);
                if (isRunning) {
                    animator.SetBool("isRunning", true);
                }
            }
            else if (Input.GetKey("a")) {
                Walking(-1);
                if (isRunning) {
                    animator.SetBool("isRunning", true);
                }
            }
            else {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }

            if (Input.GetKeyUp("d") || Input.GetKeyUp("a")) {
                Vector3 temp = rb.velocity;
                temp.x /= 2;
                rb.velocity = temp;

                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        } 
    }

	private void Walking(int direction){

        canJump = true;

        currentDirection = direction;

        if (IsGrounded ()) {
            if (isRunning) {
                movementSpeed = runSpeed / slowDebuff;
                animator.Play("Run");
            } else {
                movementSpeed = walkSpeed / slowDebuff;
                animator.Play("Walk");
                animator.SetBool("isWalking", true);
            }
        }
			
		//else
			//animator.Play ("Idle");
            //animator.SetBool("isWalking", false);

        newRotation.eulerAngles = new Vector3 (newRotation.eulerAngles.x, direction * 90, newRotation.eulerAngles.z); 
		transform.rotation = newRotation;



            rb.AddForce(new Vector3(-rb.velocity.x, 0, 0), ForceMode.Force);
			rb.AddForce (new Vector3 (direction * movementSpeed, 0, 0), ForceMode.Force);
	}

	private bool IsGrounded(){
        return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}

    public void Damage() {
        currentGuitarObject.GetComponent<Weapon>().CallDamage();
    }

    public void takeDamage(float damage) {
        if (Time.time >= damageTimer && !invulnerable) {
            health -= damage;
            damageTimer = Time.time + damageTime;
            //isSlowed = true;

            if (health <= 0 && !dead) {
                dead = true;
                livesLeft--;
                if (livesLeft >= 0) {
                    livesText.text = "Lives: " + livesLeft;
                }
                Instantiate(deathSound, transform.position, transform.rotation);
            }
        }
    }

	void OnTriggerEnter(Collider otherObject) {

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


            if (itemID == 420) { // Player picks up currency object
                currencyCount += itemValue;
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

            } else if (itemID == 421) { // Player picks up health object
                Instantiate(shardSound, transform.position, transform.rotation);
                gainHealth(itemValue);

            } else if (itemID == 422) { // Player picks up health object
                Instantiate(shardSound, transform.position, transform.rotation);
                livesLeft = livesLeft + itemValue;
                livesText.text = "Lives: " + livesLeft;

            } else if (itemID == 600) { // Player finds and unlocks Harmonica
                setHarmonicaUnlocked(true);
                unlockHarmonica();

            } else if (itemID == 601) { // Player finds and unlocks Guitar
                setGuitarUnlocked(true);
                unlockGuitar();

            } else if (itemID == 602) { // Player finds and unlocks Megaphone
                setMegaphoneUnlocked(true);
                unlockMegaphone();
            } else { // Player picks up quest item. itemID should be the same as the questID
                GameManager.instance.AddCounter(itemID, itemValue);
                Instantiate(shardSound, transform.position, transform.rotation);

                //shardCount += itemValue;
                //shardText.text = "Shards: " + shardCount.ToString() + " / " + shardsNeeded;
                //Instantiate(shardSound, transform.position, transform.rotation);
            }

            Destroy(otherObject.gameObject);

        }

        if (otherObject.transform.tag == "Falloff") {
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

        if(otherObject.transform.tag == "Rope")
        {
            canSwing = true;
            ropeObject = otherObject.gameObject;
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

        if (other.transform.tag == "Rope") {
            canSwing = false;
            ropeObject = null;
        }

    }

    public void gainHealth(int healthRecovered) {
        if (health <= maxHealth - healthRecovered) {
            health += healthRecovered;
        }
        else if (health < maxHealth && health > maxHealth - healthRecovered) {
            health = maxHealth;
        }
        healthBar.value = (health / maxHealth);
    }

    private void unlockHarmonica() {
        harmonicaSelectImage.SetActive(true);
        currentHarmonicaObject = harmonicaObject;
        gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
        gameObject.GetComponent<PauseMenu>().selectHarmonica();
    }

    private void unlockGuitar() {
        currentGuitarObject.SetActive(false);
        currentGuitarObject = guitarObject;
        gameObject.GetComponent<PauseMenu>().setGuitarWeapon(currentGuitarObject);
        gameObject.GetComponent<PauseMenu>().selectGuitar();
    }

    private void unlockMegaphone() {
        currentHarmonicaObject.SetActive(false);
        currentHarmonicaObject = megaphoneObject;
        gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
        gameObject.GetComponent<PauseMenu>().selectHarmonica();
    }

    //---------------------------------------------------------------------------------------------------------------------
    // These are used in animation events, make sure to keep them.
    public void IdleStance() {
        currentGuitarObject.GetComponent<Weapon>().SetIdleStance(0);
        currentHarmonicaObject.GetComponent<Weapon>().SetIdleStance(0);
    }

    public void PlayGuitarStance() {
        currentGuitarObject.GetComponent<Weapon>().SetIdleStance(1);
        currentHarmonicaObject.GetComponent<Weapon>().SetIdleStance(1);

    }

    public void AttackStance() {
        currentGuitarObject.GetComponent<Weapon>().SetIdleStance(2);
        currentHarmonicaObject.GetComponent<Weapon>().SetIdleStance(2);
    }

    //---------------------------------------------------------------------------------------------------------------------

    public void setCheckPoint(GameObject spawnlocation) {
        spawnPoint = spawnlocation;
    }

    public GameObject getCheckPoint() {
        return spawnPoint;
    }

    public void setMaxHealth(float moreHealth) {
        maxHealth += moreHealth;
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

    public int getPlayerDirection() {
        return currentDirection;
    }

    public void setCanMove(bool ableToMove) {
        canMove = ableToMove;
    }

    public void setUnlockedDoubleJump(bool hasUnlocked) {
        unlockedDoubleJump = hasUnlocked;
    }

    //---------------------------------------------------------------------------------------------------------------------
    // Getters and Setters for Rope swing
    public bool getCanSwing() {
        return canSwing;
    }

    public void setCanSwing(bool letSwing) {
        canSwing = letSwing;
    }

    //---------------------------------------------------------------------------------------------------------------------
    // Getters and Setters for Instuments

    public void setHarmonicaUnlocked(bool hasUnlocked) {
        harmonicaUnlocked = hasUnlocked;
    }

    public void setGuitarUnlocked(bool hasUnlocked) {
        guitarUnlocked = hasUnlocked;
    }

    public void setMegaphoneUnlocked(bool hasUnlocked) {
        megaphoneUnlocked = hasUnlocked;
    }

    public bool getHarmonicaUnlocked() {
        return harmonicaUnlocked;
    }

    public bool getGuitarUnlocked() {
        return guitarUnlocked;
    }

    public bool getMegaphoneUnlocked() {
        return megaphoneUnlocked;
    }
}
