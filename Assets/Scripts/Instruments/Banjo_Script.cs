using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banjo_Script : Weapon {

    //Magic Variables
    public GameObject lightningControl;
    public float lightningDamage = 100;
    private float currentChannelTime;
    public float channelTime = 1.0f; // How long it takes for attack to charge before actually activating
    public Vector3 target;
    private Vector3 nullTarget = new Vector3(0, 0, 0);

    public GameObject fireSpell;

    private float angle;
    private GameObject bullet;
    
    //Rotation vars
    private float rotationSpeed = 1.5f;
    private float adjRotSpeed;

    protected override void Update() {
        base.Update();

        Vector3 mousePos;
        Vector3 attackPos = damageLocation.transform.position;

        mousePos = Input.mousePosition;
        mousePos.z = Vector3.Distance(Camera.main.transform.position, transform.position);
        attackPos = Camera.main.WorldToScreenPoint(attackPos);
        mousePos.x -= attackPos.x;
        mousePos.y -= attackPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (bullet != null) {
            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
            bullet.gameObject.transform.rotation = Quaternion.Lerp(bullet.transform.rotation, Quaternion.Euler(new Vector3(-angle, 90, 0)), adjRotSpeed);
            //bullet.gameObject.transform.rotation = new Quaternion(Mathf.Lerp(bullet.transform.rotation.x, -angle, adjRotSpeed), bullet.transform.rotation.y, bullet.transform.rotation.z, 1);
        }

        //target = lightningControl.GetComponent<ThunderCollider>().findClosest();
        //Debug.Log(target);
        //if (target != new Vector3(0, 0, 0)) {
        //    canUse1 = true;
        //} else {
        //    canUse1 = false;
        //}
    }

    protected override void SpecialAttack1() {

        //playerObject.GetComponent<Player>().setCurrentlyCasting(true);
        animator.SetTrigger("Playing");
        Instantiate(magicSound1, transform.position, transform.rotation);
        bullet = Instantiate(fireSpell, damageLocation.transform.position, Quaternion.Euler(new Vector3(-angle, 90, 0)), damageLocation.gameObject.transform);
        magicTimer1 = Time.time + magicRate1;

        //Invoke("setCastFalse", 4);

        //bullet.GetComponent<Projectile_Script>().SetDamage(weaponDamage);
        //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.right * bulletSpeed);
    }

    private void setCastFalse() {
        playerObject.GetComponent<Player>().setCurrentlyCasting(false);
    }

}
