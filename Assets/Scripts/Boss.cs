using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public enum state { charge, smash, crush, summon, idle};
    public bool started = false;

    public state current_state = state.idle;

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
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        rb = GetComponent<Rigidbody>();
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

        int new_state = Random.Range(1, 8);
        if (new_state > 5) new_state = 5;
        switch (new_state) {
            case 1:
                //transform.Rotate(Vector3.forward * 55 * -direction);
                current_state = state.charge;
                break;
            case 2:
                current_state = state.crush;
                break;
            case 3:
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
        direction = (int)Mathf.Sign((player.transform.position.x - transform.position.x));
        //reset timer
        cycle_timer = Time.time + 3f;
    }

    public void charge() {
        rb.velocity = new Vector3(speed * direction * Time.deltaTime * 2.5f, 0, 0);
    }

    public void OnTriggerEnter(Collider other) {
        if (current_state == state.charge && other.transform.tag == "Wall") {
            cycle();
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
        Vector3 above_player = player.transform.position + new Vector3(0f, 9f,0f);
        

        rb.velocity = Vector3.Normalize(above_player - transform.position) * speed * Time.deltaTime;
    }
}
