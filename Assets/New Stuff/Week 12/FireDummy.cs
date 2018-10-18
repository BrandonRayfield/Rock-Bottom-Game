using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireDummy : MonoBehaviour
{

    //Health Bar Objects
    public GameObject EnemyHealthBar;
    private Transform healthBarTarget;
    private GameObject EnemyHealth;
    private bool hasBeenDamaged;
    private float healthBarDisappearTime = 3.0f;
    private float currentHealthDisTime;
    private bool dead;

    private float invisTime;

    private EnemyGeneric enemygeneric;

    public GameObject damageSound;

    // Use this for initialization
    void Start() {
        enemygeneric = GetComponent<EnemyGeneric>();
        enemygeneric.health = 100;
        enemygeneric.maxHealth = enemygeneric.health;

        EnemyHealth = Instantiate(EnemyHealthBar);
        EnemyHealth.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);

        EnemyHealth.SetActive(false);
    }

    private void Awake() {
        healthBarTarget = gameObject.transform;
    }

    // Update is called once per frame
    void Update() {

        if (hasBeenDamaged) {
            currentHealthDisTime += Time.deltaTime;
            if (currentHealthDisTime >= healthBarDisappearTime) {
                EnemyHealth.SetActive(false);
                hasBeenDamaged = false;
                currentHealthDisTime = 0;
            }
        }

        //Updating position of enemy healthbar

        EnemyHealth.transform.position = Camera.main.WorldToScreenPoint(new Vector3(healthBarTarget.position.x, healthBarTarget.position.y + 1, healthBarTarget.position.z));

    }

    public void takeDamage(float damage) {
        if (invisTime < Time.time) {

            enemygeneric.health -= damage;

            Instantiate(damageSound, transform.position, transform.rotation);

            EnemyHealth.SetActive(true);
            EnemyHealth.GetComponent<Slider>().value = (enemygeneric.health / enemygeneric.maxHealth);

            invisTime = Time.time + 0.5f;
            hasBeenDamaged = true;

            if (enemygeneric.health <= 0) {
                dead = true;
                Instantiate(damageSound, transform.position, transform.rotation);
                Destroy(EnemyHealth);
                Destroy(gameObject);
                this.transform.tag = "Untagged";
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "AbilityFire") {
            takeDamage(25);
        }

    }
}
