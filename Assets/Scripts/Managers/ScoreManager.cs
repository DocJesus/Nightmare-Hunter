using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    Text text;
    int oldscore = 0;
    float MulTime = 0;
    Animator anim;

    public GameObject Slider;
    Slider comboSlider;
    public static int score;
    public static int Multiplicator = 0;
    public Text comboText;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        text = GetComponent <Text> ();
        score = 0;
    }

    private void Start()
    {
        comboSlider = Slider.GetComponentInChildren<Slider>();
    }

    void Update ()
    {
        text.text = "Score: " + score;

        if (score != oldscore)
        {
            oldscore = score;
            MulTime = Time.time;
            Multiplicator += 1;
            anim.Play("ComboClip");
        }

        if ((Time.time - MulTime) > 5f)
        {
            Multiplicator = 1;
            comboText.enabled = false;
            Slider.SetActive(false);
            comboSlider.value = 5;
        }

        if (Multiplicator > 1)
        {
            Slider.SetActive(true);
            comboText.enabled = true;
            comboText.text = "Combo X" + Multiplicator;
            comboSlider.value = 5 - ((Time.time - MulTime));
        }
    }

}
