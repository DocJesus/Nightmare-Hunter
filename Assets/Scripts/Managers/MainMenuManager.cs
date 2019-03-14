using UnityEngine;
using System.Collections;
using UnityEngine.UI; // include UI namespace since references UI Buttons directly
using UnityEngine.EventSystems; // include EventSystems namespace so can set initial input for controller support
using UnityEngine.SceneManagement; // include so we can load new scenes

public class MainMenuManager : MonoBehaviour {

    enum control
    {
        none,
        controler,
        keyboard
    }

    int[] PlayerControl = new int[4];

    public Text controllerText;

    public int nbPlayer = 0;

    //Selection des joueurs
    public Image P1Image;
    public Image P2Image;
    public Image P3Image;
    public Image P4Image;

    public Sprite ControllerSprite;
    public Sprite KeyboardSprite;
    public Sprite NullSprite;

    bool keyboardTaken = false;
    //----------------------------------

    // references to Submenus
    public GameObject _MainMenu;
	public GameObject _LevelsMenu;
	public GameObject _AboutMenu;
    public GameObject _nbPlayerMenu;

	// references to Button GameObjects
	public GameObject MenuDefaultButton; //
	public GameObject AboutDefaultButton;
	public GameObject LevelSelectDefaultButton;
    public GameObject NewGameButton;
    public GameObject LaunchGameButton;

    public GameObject QuitButton;

	// list the level names
	public string[] LevelNames;

	// reference to the LevelsPanel gameObject where the buttons should be childed
	public GameObject LevelsPanel;

	// reference to the default Level Button template
	public GameObject LevelButtonPrefab;
	
	// reference the titleText so we can change it dynamically
	public Text titleText;

	// store the initial title so we can set it back
	private string _mainTitle;

	// init the menu
	void Awake()
	{
		// store the initial title so we can set it back
		_mainTitle = titleText.text;

		// disable/enable Level buttons based on player progress
		setLevelSelect();

		// determine if Quit button should be shown
		displayQuitWhenAppropriate();

		// Show the proper menu
		ShowMenu("MainMenu");
        nbPlayer = 0;
	}

    private void Update()
    {

        if (_nbPlayerMenu.activeSelf && nbPlayer == 0)
        {
            LaunchGameButton.SetActive(false);
        }
        else if (_nbPlayerMenu.activeSelf && nbPlayer != 0)
        {
            LaunchGameButton.SetActive(true);
        }
    }

    // loop through all the LevelButtons and set them to interactable 
    // based on if PlayerPref key is set for the level.
    void setLevelSelect() {
		// turn on levels menu while setting it up so no null refs
		_LevelsMenu.SetActive(true);

		// loop through each levelName defined in the editor
		for(int i=0;i<LevelNames.Length;i++) {
			// get the level name
			string levelname = LevelNames[i];

			// dynamically create a button from the template
			GameObject levelButton = Instantiate(LevelButtonPrefab,Vector3.zero,Quaternion.identity) as GameObject;

			// name the game object
			levelButton.name = levelname+" Button";

			// set the parent of the button as the LevelsPanel so it will be dynamically arrange based on the defined layout
			levelButton.transform.SetParent(LevelsPanel.transform,false);

			// get the Button script attached to the button
			Button levelButtonScript = levelButton.GetComponent<Button>();

			// setup the listener to loadlevel when clicked
			levelButtonScript.onClick.RemoveAllListeners();
			levelButtonScript.onClick.AddListener(() => loadLevel(levelname));

			// set the label of the button
			Text levelButtonLabel = levelButton.GetComponentInChildren<Text>();
			levelButtonLabel.text = levelname;

			// determine if the button should be interactable based on if the level is unlocked
			if (PlayerPrefManager.LevelIsUnlocked (levelname)) {
				levelButtonScript.interactable = true;
			} else {
				levelButtonScript.interactable = false;
			}
		}
	}

	// determine if the QUIT button should be present based on what platform the game is running on
	void displayQuitWhenAppropriate() 
	{
		switch (Application.platform) {
            // platforms that should have quit button
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.LinuxPlayer:
				QuitButton.SetActive(true);
				break;

			// platforms that should not have quit button
			// note: included just for demonstration purposed since
			// default will cover all of these. 
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.WebGLPlayer:
                QuitButton.SetActive(false);
				break;

			// all other platforms default to no quit button
			default:
				QuitButton.SetActive(false);
				break;
		}
	}

	// Public functions below that are available via the UI Event Triggers, such as on Buttons.

	// Show the proper menu
	public void ShowMenu(string name)
	{
		// turn all menus off
		_MainMenu.SetActive (false);
		_AboutMenu.SetActive(false);
		_LevelsMenu.SetActive(false);
        _nbPlayerMenu.SetActive(false);


		// turn on desired menu and set default selected button for controller input
		switch(name) {
		case "MainMenu":
			_MainMenu.SetActive (true);
			EventSystem.current.SetSelectedGameObject (MenuDefaultButton);
			titleText.text = _mainTitle;
			break;
		case "LevelSelect":
			_LevelsMenu.SetActive(true);
			EventSystem.current.SetSelectedGameObject (LevelSelectDefaultButton);
			titleText.text = "Level Select";
			break;
		case "About":
			_AboutMenu.SetActive(true);
			EventSystem.current.SetSelectedGameObject (AboutDefaultButton);
			titleText.text = "About";
			break;
        case "NumberPlayer":
            _nbPlayerMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(NewGameButton);
            titleText.text = "Select Nomber of Player";
            break;                
        }
	}

	// load the specified Unity level
	public void loadLevel(string levelToLoad)
	{
        PlayerSpawn.playerControl = PlayerControl; //potentiel changement sur quel personnage on envoie, genre on peut prendre le player 1 & 3 et jouer à 2;
        EnemyManager.nbEnnemy = 0;
		SceneManager.LoadScene(levelToLoad);
	}

    //set the controller of the player and add a player
    public void ChangeControll(int num)
    {
        int value;

        value = num / num;
        if (num < 0)
        {
            value = value * (-1);
            num = num * (-1);
        }

        num = num - 1;

        switch (num)
        {
            case 0:
                IsAllowed(num, value);
                ChangeSprite(P1Image, PlayerControl[0]);
                break;
            case 1:
                IsAllowed(num, value);
                ChangeSprite(P2Image, PlayerControl[1]);
                break;
            case 2:
                IsAllowed(num, value);
                ChangeSprite(P3Image, PlayerControl[2]);
                break;
            case 3:
                IsAllowed(num, value);
                ChangeSprite(P4Image, PlayerControl[3]);
                break;
        }
        Debug.Log("nbPlayer = " + nbPlayer);

    }

    void IsAllowed(int num, int value)
    {
       
        //Debug.Log("keyboardTaken = " + keyboardTaken);

        PlayerControl[num] += value;

        //cas où on monte (value > 0)
        if (value > 0)
        {
            Debug.Log("passage Positif");
            //si la valuer était le keyboard, on le libère
            if (PlayerControl[num] - value == 2)
                keyboardTaken = false;
            //cas de 1 à 2
            if (PlayerControl[num] == 2 && keyboardTaken)
            {
                PlayerControl[num] = 0;
            }
            else if (PlayerControl[num] == 2 && !keyboardTaken)
                keyboardTaken = true;
            //cas de 2 à 0
            if (PlayerControl[num] > 2)
                PlayerControl[num] = 0;
        }

        //cas où on descends (value < 0)
        if (value < 0)
        {
            //si la valuer était keyboard, on le libère
            if (PlayerControl[num] - value == 2)
                keyboardTaken = false;
            //cas de 0 à 2 (ou 1)
            if (PlayerControl[num] < 0)
                PlayerControl[num] = 2;
            if (keyboardTaken && PlayerControl[num] == 2)
                PlayerControl[num] = 1;
            else if (PlayerControl[num] == 2 && !keyboardTaken)
                keyboardTaken = true;
        }
    }

    void ChangeSprite(Image sprite, int value)
    {
        switch (value)
        {
            case (0):
                sprite.sprite = NullSprite;
                break;
            case (1):
                sprite.sprite = ControllerSprite;
                break;
            case (2):
                sprite.sprite = KeyboardSprite;
                break;
        }
    }

	// quit the game
	public void QuitGame()
	{
		Application.Quit ();
	}
}
