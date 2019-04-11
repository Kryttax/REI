using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenMotorHandler : MonoBehaviour 
{
	//GVF
	private bool showMessage;
	private GameObject player;
	private GameObject missingItemMessage;
	private GameObject gearBox;
	
	private float elapsedTime = 0.0f;

/// <summary>
/// Awake is called when the script instance is being loaded.
/// </summary>
	void Awake()
	{
		if (GameObject.Find("Message"))
       		missingItemMessage = GameObject.Find("Message");
    	else 
        	Debug.LogError("<color=red>Error:</color> No object named \"Message\" could be found on Canvas!", this);
	}

    // Use this for initialization
    void Start () 
	{
		if (GameObject.FindGameObjectWithTag("Player")) 
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
		else
			Debug.LogError("No gameobject tagged Player could be found. Ensure player object is in the scene and tagged correctly!", this);

		if (GameObject.FindGameObjectWithTag("GearBox")) 
		{
			gearBox = GameObject.FindGameObjectWithTag("GearBox");

			if (gearBox.activeInHierarchy)
				gearBox.SetActive(!gearBox.activeInHierarchy);
		}
		else
			Debug.LogError("<color=red>Missing:</color> Broken Motor needs gear box placed as child and tagged \"GearBox\"!", this);

		//Deactivate the "missing item" message on canvas if active
		if (missingItemMessage.activeInHierarchy)
			missingItemMessage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Track how long the message box has been shown
        if (showMessage)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 5f)
            {
                missingItemMessage.SetActive(false);
                showMessage = false;
            }
        }
	}

	public void Interaction() 
	{
		if (player.GetComponent<InventoryScript>().inventory.Contains("GearBox")) 
		{
			player.GetComponent<InventoryScript>().inventory.Remove("GearBox");		//Remove GearBox from players inventory
			gameObject.tag = "Untagged";											//Untag the motor, making it non-interactable

			//Activate the gear box item
			gearBox.SetActive(true);

			//Turn off the smoke particle emitting from motor
			GetComponentInChildren<ParticleSystem>().Stop();

			//Set motor script isworking variable to true
			GetComponentInParent<MotorRotation>().IsWorking = true;
		}
		else	//Show the missing item message
        {
            if (!missingItemMessage.activeInHierarchy) 		//If not active in hierarchy, activate it and start cooldown counting
            {
                elapsedTime = 0;
                showMessage = true;
                missingItemMessage.SetActive(true);
                missingItemMessage.GetComponent<Text>().text = "You're missing the required item!";
            }
        }
	}
}
