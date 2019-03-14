using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;

    GameObject[] listofPlayer;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    int i;

    //CameraControl cam;

    void Awake()
    {
        listofPlayer = GameObject.FindGameObjectsWithTag("Player");
        player = getClosestPlayer();
        //playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        //cam = GetComponent<CameraControl>();
    }

    public GameObject getClosestPlayer()
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject g in listofPlayer)
        {
            if (g.activeSelf)
            {
                float dist = Vector3.Distance(currentPos, g.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = g;
                }
            }
        }
        return closest;
    }

    void Update ()
    {
        //si l'ennemi est tjs vivant poursuit sa target
        if(enemyHealth.currentHealth > 0)
        {
            if (player != null && player.activeSelf)
                nav.SetDestination (player.transform.position);
        }
        else
        {
            nav.enabled = false;
        }

        //cherche en permanance le joueur le plus proche
        player = getClosestPlayer();
    }
}
