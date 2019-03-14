using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    AudioSource soundEffect;
    ParticleSystem particule;
    float timer;
    bool onetime = true;

	// Use this for initialization
	void Start ()
    {
        soundEffect = GetComponent<AudioSource>();
        particule = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int i = 0;

        if (other.gameObject.tag == "Player" && onetime)
        {
            onetime = false;
            Debug.Log("Supp nigga ?");
            soundEffect.Play();
            particule.Play();
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 2f);
            i = Random.Range(0, 3); //3 powerup
            other.gameObject.GetComponent<PowerUpManager>().PowerUp(i);
        }
    }
}
