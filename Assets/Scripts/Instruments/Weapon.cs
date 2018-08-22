using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    // Player objects 
    public GameObject playerModel; // Actual character model. Used to adjust model angle without affecting player
    protected GameObject playerObject; // Object where script, physics, etc. is attached.
    protected int currentDirection;
    protected static bool canAttack;

    // Damage variables
    public float weaponDamage = 50.0f;
    protected float attackTimer;
    public float attackRate = 1.5f;
    public GameObject damageLocation;
    public GameObject damageHitBox;
    protected float magicTimer1;
    protected float magicTimer2;
    public float magicRate1 = 2f; // How long the player has to wait between activating abilities
    public float magicRate2 = 2f; // How long the player has to wait between activating abilities

    // Sound variables
    public GameObject attackSound;
    public GameObject magicSound1;
    public GameObject magicSound2;

    // Animation variables
    protected Animator animator;
    public string attackAnimationName = "Attacking";

    // Where the instrument is located on the player model
    protected int guitarStance; // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing

    // Player rig objects. Used to relocate weapon model position
    public GameObject guitarParentSpine;
    public GameObject guitarParentHand;
    
    // Use this for initialization
    void Start () {
        try {
            playerObject = GameObject.Find("Player");
            animator = playerObject.GetComponent<Animator>();
        } catch {
            playerObject = null;
        }

        canAttack = true;

    }
	
	// Update is called once per frame
	void Update () {

        AdjustWeaponPosition();

        //attack
        if(canAttack) {
            if (Input.GetMouseButtonDown(0) && Time.time > attackTimer) {
                //Debug.Log("Player Attacked");
                animator.SetTrigger(attackAnimationName);
                Instantiate(attackSound, transform.position, transform.rotation);
                Damage();

                attackTimer = Time.time + attackRate;
            }
            //Lightning Attack
            if (Input.GetKeyDown("1") && Time.time > magicTimer1) {
                SpecialAttack1();
                magicTimer1 = Time.time + magicRate1;
            }

            if (Input.GetKeyDown("2") && Time.time > magicTimer2) {
                SpecialAttack2();
                magicTimer2 = Time.time + magicRate2;
            }
        }
    }

    protected virtual void Damage() {

        //GameObject hitBox = Instantiate(damageHitBox, damageLocation.transform.position, damageLocation.transform.rotation, damageLocation.transform);

        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;
        float angle;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        GameObject hitBox = Instantiate(damageHitBox, damageLocation.transform.position,
            Quaternion.Euler(new Vector3(0, 0, angle)), damageLocation.transform);
        hitBox.GetComponent<DamageHitBox>().damage = weaponDamage;
        hitBox.GetComponent<DamageHitBox>().moveForward(0.8f);

    }

    protected virtual void AdjustWeaponPosition() {
        // Determines what guitar object is enabled. 0 for Back enabled, 1 for front, 2 for swing
        if (guitarStance == 1) {
            // Adjusts the guitar objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(-0.1985897f, -0.08787628f, 0.381499f);
            this.gameObject.transform.localEulerAngles = new Vector3(3.086f, 15.034f, 75.04601f);

            // Adjusts the guitar object parent so it moves with the body
            this.gameObject.transform.parent = guitarParentSpine.transform;

        } else if (guitarStance == 2) {
            // Adjusts the guitar objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(-0.04300219f, -0.3379997f, -0.3949996f);
            this.gameObject.transform.localEulerAngles = new Vector3(197.45f, 77.406f, 135.738f);

            // Adjusts the guitar object parent so it moves with the body
            this.gameObject.transform.parent = guitarParentHand.transform;
        } else {
            // Adjusts the guitar objects position and rotation
            this.gameObject.transform.localPosition = new Vector3(0.2679967f, 0.6450007f, -0.1319958f);
            this.gameObject.transform.localEulerAngles = new Vector3(-1.448f, 157.108f, 20.164f);

            // Adjusts the guitar object parent so it moves with the body
            this.gameObject.transform.parent = guitarParentSpine.transform;
        }
    }

    
    public void SetIdleStance(int newStance) {
        guitarStance = newStance;
    }

    protected virtual void SpecialAttack1() {

        Debug.Log("Special Attack 1 activated.");
        magicTimer1 = Time.time + magicRate1;

    }

    protected virtual void SpecialAttack2() {
        Debug.Log("Special Attack 2 activated.");
        magicTimer2 = Time.time + magicRate2;
    }

    public void setCanAttack(bool attackSetting) {
        canAttack = attackSetting;
    }

}
