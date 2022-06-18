using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSystem : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    public float fireCooldown = 0;
    public float reloadCooldown = 0;

    public AudioSource audioSource;
    ActiveWeapon activeWeapon;

    public EquippableItem weapon;
    public bool canFire = true;
    int addableAmmo;
    public GameObject rayPoint;
    public Animator animator;
    public ParticleSystem MuzzleFlash;

    virtual protected void Start()
    {
        activeWeapon = Object.FindObjectOfType<ActiveWeapon>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        weapon.rayPoint = rayPoint;
        weapon.muzzleFlash = MuzzleFlash;
    }
    virtual protected void Update()
    {
        CheckFire();
        CheckReload();
    }
    
    virtual protected void CheckFire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= fireCooldown && weapon.AmmoInGun > 0 && canFire)
        {
            Fire();
            fireCooldown = Time.time + weapon.fireRate;
        }
    }

    virtual protected void CheckReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && weapon.AmmoInPocket > 0 && Time.time >= reloadCooldown)
        {
            StartCoroutine(Reload());
            reloadCooldown = Time.time + weapon.reloadRate;
        }
    }
    virtual protected void Fire()
    {
        //activeWeapon.activeWeapon.AmmoInGun--;
        //weapon.AmmoInGun--;
        audioSource.clip = weapon.fireSound;
        audioSource.Play();
        weapon.muzzleFlash.Play();
    }
    virtual protected IEnumerator Reload()
    {
        //Reload Anim      animator.SetTrigger("reloading");
        canFire = false;
        addableAmmo = weapon.AmmoMax - weapon.AmmoInGun;
        if (addableAmmo > weapon.AmmoInPocket)
            addableAmmo = weapon.AmmoInPocket;
        else
            addableAmmo = weapon.AmmoMax - weapon.AmmoInGun;
        yield return new WaitForSeconds(2f);
        weapon.AmmoInGun += addableAmmo;
        weapon.AmmoInPocket -= addableAmmo;
        canFire = true;
    }

}
