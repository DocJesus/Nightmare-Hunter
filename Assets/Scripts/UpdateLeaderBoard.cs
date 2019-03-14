using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class UpdateLeaderBoard : MonoBehaviour {

    //static int[] scores = new int[5];
    public Text[] TextScore;

    string tmpScore;
    string[] allLines;
    bool izok = false;

    // Use this for initialization
    void Start ()
    {
        if (System.IO.File.Exists("./saves/score.txt"))
            allLines = System.IO.File.ReadAllLines(@"./saves/score.txt");

        int k;
        int j = 0;
        string tmp;

        //mettre les scores dans l'ordre
        while (j == 0)
        {
            izok = true;
            k = 0;
            while (k < allLines.Length - 2)
            {
                if (Int32.Parse(allLines[k]) < Int32.Parse(allLines[k + 1]))
                {
                    tmp = allLines[k];
                    allLines[k] = allLines[k + 1];
                    allLines[k + 1] = tmp;
                    izok = false;
                }
                k = k + 1;
            }
            if (izok)
                j = 1;
        }

        /*
        for (int i = 0; i < allLines.Length; i++)
            Debug.Log(i + " = " + allLines[i]);
            */

        for (int l = 0; l < TextScore.Length; l++)
        {
            //Debug.Log("l = " + l + " textScore.length = " + allLines.Length);
            if (l < allLines.Length)
                TextScore[l].text = (l + 1) + "................................." + allLines[l];
            else
                TextScore[l].text = (l + 1) + ".................................XXXX";
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

}
