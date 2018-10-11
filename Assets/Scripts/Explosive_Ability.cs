using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Ability : MonoBehaviour {

    private float time = 0.0f;
    private float lifeTime = 3.0f;
    private float damage;

    // Use this for initialization
    void Start() {
        damage = 100;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time >= lifeTime)
            Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.takeDamage(damage);
        }
    }
}
