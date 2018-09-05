using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaphone_Script : Weapon {

    public int clipSize;
    public float bulletSpeed;
    public GameObject projectileObject;
    public int ammoCount;

    //Magic
    public GameObject shield;
    private GameObject createdShield;


    protected override void Damage() {
        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;
        float angle;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(projectileObject, damageLocation.transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);

    }

    protected override void AdjustWeaponPosition() {
        // Determines what harmonica object is enabled. 0 for Back enabled, 1 for stage, 2 for attack
        if (guitarStance == 1) {
            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(0.02f, 2.057f, 0.779f);
            //this.gameObject.transform.localEulerAngles = new Vector3(0.0f, -90.00001f, 0.0f);

            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentSpine.transform;

        } else if (guitarStance == 2) {
            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentHand.transform;

            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(-0.4303697f, 0.6051218f, 0.6971588f);
            //this.gameObject.transform.localEulerAngles = new Vector3(2.145f, 51.479f, 0.791f);

        } else {
            // Adjusts the harmonica objects position and rotation
            //this.gameObject.transform.localPosition = new Vector3(-0.2714013f, -0.1477957f, -0.0788268f);
            //this.gameObject.transform.localEulerAngles = new Vector3(65.565f, -37.631f, 85.8f);


            // Adjusts the harmonica object parent so it moves with the body
            //this.gameObject.transform.parent = guitarParentSpine.transform;
        }
    }

    protected override void SpecialAttack1() {
        createdShield = Instantiate(shield, transform.parent.transform.position, transform.parent.transform.rotation);

        createdShield.transform.parent = gameObject.transform.parent;
        createdShield.transform.position += new Vector3(0f, 1f, 0.1f);
        playerObject.GetComponent<Player>().invulnerable = true;
    }

    protected override void SpecialAttack2() {
        createdShield = Instantiate(shield, transform.parent.transform.position, transform.parent.transform.rotation);

        createdShield.transform.parent = gameObject.transform.parent;
        createdShield.transform.position += new Vector3(0f, 1f, 0.1f);
        playerObject.GetComponent<Player>().invulnerable = true;
    }
}
