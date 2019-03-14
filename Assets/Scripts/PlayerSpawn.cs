using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public static int[] playerControl = new int[4]; //à set de base à 1
    public int nbPlayer = 0;

    public GameObject[] UIHealth;
    public GameObject[] Player;
    public Image damageImage;
    public GameObject[] weapText;
    public List<GameObject> targets;

    // Use this for initialization
    void Start()
    {
        
                
        playerControl[0] = 0;
        playerControl[1] = 2;
        playerControl[2] = 0;
        playerControl[3] = 0;
        

        int i = 0;
        int j = 0;
        GameObject tmp;

        while (i < playerControl.Length)
        {
            if (playerControl[i] != 0)
            {
                transform.position = new Vector3(i + 3, 0, 0);
                tmp = Instantiate(Player[i], transform.position, transform.rotation);
                weapText[i].SetActive(true);
                UIHealth[i].SetActive(true);
                if (playerControl[i] == 2)
                {
                    tmp.GetComponent<PlayerMovement>().keyboarded = true;
                    tmp.GetComponentInChildren<PlayerShooting>().keyboarded = true;
                    tmp.GetComponent<PlayerMovement>().nbPlayer = 0;
                    tmp.GetComponentInChildren<PlayerShooting>().playerNumber = 0;
                }
                else
                {
                    tmp.GetComponent<PlayerMovement>().keyboarded = false;
                    tmp.GetComponentInChildren<PlayerShooting>().keyboarded = false;
                    j += 1;
                    tmp.GetComponent<PlayerMovement>().nbPlayer = j;
                    tmp.GetComponentInChildren<PlayerShooting>().playerNumber = j;
                }
                tmp.GetComponent<PlayerHealth>().health = UIHealth[i];

                tmp.GetComponent<PlayerHealth>().damageImage = damageImage;
                weapText[i].GetComponent<Text>().enabled = true;
                tmp.GetComponentInChildren<PlayerShooting>().weaponChoice = weapText[i].GetComponent<Text>();
                tmp.GetComponent<PowerUpManager>().powerUPImage = UIHealth[i].GetComponentsInChildren<Image>()[3];
                targets.Add(tmp);
            }
            i = i + 1;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	}
}
