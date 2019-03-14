using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class GameOverManager : MonoBehaviour
{
    public float loadingDelay = 2f;
    public string leveltoload;
    public CameraControl cam;

    Animator anim;
    float restartTimer = 0f;
    int tmp;
    int fd;
    bool oneTime = false;

    private void Start()
    {
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        //Debug.Log("gameOvermanagerUpdate");
        if (cam.allPlayerDead())
        {
            if (!oneTime)
            {
                GameOver();
                oneTime = true;
            }
            restartTimer += Time.deltaTime;

            if (restartTimer >= loadingDelay)
            {
                Debug.Log("Loading");
                SceneManager.LoadScene(leveltoload);
            }
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        anim.SetTrigger("GameOver");

        tmp = ScoreManager.score;
        string tmpScore = tmp.ToString();
        Debug.Log("tmpScore = " + tmpScore);

        bool yesExists = System.IO.File.Exists("./saves/score.txt");
        if (!yesExists)
        {
            Debug.Log("le fichier n'existe pas alors je le créer");
            var fileStream = new FileStream(@"./saves/score.txt", FileMode.Create);
            fileStream.Close();
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"./saves/score.txt", true))
        {
            file.WriteLine(tmpScore);
        }
        GetComponent<WaveManager>().enabled = false;
    }
}
