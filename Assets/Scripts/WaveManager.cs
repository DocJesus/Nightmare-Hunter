using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{

    public int waveCount = 0;
    public Text waveText;

    Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
	}

    /*
	
	// Update is called once per frame
	void Update () {
		
	}
    */

    public void NextWave()
    {
        waveCount += 1;
        waveText.text = "Wave " + waveCount;
        animator.Play("WaveClip");
    }
}
