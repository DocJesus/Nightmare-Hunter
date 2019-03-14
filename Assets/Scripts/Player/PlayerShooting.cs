using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum shootType
{
    PinkRifle,
    BlueRifle,
    Rocket
}

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float timeBetweenRocket = 1f;
    public float range = 100f;
    public float timeBetweenChange = 0.5f;
    public shootType _shootType = shootType.PinkRifle;
    public AudioClip rifleClip;
    public AudioClip rocketClip;
    public Transform ShellSpawn;
    public Rigidbody m_Shell;
    public int playerNumber;
    public Transform[] shootPoints;
    public LineRenderer scope;
    public bool keyboarded = true;
    public bool shoot;

    [HideInInspector]
    public Text weaponChoice;

    /* 
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;  
    public float MaxChargeTime = 0.75f;

    bool fire = false;
    float m_CurrentLaunchForce;
    float m_ChargeSpeed;
    */

    float timer;
    float weaponTimer = 0;
    float rocketTimer = 0;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles; //dépendent
    LineRenderer gunLine; //dépendent
    AudioSource gunAudio; //dépendent
    Light gunLight; //dépendent
    float effectsDisplayTime = 0.2f;
    int transShoot = 0;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        scope.enabled = true;
        shoot = false;
    }

    private void Start()
    {
        weaponChoice.text = "Pink Rifle";
        gunLine.material.color = Color.magenta;
        weaponChoice.color = Color.magenta;
    }

    void Update ()
    {
        timer += Time.deltaTime;
        weaponTimer += Time.deltaTime;
        rocketTimer += Time.deltaTime;
        //Debug.Log("PlayerNUmber = " + playerNumber);

        LightTarget();

        if (((!keyboarded && Input.GetButton("Player" + playerNumber + "JoystickSwitch")) || (keyboarded && (Input.GetMouseButton(1) /*|| Input.GetButton("switch" + playerNumber)*/))) && weaponTimer >= timeBetweenChange && Time.timeScale != 0)
        {
            if (_shootType == shootType.BlueRifle)
            {
                _shootType = shootType.Rocket;
                weaponChoice.text = "Rocket";
                weaponChoice.color = Color.yellow;
            }
            else if (_shootType == shootType.Rocket)
            {
                _shootType = shootType.PinkRifle;
                weaponChoice.text = "Pink Rifle";
                gunLine.material.color = Color.magenta;
                weaponChoice.color = Color.magenta;
            }
            else if (_shootType == shootType.PinkRifle)
            {
                _shootType = shootType.BlueRifle;
                weaponChoice.text = "Blue Rifle";
                gunLine.material.color = Color.blue;
                weaponChoice.color = Color.blue;
            }
            weaponTimer = 0;
        }

		if (((!keyboarded && Input.GetButton("Player" + playerNumber + "JoystickFire")) || (keyboarded && (Input.GetMouseButton(0) /*|| Input.GetButton("Fire" + playerNumber)*/))) && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            shoot = true;
        }

        if (shoot)
        {
            if (_shootType == shootType.BlueRifle || _shootType == shootType.PinkRifle)
            {
                Shoot();
            }
            if (_shootType == shootType.Rocket && rocketTimer >= timeBetweenRocket)
                LaunchRocket();
            shoot = false;
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot()
    {
        //SetDependencies();

        timer = 0f;
        if (gunAudio.clip != rifleClip)
            gunAudio.clip = rifleClip;
        gunAudio.Play();

        //Debug.Log("transShoot = " + transShoot + " shootPoint.length = " + shootPoints.Length); 

        if (transShoot >= shootPoints.Length)
        {
            transShoot = 0;
        }

        gunLight.transform.position = shootPoints[transShoot].position;
        gunParticles.transform.position = shootPoints[transShoot].position;

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        //gunLine.SetPosition (0, transform.position);

        gunLine.SetPosition(0, shootPoints[transShoot].position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            string tag = shootHit.collider.gameObject.tag;

            if (enemyHealth != null && ((tag == "bunny" && _shootType == shootType.BlueRifle) || (tag == "bear" && _shootType == shootType.PinkRifle)))
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
        transShoot += 1;
    }

    void LaunchRocket()
    {
        gunAudio.clip = rocketClip;
        gunAudio.Play();

        Rigidbody shellInstance = Instantiate(m_Shell, ShellSpawn.position, ShellSpawn.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = 75 * transform.forward; ;

        rocketTimer = 0;
    }

    void LightTarget()
    {
        scope.enabled = true;
        scope.SetPosition(0, transform.position);
        scope.SetPosition(1, transform.position + transform.forward * range);
    }

    public void shutDown()
    {
        scope.enabled = false;
        gunLine.enabled = false;
        gunLight.enabled = false;
    }
}
