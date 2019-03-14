using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    EnemyMovement enemyMove;
    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake ()
    {
        enemyMove = GetComponent<EnemyMovement>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Update ()
    {
        player = enemyMove.player;
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();

        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack ();
        }

        if(playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }
    }

}
