using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public CameraControl cam;

    public GameObject[] enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public WaveManager waveManager;

    public static int nbEnnemy = 0;

    void Start ()
    {
    }

    private void Update()
    {
        int currentWave;

        if (nbEnnemy == 0)
        {
            waveManager.NextWave();
            currentWave = waveManager.waveCount;
            for (int i = 0; i <= (currentWave * 2); ++i)
                Spawn();
        }
    }


    void Spawn ()
    {
        if(cam.allPlayerDead())
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int whoSpawn = Random.Range(0, enemy.Length);
        Instantiate(enemy[whoSpawn], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        nbEnnemy += 1;
    }


}
