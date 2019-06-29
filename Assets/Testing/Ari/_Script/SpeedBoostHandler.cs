using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostHandler : MonoBehaviour 
{
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController characterController;
	private GameObject boostUIHolder;
	private Image boostMask;
	private float initialRunSpeed;
	private bool speedBoostActive = false;
	private GameManager gameManager;

	// Use this for initialization
	void Start () 
	{
		//----- GET REFERENCES -----//
		if (GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()) 
		{
			characterController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
			initialRunSpeed = characterController.RunSpeed;
		}
		else Debug.LogError("<color=red>Warning:</color> " + this.gameObject.name + "has no CharacterController Script attached to it." 
			+ "In order to make speed boost work, be sure the object has this script and a character controller script attached to it!" , this);

		if (GameObject.FindGameObjectWithTag("GameController"))
			gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		if (GameObject.FindGameObjectWithTag("SpeedBoost")) 
		{
			//Get the UI holder
			boostUIHolder = GameObject.FindGameObjectWithTag("SpeedBoost");

			//If child exists and has Image component: get reference
			if (boostUIHolder.transform.GetChild(1).GetChild(0))
				if (boostUIHolder.transform.GetChild(1).GetChild(0).GetComponent<Image>())
					boostMask = boostUIHolder.transform.GetChild(1).GetChild(0).GetComponent<Image>();
			
			boostMask.fillAmount = 0f;
		}

		//Deactivate speed boost UI on start
		if (boostUIHolder.activeInHierarchy)
			boostUIHolder.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If fill amount has reached zero and speed boost is active, set it to initial speed and hide UI element
		if (boostMask.fillAmount <= 0f && speedBoostActive) 
		{
			characterController.RunSpeed = initialRunSpeed;
			speedBoostActive = false;

			if (boostUIHolder.activeInHierarchy)
				boostUIHolder.SetActive(false);
		}

		//Decrease fill amount when player is running
		if (!characterController.IsWalking)
			boostMask.fillAmount -= .001f;
            

        //If diary is activated, hide speed boost UI
        if (gameManager.diaryIsActive && speedBoostActive)
			boostUIHolder.SetActive(false);
		else if (!gameManager.diaryIsActive && speedBoostActive)
			boostUIHolder.SetActive(true);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("SpeedBoostPickup")) 
		{	
			//If speed boost is already full, return
			if (boostMask.fillAmount.Equals(1.0f))
				return;

			//Enable slider if not already active
			if (!boostUIHolder.activeInHierarchy) 
				boostUIHolder.SetActive(true);

			//Set fill value to max if it is less than max
			if (boostMask.fillAmount < 1.0f)
				boostMask.fillAmount = 1.0f;

			//Increase the run speed by two
			characterController.RunSpeed = initialRunSpeed * 2.0f;

            SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(1, "BOOST", 0, 0, 0);

            Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
            SimTracker.InteractionEvent evnt = new SimTracker.InteractionEvent(GameManager.instance.GetSceneNumber(), pos.x, pos.y, pos.z, "ITEM TAKEN", "object: Speed Boost");
            SimTracker.SimTracker.instance.PushEvent(progreso);

            //Destroy the speed boost pickup
            Destroy(other.gameObject);
			speedBoostActive = true;

            
        }
	}
}
