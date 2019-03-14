using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    [HideInInspector]
    public Slider healthSlider;
    public GameObject health;
    [HideInInspector]
    public Image damageImage;

    //Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    IEnumerator coroutine;
    float timeDeath = 2f;

    void Awake ()
    {
        //anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    public void TakeDamage (int amount)
    {
        damaged = true;

        currentHealth -= amount;

        health.GetComponentInChildren<Slider>().value = currentHealth;

        playerAudio.Play ();

       if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        //anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();
        playerMovement.enabled = false;
        playerShooting.enabled = false;

        GetComponent<Rigidbody>().isKinematic = true;

        //lancer une coroutine
        coroutine = SinkingPlayer();
        StartCoroutine(coroutine);
        playerShooting.shutDown();
        playerShooting.weaponChoice.gameObject.SetActive(false); //cacher le text d'arme quand le joueur meurt
        health.SetActive(false); //désactive l'UI de health;
    }

    private IEnumerator SinkingPlayer()
    {
        while (timeDeath >= 0.0f)
        {
            transform.Rotate(-Vector3.right * Time.deltaTime * 75);
            transform.Translate(-Vector3.up * Time.deltaTime);
            timeDeath -= Time.deltaTime;
            yield return null;
        }
        transform.Translate(-Vector3.down * 3);
        this.gameObject.SetActive(false);
        yield return null; 
    }

    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }
}
