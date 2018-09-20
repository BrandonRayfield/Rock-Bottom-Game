using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour {

    public enum state { charge, smash, crush, summon, idle};
    public bool started = false;

    public state current_state = state.idle;

    Animator animator;
    //movement variables
    public float speed;
    private Rigidbody rb;

    //player tracking
    public Player player;
    private int direction = 0;
    private float cycle_timer = 0;

    //Summon variables
    public List<GameObject> summons = new List<GameObject>();
    private bool summoned = false;

    //Crush variables
    private Vector3 crush_location;
    private bool drop = false;
    public Quaternion crush_rotation;
    public GameObject crush_effect;

    //Health Bar Objects
    public int health = 100;
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    public GameObject EnemyHealth;
    private float currentHealthDisTime;
    private bool dead = false;
    public EnemyGeneric enemygeneric;

    //attack objects
    public GameObject smash_projectile;
    public GameObject crushAttack;
    public GameObject chargeAttack;
    public GameObject crushLocation;
    public GameObject chargeLocation;

    //NEW VARIABLES

    public GameObject guitarObject;

    private Vector3 guitarStartPosition;
    private Quaternion guitarStartRotation;
    private Vector3 guitarStartScale;

    private Vector3 guitarAttackPosition;
    private Quaternion guitarAttackRotation;
    private Vector3 guitarAttackScale;

    public GameObject modelSpine;
    public GameObject weaponObjectSlot;
    private int guitarStance;

    // Winning Variables
    public GameObject bossZone;
    public GameObject finalDialogue;
    public bool isEngaged;


      // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemygeneric = GetComponent<EnemyGeneric>();
        direction = (int)Mathf.Sign((player.transform.position.x - transform.position.x));


        guitarStartPosition = guitarObject.transform.localPosition;
        guitarStartRotation = guitarObject.transform.localRotation;
        guitarStartScale = guitarObject.transform.localScale;

        guitarAttackPosition = new Vector3(0.0f, 0.0f, 0.0f); // This is only a rough position, might need to be tuned a bit in order to line it up correctly with hands
        guitarAttackRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f); // This is only a rough rotation, might need to be tuned a bit in order to line it up correctly with hands
        guitarAttackScale = new Vector3(1.0f, 1.0f, 1.0f); // This is only a rough scale, might need to be tuned a bit in order to line it up correctly with hands

   }

    // Update is called once per frame
    void Update() {

        if(isEngaged && !dead) {
            if (EnemyHealth != null) {
                EnemyHealth.SetActive(true);
            }
            switch (current_state) {
                case state.charge:
                    charge();
                    break;
                case state.summon:
                    summon();
                    break;
                case state.crush:
                    crush();
                    break;
                case state.smash:
                    smash();
                    break;
            }

            if (cycle_timer < Time.time) cycle();
        } else {
            if (EnemyHealth != null) {
                EnemyHealth.SetActive(false);
            }
        }
        



        if (Input.GetKey(KeyCode.F1)) {
            animator.Play("Idle");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F2)) {
            animator.Play("Charge");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F3)) {
            animator.Play("Smash");
            changeWeaponPosition(0);
        }

        if (Input.GetKey(KeyCode.F4)) {
            animator.Play("Roar");
            changeWeaponPosition(1);
        }

        if (Input.GetKey(KeyCode.F5)) {
            animator.Play("Jump");
            changeWeaponPosition(1);

        }
    }
    public void cycle() {
        //Get a new state
        /*if (transform.rotation.z != 0) {
            transform.Rotate(Vector3.forward * 55 * direction);
        }*/

        //Reset variables
        crush_location = Vector3.zero;
        drop = false;

        int new_state = Random.Range(1, 6);
        if (new_state > 5) new_state = 5;
        switch (new_state) {
            case 1:
                //transform.Rotate(Vector3.forward * 55 * -direction);
                animator.Play("Charge");
                current_state = state.charge;
                GameObject charge = Instantiate(chargeAttack, chargeLocation.transform.position, chargeLocation.transform.rotation);
                charge.transform.parent = transform;
                cycle_timer = Time.time + 3f;
                break;
            case 2:
                
                current_state = state.crush;
                //cycle_timer = Time.time + 1.5f;
                break;
            case 3:
                animator.Play("Roar");
                current_state = state.smash;
                cycle_timer = Time.time + 1.5f;
                break;
            case 4:
                summoned = false;
                current_state = state.summon;
                cycle_timer = Time.time + 1.5f;
                break;
            case 5:
                current_state = state.idle;
                cycle_timer = Time.time + 1.5f;
                break;
        }
        //change direction
        if (direction != (int)Mathf.Sign(player.transform.position.x - transform.position.x)) {
            direction = (int)Mathf.Sign((player.transform.position.x - transform.position.x));
            transform.Rotate(Vector3.up, 180f);
        }

        //reset timer
        
    }

    public void charge() {
        rb.velocity = new Vector3(speed * direction * Time.deltaTime * 2.5f, 0, 0);
        
    }

    public void OnTriggerEnter(Collider other) {
        if (current_state == state.charge && other.transform.tag == "Wall") {
            cycle();
            cycle_timer = Time.time + 15f;
            animator.Play("Idle");
        }
        if (current_state == state.crush && other.transform.tag == "Wall") {
            GameObject projectile = Instantiate(crush_effect, transform.position + new Vector3(direction, 0.5f, 0), crush_rotation);
            cycle();
            cycle_timer = Time.time + 15f;
        }
    }

    private void summon() {
        if (cycle_timer - Time.time >= 1.0f) {
            MoveTowards(player.transform.position);
        } else if (!summoned) {
            GameObject spawn1 = Instantiate(summons[0], transform.position + new Vector3(2f,-1.25f,0f), crushAttack.transform.rotation);
            GameObject spawn2 = Instantiate(summons[0], transform.position + new Vector3(-2f, -1.25f, 0f), crushAttack.transform.rotation);
            
            summoned = true;
        } else {
            MoveTowards(player.transform.position);
        }
    }

    public void MoveTowards(Vector3 location) {
        //if (Mathf.Abs(Vector3.Distance(transform.position, location)) >= 1) {
            rb.velocity = new Vector3(speed * Mathf.Sign(location.x - transform.position.x) * Time.deltaTime * 2.5f, 0, 0);
        //} else {
           // rb.velocity = new Vector3(0f, 0f, 0f);
       // }
    }

    public void crush() {
        if (crush_location == Vector3.zero) {
            crush_location = player.transform.position + new Vector3(0f, 8f, 0f);
            animator.Play("Smash");
            
        }
        if (!drop) {
            rb.velocity = Vector3.Normalize(crush_location - transform.position) * speed * 1.5f * Time.deltaTime;
            if (Vector3.Distance(crush_location, transform.position) <= 0.2f) {
                drop = true;
                GameObject crush = Instantiate(crushAttack, crushLocation.transform.position, crushLocation.transform.rotation);
                crush.transform.parent = transform;
            }
        } else {
            rb.velocity = Vector3.down * speed * 4 * Time.deltaTime;
        }
    }

    public void smash() {
        if (!drop) {
            GameObject projectile = Instantiate(smash_projectile, transform.position + new Vector3(direction, -1.15f, 0f), crush_rotation);
            Rigidbody projectile_rb = projectile.GetComponent<Rigidbody>();
            projectile_rb.velocity = new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
            projectile_rb.useGravity = false;

            

            drop = true;
        }
    }

    private void changeWeaponPosition(int position) {
        if (position == 0) {
            guitarObject.transform.parent = weaponObjectSlot.transform;
            guitarObject.transform.localPosition = guitarAttackPosition;
            guitarObject.transform.localRotation = guitarAttackRotation;
            guitarObject.transform.localScale = guitarAttackScale;
        }
        else if (position == 1) {
            guitarObject.transform.parent = modelSpine.transform;
            guitarObject.transform.localPosition = guitarStartPosition;
            guitarObject.transform.localRotation = guitarStartRotation;
            guitarObject.transform.localScale = guitarStartScale;
        }
    }

    //Used for animation event (You will need this exactly the same spelling / case / etc.)
    public void setDefaultPosition() {
        changeWeaponPosition(1);
    }

    public void takeDamage(float damage) {
        if (isEngaged) {
            if (currentHealthDisTime < Time.time) {
                enemygeneric.health -= damage;

                EnemyHealth.SetActive(true);
                EnemyHealth.GetComponent<Slider>().value = enemygeneric.health;

                currentHealthDisTime = Time.time + 0.5f;

                if (enemygeneric.health <= 0) {
                    dead = true;
                    //Instantiate(deathSound, transform.position, transform.rotation);
                    this.transform.tag = "Untagged";
                    Destroy(EnemyHealth.gameObject);
                    //Destroy(this.gameObject);
                    //animator.Play("Kneel");
                    animator.Play("Idle");
                    bossZone.GetComponent<Boss_Zone>().setIsDisabled(true);
                    finalDialogue.SetActive(true);
                    finalDialogue.transform.SetParent(null);
                }
            }
        }  
    }

    public void setIsEngaged(bool engage) {
        isEngaged = engage;
    }

}
