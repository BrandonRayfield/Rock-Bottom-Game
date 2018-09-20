using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //---------------------------------------------------------------
    Animator animator;
	Rigidbody rb;
    [Header("Main Objects")]
    public GameObject cameraObject;
    public GameObject playerModel;

    //---------------------------------------------------------------
    // Guitar Objects
    [Header("Weapon Objects")]
    public GameObject banjoObject;
    public GameObject guitarObject;
    public GameObject currentGuitarObject;

    //---------------------------------------------------------------
    // Wind Objects
    public GameObject harmonicaObject;
    public GameObject megaphoneObject;
    public GameObject currentHarmonicaObject;

    //---------------------------------------------------------------
    // Instument Unlocks
    public bool harmonicaUnlocked;
    public bool guitarUnlocked;
    public bool megaphoneUnlocked;

    //---------------------------------------------------------------
    // Skeleton Objects used for repositioning instruments
    public GameObject guitarParentSpine;
    public GameObject guitarParentHand;

    private int guitarStance; // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing

    //---------------------------------------------------------------
    //Time variables
    private float time = 3.0f;
    private float restartTime = 0;

    //---------------------------------------------------------------
    private bool canMove; // Disables player movement. Mainly used for pausing and dialogue
    private bool canJump; // Work around for weird bug we are having
    private bool canSwing; // Used for rope swings
    private GameObject ropeObject;

    [HideInInspector]
    public bool unlockedDoubleJump = false;
    private bool canDoubleJump;

    //---------------------------------------------------------------
    //Walking

    private float moveSpeed = 2.0f;
	private Quaternion newRotation;
    private int currentDirection;
    [Header("Movment Variables")]
    public int movementSpeed;
    private int walkSpeed = 10;
    private int runSpeed = 20;

    private int newMovementSpeed;
    private int newWalkSpeed = 4;
    private int newRunSpeed = 8;

    //---------------------------------------------------------------
    //Running
    private bool isRunning;

    //---------------------------------------------------------------
    //Jumping
    [Header("Jumping Variables")]
    private bool wantsToJump;
    public float jumpForce = 200.0f;
	private float distToGround = 1;
	private float jumpCoolDown = 0.6f;
	private float jumpTime;

    //---------------------------------------------------------------
    // Obstacle Variables
    private bool canPhase;
    private GameObject platformObject;
    private Collider platformCollider;

    //---------------------------------------------------------------
    // Cutscene Variables
    private bool isCutscene = false;

    //---------------------------------------------------------------
    //Spawn Variables
    [Header("Spawnpoint Variables")]
    public GameObject startPoint;
    public GameObject spawnPoint;
    //---------------------------------------------------------------
    //Damage Variables
    [Header("Player Health Variables")]
    public float health;
    public float maxHealth = 100.0f;
    private int livesLeft;
    public bool invulnerable = false;
    public bool dead = false;

    [Header("Player Damage Variables")]
    private float damage = 50.0f;
    private float attackTimer;
    private float attackRate = 1.5f;
	public GameObject damageLocation;
	public GameObject damageHitBox;

    private float lightningDamage = 150.0f;

    private float damageTimer;
    private float damageTime = 0.5f;
    //---------------------------------------------------------------
    // Obstacle Variables
    private bool touchTopCrush;
    private bool touchBottomCrush;

    // Checkpoint Variables
    private bool checkpointActivated;
    //---------------------------------------------------------------
    //UI Variables
    [Header("UI Variables")]
    public Slider healthBar;
    public Text gameResult;
    public Text gameResult2;
    public Text shardText;
    public Text currencyText;
    public Text livesText;
    public GameObject restartMenu;
    //---------------------------------------------------------------
    // Weapon UI Variables
    [Header("Weapon UI Variables")]
    public GameObject currentGuitarIcon;
    public Text guitarText;
    public GameObject currentWindIcon;
    public Text windText;

    public Sprite banjoImage;
    public Sprite guitarImage;
    public Sprite harmonicaImage;
    public Sprite megaphoneImage;
    //---------------------------------------------------------------
    //Item Variables
    [Header("Item Variables")]
    public int currencyCount;
    public int shardCount;
    private int itemValue;
    private int itemID;
    //---------------------------------------------------------------
    //Sound Variables
    [Header("Sound Variables")]
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
    //---------------------------------------------------------------
    //Magic Variables
    [Header("Ability Variables")]
    private float magicTimer1;
    private float magicTimer2;

    private float currentAbilityTime1;
    private float currentAbilityTime2;

    private float targetTime1;
    private float targetTime2;

    private bool activatedAbility1;
    private bool activatedAbility2;

    private float maxCooldown1;
    private float maxCooldown2;

    private bool canUse1;
    private bool canUse2;

    //---------------------------------------------------------------
    // Weapon Ability UI Variables
    [Header("Ability UI Elements")]
    public GameObject AbilityUI1;
    public GameObject AbilityUI2;
    public GameObject AbilityUI1_CD;
    public GameObject AbilityUI2_CD;
    public GameObject AbilityAvailableUI1;
    public GameObject AbilityAvailableUI2;
    public Text abilty1Text;
    public Text abilty2Text;
    public Text abilty1TimerText;
    public Text abilty2TimerText;

    //-----------------------
    // Weapon Icons
    [Header("Abilty Icons")]
    public Sprite emptyIcon;
    public Sprite banjoIcon1;

    public Sprite harmonicaIcon1;

    public Sprite guitarIcon1;
    public Sprite guitarIcon2;

    public Sprite megaphoneIcon1;
    public Sprite megaphoneIcon2;

    private bool guitarEquipped;
    //---------------------------------------------------------------
    // Transition Scene Variables
    [Header("Transition Scene Variables")]
    public bool isTransiton;
    public GameObject elevatorTrigger;
    public Animator elevatorAnimator;
    //---------------------------------------------------------------

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		distToGround = transform.GetComponent<Collider> ().bounds.extents.y;
		newRotation = transform.rotation;
        health = maxHealth;
        currentDirection = 1;

        // Make sure game over menu starts disabled
        restartMenu.SetActive(false);

        livesLeft = 3;
        currentPitch = minPitch;
        guitarStance = 0; // Back guitar starts enabled

        livesText.text = "Lives: " + livesLeft;

        //Sets current weapon
        if (!guitarUnlocked) {
            currentGuitarObject = banjoObject;
            gameObject.GetComponent<PauseMenu>().setGuitarWeapon(currentGuitarObject);
            // Set UI stuff
            currentGuitarIcon.GetComponent<Image>().sprite = banjoImage;
            guitarText.text = "Banjo";
        } else {
            currentGuitarObject = guitarObject;
            gameObject.GetComponent<PauseMenu>().setGuitarWeapon(currentGuitarObject);
            // Set UI stuff
            currentGuitarIcon.GetComponent<Image>().sprite = guitarImage;
            guitarText.text = "Guitar";
        }

        if(!megaphoneUnlocked) {
            currentHarmonicaObject = harmonicaObject;
            gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
            // Set UI stuff
            currentWindIcon.GetComponent<Image>().sprite = harmonicaImage;
            windText.text = "Harmonica";
        } else {
            currentHarmonicaObject = megaphoneObject;
            gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
            // Set UI stuff
            currentWindIcon.GetComponent<Image>().sprite = megaphoneImage;
            windText.text = "Megaphone";
        }

        if(!megaphoneUnlocked && !harmonicaUnlocked) {
            currentWindIcon.SetActive(false);
        }

        // Updating Guitar
        gameObject.GetComponent<PauseMenu>().selectGuitar();

        canMove = true;
        canJump = false;
        canDoubleJump = false;

        try {
            cameraObject = GameObject.Find("Camera");
        } catch {
            cameraObject = null;
        }

        //Disable Ability UI Elements
        abilty1TimerText.gameObject.SetActive(false);
        AbilityUI1_CD.gameObject.SetActive(false);

        abilty2TimerText.gameObject.SetActive(false);
        AbilityUI2_CD.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {

        AbilityCooldowns();

        if (!dead && !isCutscene) {
            Controls();
        } else if (dead) {
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
                    //SceneManager.LoadScene(0);
                    gameResult.enabled = false;
                    restartMenu.SetActive(true);
                } else {
                    gameResult.enabled = false;
                    dead = false;
                    if (spawnPoint == null) {
                        gameObject.transform.position = new Vector3(startPoint.transform.position.x, startPoint.transform.position.y, gameObject.transform.position.z);
                        cameraObject.transform.position = startPoint.transform.position;
                        health = maxHealth;
                        time = 3f;

                    } else {
                        gameObject.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, gameObject.transform.position.z);
                        cameraObject.transform.position = spawnPoint.transform.position;
                        health = maxHealth;
                        time = 3f;
                    }

                    // Resetting Elevator in transition
                    if (isTransiton) {
                        transform.parent = null;
                        elevatorAnimator.Play("Reset");
                        elevatorTrigger.GetComponent<Ring_Trigger>().SetIsTriggered(false);
                        GameManager.instance.killTheSpiders();
                    }

                    if (SceneManager.GetActiveScene().buildIndex == 7) {
                        GameManager.instance.killTheSpiders();
                    }

                }
            }
        }

        if (canSwing && Input.GetKey(KeyCode.E)) {

            //gameObject.transform.parent = ropeObject.transform;
            //gameObject.transform.position = ropeObject.transform.position; 
            ropeObject.GetComponent<Rope_Swing>().setIsSwinging(true);
        }

        gameResult2.text = gameResult.text;
        gameResult2.enabled = gameResult.enabled;

        if (touchTopCrush && touchBottomCrush) {
            takeDamage(health);
            Instantiate(squashSound, transform.position, transform.rotation);
            touchTopCrush = false;
            touchBottomCrush = false;
        }

        //Update UI Components
        healthBar.value = health / 100;
    }

    // Moved jumping code to fixed update in order to fix weird super jump glitch. Seems to work much better now
    private void FixedUpdate() {
        if(wantsToJump) {
            Vector3 velocity = rb.velocity;
            velocity.y = jumpForce * Time.deltaTime;
            rb.velocity = velocity;
            //rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
            Instantiate(jumpSound, transform.position, transform.rotation);
            canDoubleJump = true;
            //jumpTime = Time.time + jumpCoolDown;
            wantsToJump = false;
        }
    }

    private void AbilityCooldowns() {

        if(guitarEquipped) {
            canUse1 = currentGuitarObject.GetComponent<Weapon>().getCanUse1();
            canUse2 = currentGuitarObject.GetComponent<Weapon>().getCanUse2();
        } else {
            canUse1 = currentHarmonicaObject.GetComponent<Weapon>().getCanUse1();
            canUse2 = currentHarmonicaObject.GetComponent<Weapon>().getCanUse2();
        }

        if(!canUse1) {
            AbilityAvailableUI1.gameObject.SetActive(true);
        } else {
            AbilityAvailableUI1.gameObject.SetActive(false);
        }

        if (!canUse2) {
            AbilityAvailableUI2.GetComponent<Image>().color = Color.red;
            AbilityAvailableUI2.gameObject.SetActive(true);
        } else {
            AbilityAvailableUI2.gameObject.SetActive(false);
        }

        if (activatedAbility1) {
            currentAbilityTime1 -= Time.deltaTime;

            abilty1TimerText.text = Mathf.RoundToInt(currentAbilityTime1).ToString();
            AbilityUI1_CD.GetComponent<Image>().fillAmount = (1 / maxCooldown1) * currentAbilityTime1;

            if (currentAbilityTime1 <= 0) {
                activatedAbility1 = false;
                abilty1TimerText.gameObject.SetActive(false);
            }
        }

        if (activatedAbility2) {
            currentAbilityTime2 -= Time.deltaTime;

            abilty2TimerText.text = Mathf.RoundToInt(currentAbilityTime2).ToString();
            AbilityUI2_CD.GetComponent<Image>().fillAmount = (1 / maxCooldown2) * currentAbilityTime2;

            if (currentAbilityTime2 <= 0) {
                activatedAbility2 = false;
                abilty2TimerText.gameObject.SetActive(false);
            }
        }
    }

    private void Controls() {

        if (!IsGrounded()) {
            animator.Play("Jump");
            animator.SetBool("isJumping", true);
        } else {
            animator.SetBool("isJumping", false);
        }

        if (canMove && GameManager.instance.isTalking == false) {
            //Jumping
            if(canJump) {
                if (Input.GetKeyDown("space") && IsGrounded() && Time.time > jumpTime) {
                    wantsToJump = true;
                }

                if(unlockedDoubleJump) {
                    if (Input.GetKeyDown("space") && !IsGrounded() && canDoubleJump) {
                        animator.SetBool("isJumping", false);
                        animator.Play("Jump");
                        animator.SetBool("isJumping", true);
                        Vector3 velocity = rb.velocity;
                        velocity.y = jumpForce * Time.deltaTime;
                        rb.velocity = velocity;
                        //rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
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
        } else if (GameManager.instance.isTalking == true) {
            Vector3 velocity = rb.velocity;
            if (velocity.y > 0f) velocity.y = 0f;
            velocity.x = 0f;
            rb.velocity = velocity;
        }
    }

	private void Walking(int direction){

        canJump = true;

        currentDirection = direction;

        if (IsGrounded ()) {
            if (isRunning) {
                movementSpeed = runSpeed;
                newMovementSpeed = newRunSpeed;
                animator.Play("Run");
            } else {
                movementSpeed = walkSpeed;
                newMovementSpeed = newWalkSpeed;
                animator.Play("Walk");
                animator.SetBool("isWalking", true);
            }
        }
			
		//else
			//animator.Play ("Idle");
            //animator.SetBool("isWalking", false);

        newRotation.eulerAngles = new Vector3 (newRotation.eulerAngles.x, direction * 90, newRotation.eulerAngles.z); 
		transform.rotation = newRotation;

        // Testing new movement system... Might be too late to change now that levels have been designed with previous values.
        //Vector3 newVelocity = rb.velocity;
        //newVelocity.x = newMovementSpeed * direction;
        //rb.velocity = newVelocity;

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

        // Placeholder for inventory system. Will implement better system during holidays
        if(otherObject.transform.tag == "Collectable") {
            itemID = otherObject.GetComponent<ItemPickupScript>().getItemID();
            itemValue = otherObject.GetComponent<ItemPickupScript>().getValue();


            if (itemID == 420) { // Player picks up currency object
                currencyCount += itemValue;
                if (currencyCount%100 == 0) {
                    Instantiate(shardSound, transform.position, transform.rotation);
                    livesLeft++;
                    livesText.text = "Lives: " + livesLeft;
                }
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

    public void updateCurrencyAmount(int costAmount) {
        currencyCount -= costAmount;
        currencyText.text = "Beat Coins: " + currencyCount.ToString();
    }

    public int getCurrencyAmount() {
        return currencyCount;
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

    //---------------------------------------------------------------------------------------------------------------------
    // Function used for restoring health from health pickups and checkpoints
    public void gainHealth(int healthRecovered) {
        if (health <= maxHealth - healthRecovered) {
            health += healthRecovered;
        }
        else if (health < maxHealth && health > maxHealth - healthRecovered) {
            health = maxHealth;
        }
        healthBar.value = (health / maxHealth);
    }

    //---------------------------------------------------------------------------------------------------------------------
    // Functions used to update weapon ability UI + cooldowns
    public void updateWeaponAbilities() {
        guitarEquipped = gameObject.GetComponent<PauseMenu>().getGuitarEquipped();

        if (guitarEquipped && currentGuitarObject == banjoObject) {
            AbilityUI1.GetComponent<Image>().sprite = banjoIcon1;
            AbilityUI2.GetComponent<Image>().sprite = emptyIcon;
        } 

        else if(guitarEquipped && currentGuitarObject == guitarObject) {
            AbilityUI1.GetComponent<Image>().sprite = guitarIcon1;
            AbilityUI2.GetComponent<Image>().sprite = guitarIcon2;
        } 
        
        else if (!guitarEquipped && currentHarmonicaObject == harmonicaObject) {
            AbilityUI1.GetComponent<Image>().sprite = harmonicaIcon1;
            AbilityUI2.GetComponent<Image>().sprite = emptyIcon;
        } 
        
        else if (!guitarEquipped && currentHarmonicaObject == megaphoneObject) {
            AbilityUI1.GetComponent<Image>().sprite = megaphoneIcon1;
            AbilityUI2.GetComponent<Image>().sprite = megaphoneIcon2;
        }

    }

    public void updateAbilityCooldown1(float currentTime) {

        if(guitarEquipped) {
            maxCooldown1 = currentGuitarObject.GetComponent<Weapon>().getMagicRate1();
        } else {
            maxCooldown1 = currentHarmonicaObject.GetComponent<Weapon>().getMagicRate1();
        }

        magicTimer1 = currentTime;
        activatedAbility1 = true;
        targetTime1 = magicTimer1 - Time.time;
        currentAbilityTime1 = targetTime1;

        //Enable UI Elements
        abilty1TimerText.gameObject.SetActive(true);
        AbilityUI1_CD.gameObject.SetActive(true);

    }

    public void updateAbilityCooldown2(float currentTime) {

        if (guitarEquipped) {
            maxCooldown2 = currentGuitarObject.GetComponent<Weapon>().getMagicRate2();
        } else {
            maxCooldown2 = currentHarmonicaObject.GetComponent<Weapon>().getMagicRate2();
        }

        magicTimer2 = currentTime;
        activatedAbility2 = true;
        targetTime2 = magicTimer2 - Time.time;
        currentAbilityTime2 = targetTime2;

        //Enable UI Elements
        abilty2TimerText.gameObject.SetActive(true);
        AbilityUI2_CD.gameObject.SetActive(true);
    }



    //---------------------------------------------------------------------------------------------------------------------
    // Functions for unlocking new weapons
    private void unlockHarmonica() {
        currentWindIcon.GetComponent<Image>().sprite = harmonicaImage;
        currentWindIcon.SetActive(true);
        currentHarmonicaObject = harmonicaObject;
        gameObject.GetComponent<PauseMenu>().setHarmonicaWeapon(currentHarmonicaObject);
        gameObject.GetComponent<PauseMenu>().selectHarmonica();
    }

    private void unlockGuitar() {
        currentGuitarIcon.GetComponent<Image>().sprite = guitarImage;
        currentGuitarObject.SetActive(false);
        currentGuitarObject = guitarObject;
        gameObject.GetComponent<PauseMenu>().setGuitarWeapon(currentGuitarObject);
        gameObject.GetComponent<PauseMenu>().selectGuitar();
    }

    private void unlockMegaphone() {
        currentWindIcon.GetComponent<Image>().sprite = megaphoneImage;
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

    //---------------------------------------------------------------------------------------------------------------------
    // Getters and Setters for UI
    public GameObject getCurrentGuitarIcon() {
        return currentGuitarIcon;
    }

    public GameObject getCurrentWindIcon() {
        return currentWindIcon;
    }
}
