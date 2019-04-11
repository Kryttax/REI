using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle what happens when a button on the windmill control panel is pressed
/// </summary>
public class ButtonInteractionWM : MonoBehaviour 
{
    #region Global variable field
	public GameObject windMill;
	public MotorRotation motorScript;
	private AudioSource audioSource;
    private AudioClip buttonPressSound;
    private AudioClip buzzSound;

    private float elapedTime = 0f;
    private bool showMessage = false;
    private bool isFlashing = false;
    private Color currentColor;
    private Color yellowColor = new Color();
    [SerializeField] private GameObject message;
    private DroneWireBoxController droneController;
    #endregion

    #region Properties
    public bool IsFlashing 
    { 
        get { return isFlashing; }
        set { isFlashing = value; }
    }

    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //Since more than one button needs a reference to this UI Text, we need to get it in awake, then disable it in Start()
        if (GameObject.Find("Message"))
            message = GameObject.Find("Message");
        else
            Debug.LogError("<color=red>Error:</color> No gameobject named \"Message\" are present in the canvas!");
    }

	// Use this for initialization
	void Start () 
	{
		if (windMill) 
        {
            motorScript = windMill.transform.GetComponentInChildren<MotorRotation>();
        }
        else Debug.LogError("<color=red>Error:</color> No windmill tied to gameobject: " + this.gameObject.name);

        if(GameObject.FindGameObjectWithTag("DroneWireBoxManager"))
            droneController = GameObject.FindGameObjectWithTag("DroneWireBoxManager").GetComponent<DroneWireBoxController>();

        //Disable the button UI message text
        if (message)
            message.SetActive(false);

        buttonPressSound = (AudioClip)Resources.Load("Audio/SFX/ButtonPush01");
        buzzSound = (AudioClip)Resources.Load("Audio/SFX/Error01");
        audioSource = gameObject.AddComponent<AudioSource>();

        //Get the current color of the button
        currentColor = GetComponent<Renderer>().material.color;
        ColorUtility.TryParseHtmlString("#fffa00", out yellowColor);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
        //If the windmill is in the correct rotation, change color and untag the object
        if (windMill && motorScript.IsCorrectRotation)
        {
            if (isFlashing) 
            {
                //StopCoroutine(Flash());
                //CancelInvoke();

                isFlashing = false;     //Flashing mechanic moved to a seperate script, simply setting isFlashing to false should make it stop flashing
            }

            GetComponent<Renderer>().material.color = Color.green;  //Set it to green color
			transform.tag = "Untagged";     //Make it non-interactable by changing tag
        }

        if (motorScript.IsWorking && !motorScript.IsCorrectRotation && !isFlashing) 
        {
            //Start blinking material color on button if in windmill is working but not in the correct rotation
            //StartCoroutine(Flash());
            //InvokeRepeating("FlashMaterial", 0f, 1.0f);

            if (windMill.name.Equals("WindTurbine_Valve") && !droneController.AllFixed)
                return;

            isFlashing = true;  //Prompt button to flash
            //Set the color of this button to the same as the current flashing color to sync colors
            if (transform.parent.GetComponent<ButtonFlashing>())
                GetComponent<Renderer>().material.color = transform.parent.GetComponent<ButtonFlashing>().CurrentFlashingColor;
            else 
                Debug.Log(this.gameObject.name + " has no parent with \"ButtonFlashing\" script attached to it!");
        }

        //The player interacted with the button but the windmill is not fixed yet!
        if (showMessage)
        {
            elapedTime += Time.deltaTime;
            if (elapedTime >= 5f)
            {
                message.SetActive(false);
                showMessage = false;
            }
        }
	}

	public void Interaction()
    {
        //If the windmill is working, prompt the motor script to rotate
        if(motorScript.IsWorking)
        {

            if (windMill.name.Equals("WindTurbine_Valve") && !droneController.AllFixed)
            {

                audioSource.PlayOneShot(buzzSound);

                return;
            }

            audioSource.PlayOneShot(buttonPressSound);
            motorScript.IsRotating = true;
        }
        //Else tell player to fix windmill first
        else if (!message.activeInHierarchy)    //Prevent player form spamming button and sound
        {
            message.SetActive(true);
            message.GetComponent<Text>().text = "You need to fix the windmill first!";
            audioSource.PlayOneShot(buzzSound);
            showMessage = true;
            elapedTime = 0f;
        }
    }

    private void FlashMaterial() 
    {
        if (currentColor.Equals(Color.red)) 
        {
            GetComponent<Renderer>().material.color = yellowColor;
        } else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        currentColor = GetComponent<Renderer>().material.color;
    }


    //Recursive IEnumerator to flash the color on the button
    //NOTE TO SELF: Recursiveness in IEnumerators DOES NOT GO WELL, don't know why I thought so... (Ari)
    /*private IEnumerator Flash() 
    {
        if (currentColor.Equals(Color.red)) 
        {
            GetComponent<Renderer>().material.color = yellowColor;
            //GetComponentInChildren<Light>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
            //GetComponentInChildren<Light>().enabled = false;
        }

        currentColor = GetComponent<Renderer>().material.color;
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Flash());
    }*/
}
