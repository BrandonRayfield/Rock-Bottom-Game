using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Smash variables
    public GameObject smash_projectile;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        direction = (int)Mathf.Sign((player.transform.position.x - transform.position.x));
    }
	
	// Update is called once per frame
	void Update () {

        switch (current_state) {
            case state.charge:
                charge();
                break;
            case state.summon:
                //summon();
                break;
            case state.crush:
                crush();
                break;
            case state.smash:
                smash();
                break;
        }

        
        if (cycle_timer < Time.time) cycle();

        
       

    }

    public void cycle() {
        //Get a new state
        /*if (transform.rotation.z != 0) {
            transform.Rotate(Vector3.forward * 55 * direction);
        }*/

        //Reset variables
        crush_location = Vector3.zero;
        drop = false;

        int new_state = Random.Range(1, 5);
        if (new_state > 5) new_state = 5;
        switch (new_state) {
            case 1:
                //transform.Rotate(Vector3.forward * 55 * -direction);
                animator.Play("Charge");
                current_state = state.charge;
                break;
            case 2:
                
                current_state = state.crush;
                break;
            case 3:
                animator.Play("Roar");
                current_state = state.smash;
                break;
            case 4:
                summoned = false;
                current_state = state.summon;
                break;
            case 5:
                current_state = state.idle;
                break;
        }
        //change direction
        if (direction != (int)Mathf.Sign(player.transform.position.x - transform.position.x)) {
            direction = (int)Mathf.Sign((player.transform.position.x - transform.position.x));
            transform.Rotate(Vector3.up, 180f);
        }

        //reset timer
        cycle_timer = Time.time + 3f;
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
            Instantiate(summons[0], transform.position + new Vector3(2f,-1.25f,0f), transform.rotation);
            Instantiate(summons[0], transform.position + new Vector3(-2f, -1.25f, 0f), transform.rotation);
            summoned = true;
        } else {
            MoveTowards(player.transform.position);
        }
    }

    public void MoveTowards(Vector3 location) {
        if (Mathf.Abs(transform.position.x - location.x) >= 1) {
            rb.velocity = new Vector3(speed * Mathf.Sign(transform.position.x - location.x) * Time.deltaTime * 2.5f, 0, 0);
        } else {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    public void crush() {
        if (crush_location == Vector3.zero) {
            crush_location = player.transform.position + new Vector3(0f, 8f, 0f);
            animator.Play("Smash");
            
        }
        if (!drop) {
            rb.velocity = Vector3.Normalize(crush_location - transform.position) * speed * 1.5f * Time.deltaTime;
            if (Vector3.Distance(crush_location, transform.position) <= 0.2f) drop = true;
        } else {
            rb.velocity = Vector3.down * speed * 4 * Time.deltaTime;
        }
    }

    public void smash() {
        if (!drop) {
            GameObject projectile = Instantiate(smash_projectile, transform.position + new Vector3(direction, 0.25f, 0f), crush_rotation);
            Rigidbody projectile_rb = projectile.GetComponent<Rigidbody>();
            projectile_rb.velocity = new Vector3(direction * speed * Time.deltaTime, 0f, 0f);
            projectile_rb.useGravity = false;

            

            drop = true;
        }
    }
}
