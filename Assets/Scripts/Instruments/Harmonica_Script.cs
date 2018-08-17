using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harmonica_Script : Weapon {

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

        GameObject bullet = Instantiate(projectileObject, transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);

    }

    protected override void AdjustWeaponPosition() {
        
    }

    protected override void SpecialAttack1() {
        createdShield = Instantiate(shield, transform.parent.transform.position, transform.parent.transform.rotation);

        createdShield.transform.parent = gameObject.transform.parent;
        createdShield.transform.position +=new Vector3(0f,1f,-0.4f);
        playerObject.GetComponent<Player>().invulnerable = true;
    }
}
