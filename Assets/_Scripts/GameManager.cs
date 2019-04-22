using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Public Variables
    public List<string> diaryPages;
    public bool diaryIsActive = false;
    public Text diaryText;
    public GameObject logReaderObj;
    public bool correct = false;
    public float powerVar = 0f;
    public int count = 0;
    public bool addText = false;
    public List<float> powerList;
    public int numOfWindmill = 0;

    //Private Variables
    private GameObject[] windMills;
    private GameObject windmillParent;
    private GameObject workingWindMill;
    private Enum_WM.Position correctPos;
    private AudioClip powerOn;
    private bool playSound = true;
    private AudioSource audioSource;
    private GameObject winText;
	private GameObject arrow;
    private GameObject powerBarUI;
    private Transform player;
    private int countDiary = 0;
    private GameObject message;
    private float elapedTime = 0f;
    private bool showMessage = false;
    private bool isTopDown;
    private GameObject pauseMenu;
    private bool pause = false;
    private GameObject droneControlPanel;
    private GameObject panelDrone;
    
    private FirstPersonController fpsController;
    private WindMillManager windMillManager;
    private AudioClip paperSound;
    private Text powerText;
    private GameObject multimeterUIObject;
    private TextMesh powerTextStation;
    private GameObject endText;
    private GameObject leftButton;
    private GameObject rightButton;
	[SerializeField] private GameObject[] gears;
	[SerializeField] private GameObject prize;
	public int num = 0;




    //Properties
    public Enum_WM.Position CorrectPos
    {
        get { return correctPos; }
        set { correctPos = value; }
    }

    public GameObject PowerBarUI
    {
        get { return powerBarUI; }
        set{ powerBarUI = value;}
    }

    public bool IsTopDown
    {
        get { return isTopDown; }
        set { isTopDown = value; }
    }

    public bool ShowMessage
    {
        get { return showMessage; }
        set { showMessage = value; }
    }

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);

        leftButton = GameObject.Find("LeftArrow");
        rightButton = GameObject.Find("RightArrow");

        if (GameObject.Find("Message"))
            message = GameObject.Find("Message");
		

		if (GameObject.Find ("GearBox")) 
		{
			prize = GameObject.Find ("GearBox");
			prize.SetActive (false);
		}
    }


    // Use this for initialization
    void Start () {

        if (GameObject.Find("DroneControlButton"))
            droneControlPanel = GameObject.Find("DroneControlButton");

        panelDrone = GameObject.FindGameObjectWithTag("PanelDrone");

        if (!droneControlPanel)
            panelDrone.SetActive(false);

        //For the text at the end of the windmill puzzle
        if (GameObject.FindGameObjectWithTag("EndText"))
        {
            endText = GameObject.FindGameObjectWithTag("EndText");
            //endText.SetActive(false);
        }

        //Powertext
        if (GameObject.FindGameObjectWithTag("PowerText"))
            powerText = GameObject.FindGameObjectWithTag("PowerText").GetComponent<Text>();

        //PowerTextStation
        if (GameObject.FindGameObjectWithTag("PowerTextStation"))
            powerTextStation = GameObject.FindGameObjectWithTag("PowerTextStation").GetComponent<TextMesh>();
        
        if (GameObject.FindGameObjectWithTag("Multimeter"))
            multimeterUIObject = GameObject.FindGameObjectWithTag("Multimeter");

        //Audio management
        audioSource = gameObject.GetComponent<AudioSource>();
        powerOn = (AudioClip)Resources.Load("Audio/SFX/Windmills working sound Effect");
        paperSound = (AudioClip)Resources.Load("Audio/SFX/paper sound");

        if (SceneManager.GetActiveScene().buildIndex.Equals(0) || SceneManager.GetActiveScene().name.Equals("Second_Test")) 
        {
            if (GameObject.FindGameObjectWithTag("WindmillManager"))
            windMillManager = GameObject.FindGameObjectWithTag("WindmillManager").GetComponent<WindMillManager>();
        else
            Debug.LogError("<color=red>Error:</color> No object in hierarchy tagged \"WindmillManager\" are present. Please tag one and attach WindMillManager script if not present!", this);
        }

        /*if (GameObject.Find("WindMills"))
        {
            windmillParent = GameObject.Find("WindMills");
            windMills = GameObject.Find("WindMills").GetChildrenList();
            workingWindMill = GameObject.Find("WorkingWindMill").transform.GetChild(0).gameObject;

            if (workingWindMill.GetComponent<WMMotorRotation>())
                CorrectPos = workingWindMill.GetComponent<WMMotorRotation>().position;
        }*/

        //Text on console at command center to show the player they fixed windmills
        if (GameObject.Find("Win!"))
        {
            winText = GameObject.Find("Win!");
            winText.SetActive(false);
            SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(10, "WINDMILLS COMPLETED", 0, 0, 0);
            SimTracker.SimTracker.Instance().PushEvent(progreso);
        }

        //Arrow over the cabin
        if(GameObject.FindGameObjectWithTag("Arrow"))
		    arrow = GameObject.FindGameObjectWithTag ("Arrow");

        //Player Variables
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fpsController = player.GetComponent<FirstPersonController>();

        //Log Reader and diary.
        logReaderObj = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(6).gameObject;
        logReaderObj.SetActive(false);
        diaryText = logReaderObj.transform.GetChild(0).GetComponent<Text>();

        if (GameObject.FindGameObjectWithTag("PowerBar"))
            powerBarUI = GameObject.FindGameObjectWithTag("PowerBar");
        else
            Debug.LogError("<color=red>Missing:</color> No object on canvas tagged \"PowerBar\"! Ensure it's there and correctly tagged.", this);

        if (powerText)
        {
            for (int i = 0; i < 5; i++)
            {
                powerList.Add(0);
            }
        }


        // TRACKER INITIALIZATION
        SimTracker.SimTracker.Instance();

    }
		
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {

            if (Camera.main.GetComponent<DroneMouseLook>())
                Camera.main.GetComponent<DroneMouseLook>().enabled = false;

            fpsController.isTopDown = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
       
        }

        if (powerText)
        {

            if (numOfWindmill != 0)
            {
                if (powerVar >= 4f)
                {
                    Debug.Log(powerVar);
                    powerList[numOfWindmill] = Mathf.Lerp(powerList[numOfWindmill], powerVar, Time.deltaTime);
                }
            }

            if (SceneManager.GetActiveScene().name.Equals("01IntroLevel"))
            {
               endText.GetComponent<TextMesh>().text = "Windmill: " + System.Math.Round(powerList[1], 2) + " MW";
            }
            else
            {
				if(endText)
                	endText.GetComponent<TextMesh>().text = "Windmill " + 1 + ": " + System.Math.Round(powerList[1], 2) + " MW" +
                                                "Windmill " + 2 + ": " + System.Math.Round(powerList[2], 2) + " MW \n" +
                                             "Windmill " + 3 + ": " + System.Math.Round(powerList[3], 2) + " MW" +
                                         "Windmill " + 4 + ": " + System.Math.Round(powerList[4], 2) + " MW \n";
            }

        }

        if (powerTextStation)
        {
            if (SceneManager.GetActiveScene().name.Equals("01IntroLevel"))
            {
                powerTextStation.text = "Windmill: " + System.Math.Round(powerList[1], 2) + " MW";
            }
            else
            {
                powerTextStation.text = "Windmill " + 1 + ": " + System.Math.Round(powerList[1], 2) + " MW " +
                                "Windmill " + 2 + ": " + System.Math.Round(powerList[2], 2) + " MW \n" +
                                 "Windmill " + 3 + ": " + System.Math.Round(powerList[3], 2) + " MW " +
                                     "Windmill " + 4 + ": " + System.Math.Round(powerList[4], 2) + " MW \n";
            }
        }

        //Open diary with Q and apply the newest text to reader.
        if(Input.GetKeyDown(KeyCode.Q) && !IsTopDown)
        {
            if(diaryPages.Count != 0)
            {
                audioSource.PlayOneShot(paperSound);
                countDiary = diaryPages.Count - 1;
                diaryText.text = diaryPages[countDiary];
                diaryIsActive = !diaryIsActive;
            }
        }

        //Check if diary is active and allow player to use the mouse.
        if (diaryIsActive)
        {
            if (powerBarUI)
                powerBarUI.SetActive(false);    //Hide power bar while reading diary
            if (multimeterUIObject)
                multimeterUIObject.SetActive(false);

            logReaderObj.SetActive(true);
            fpsController.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (countDiary == 0)
                leftButton.SetActive(false);
            else
                leftButton.SetActive(true);

            if ((countDiary == diaryPages.Count - 1 && countDiary != 0) || countDiary == diaryPages.Count - 1)
                rightButton.SetActive(false);
            else
                rightButton.SetActive(true);

        }
        else
        {

            if (powerBarUI)
                powerBarUI.SetActive(true); //Show power bar again
            if (multimeterUIObject)
                multimeterUIObject.SetActive(true);

            logReaderObj.SetActive(false);
            fpsController.enabled = true;

            //if the camera is not in top down mode and dairy is not active
            if(fpsController.isTopDown == false && !pause)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        ////Distance check for arrow.
        //if (arrow) 
        //{
        //    if (Vector3.Distance(arrow.transform.position, player.position) <= 40f)
        //        arrow.SetActive(false);
        //    else
        //        arrow.SetActive(true);
        //}

        //If all windmills are correctly rotated, play a sound and show the text on console
        if (windMillManager != null) 
        {
            if (windMillManager.AllCorrect && playSound)
            {
                audioSource.PlayOneShot(powerOn);
                playSound = false;              //Set to false to prevent another iteration in the next update

                //Message for telling the player where to go
                //message.GetComponent<Text>().text = "Energy restored! Go back to the power station";
                //showMessage = true;

                if (endText) 
                {
                    endText.GetComponent<TextMesh>().text = "Great Job! \n Now turn on the power station";    //Activate the puzzle solved text
                                                                                                              //Destroy(winText, 5f);       //Destroy it after 5 seconds
                    SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(10, "POWER STATION PREPARED", 0, 0, 0);
                    SimTracker.SimTracker.Instance().PushEvent(progreso);
                }
            }
        }

        if (showMessage)
        {
            elapedTime += Time.deltaTime;
            if (elapedTime >= 5f)
            {
                message.SetActive(false);
                showMessage = false;
            }
        }

		CheckGears ();
    }

    //Assign to left log reader button, sets the cound down if not the first page.
    public void LeftClickButton()
    {
        if(countDiary != 0)
        {
            countDiary--;
            diaryText.text = diaryPages[countDiary];
        }
    }

    //Assign to the right log reader button, sets count up if not the last page.
    public void RightClickButton()
    {
        if ((countDiary != diaryPages.Count - 1 && countDiary == 0) || countDiary != diaryPages.Count - 1)
        {
            countDiary++;
            diaryText.text = diaryPages[countDiary];
        }
    }

    public void ExitLogReader()
    {
        if (diaryPages.Count != 0)
        {
            audioSource.PlayOneShot(paperSound);
            countDiary = diaryPages.Count - 1;
            diaryText.text = diaryPages[countDiary];
            diaryIsActive = !diaryIsActive;
        }
    }

	public void CheckGears ()
	{
		if (num == 3)
        {
            Debug.Log("Works");
            if(prize)
                prize.SetActive(true);
        }
	}

    public void ContinueButton()
    {
        if (droneControlPanel)
        {
            if(!droneControlPanel.GetComponent<DronePanel>().isDrone)
                fpsController.isTopDown = false;
        }
        else
        {
            fpsController.isTopDown = false;
        }

        if (Camera.main.GetComponent<DroneMouseLook>())
            Camera.main.GetComponent<DroneMouseLook>().enabled = true;

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(-1);
        SimTracker.SimTracker.Instance().PushEvent(progreso);
        SceneManager.LoadScene(0);
    }

    void OnApplicationQuit()
    {
        SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(-1);
        SimTracker.SimTracker.Instance().PushEvent(progreso);
        SimTracker.SimTracker.instance.StopCleaning();
    }

    public int GetSceneNumber() { return SceneManager.GetActiveScene().buildIndex; }
}
