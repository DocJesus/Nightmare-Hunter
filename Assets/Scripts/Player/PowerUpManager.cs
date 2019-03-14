using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour {

    float timer;
    PlayerMovement _playerMovement;
    PlayerShooting _playerShooting;
    PlayerHealth _playerhealth;

    //[HideInInspector]
    //public float TimerHealth = 1f;

    public float TimerSpeedUP = 5f;
    public float TimerShootUp = 5f;
    public Image powerUPImage;

    public Sprite healthPowerUP;
    public Sprite SpeedPowerUP;
    public Sprite DamagePowerUp;

	// Use this for initialization
	void Start ()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponentInChildren<PlayerShooting>();
        _playerhealth = GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= TimerShootUp)
        {
            //Debug.Log("je reset la vitesse");
            _playerMovement.speed = 10f;
            powerUPImage.enabled = false;
        }
        if (timer >= TimerSpeedUP)
        {
            //Debug.Log("Je reset le shoot");
            _playerShooting.damagePerShot = 20;
            powerUPImage.enabled = false;
        }
    }

    public void PowerUp(int i)
    {
        Debug.Log("Je reçois un powerUp et i = " + i);
        switch(i)
        {
            case 0:
                Debug.Log("PowerUp de vitesse");
                _playerMovement.speed = 15f;
                _playerShooting.damagePerShot = 20;
                powerUPImage.sprite = SpeedPowerUP;
                //feedback visuel powerUP Vitesse
                break;
            case 1:
                Debug.Log("PowerUp de Dégat");
                _playerShooting.damagePerShot = 40;
                _playerMovement.speed = 10f;
                powerUPImage.sprite = DamagePowerUp;
                //feedback visuel powerUP Dégats
                break;
            case 2:
                Debug.Log("PowerUp de vie");
                _playerhealth.currentHealth += 20;
                powerUPImage.sprite = healthPowerUP;
                //feedback visuel powerUP Vie
                break;
        }
        powerUPImage.enabled = true;
        timer = 0;
    }
}
