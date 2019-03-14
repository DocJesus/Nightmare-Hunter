using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // include so we can load new scenes

public class MenuPauseAndLoadLevel : MonoBehaviour {

	public string levelToLoad;
	public float delay;
    public float showInstruction;
    public Animator animFirstText;
    public Animator animSecondText;
    public GameObject secondInstruction;
    public GameObject firstInstruction;

    private float time;

    private void Start()
    {
        firstInstruction.SetActive(true);
        secondInstruction.SetActive(false);
        time = 0;   
    }

    void Update ()
    {
        time += Time.deltaTime;
        if (time >= showInstruction && firstInstruction.activeSelf)
        {
            Debug.Log("je suis dans le premier");
            animFirstText.SetTrigger("ShowInstruction");
            if (Input.anyKey)
            {
                firstInstruction.SetActive(false);
                secondInstruction.SetActive(true);
                time = 0;
            }
        }

            if (time >= showInstruction + 2 && secondInstruction.activeSelf)
            {
            Debug.Log("je suis dans le second");
                animSecondText.SetTrigger("ShowInstruction");
                if (Input.anyKey)
                {
                    LoadLevel();
                }
            }

    }

	// load the specified level
	void LoadLevel() {
		SceneManager.LoadScene(levelToLoad);
	}
}
