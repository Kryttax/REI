using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour 
{

	private InventoryScript playerInventory;
    private GameManager gM;

	// Use this for initialization
	void Start () {

        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (GameObject.FindGameObjectWithTag("Player"))
			playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();
		else
			Debug.LogError("<color=red>Error:</color> missing a player object tagged \"Player\" in the scene! ", this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Rotate the item
		transform.Rotate(Vector3.up, 50f * Time.deltaTime, Space.World);	
	}

    
	public void Interaction()
    {	
		//Show the open inventory hint the first time a player picks up an item
		if(playerInventory.inventory.Count == 0 && !playerInventory.hasComeUp)
        {
            playerInventory.Message.SetActive(true);
            playerInventory.Message.GetComponent<Text>().text = "Press and hold tab to open inventory";
            playerInventory.hasComeUp = true;
        }

		//Add the item to the players inventory
        playerInventory.inventory.Add(gameObject.name);
        Destroy(gameObject);	//Destroy the object
        SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(10, "ITEM TAKEN", 0, 0, 0);
        SimTracker.SimTracker.Instance().PushEvent(progreso);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Show the open inventory hint the first time a player picks up an item
            if (playerInventory.inventory.Count == 0 && !playerInventory.hasComeUp)
            {
                playerInventory.Message.SetActive(true);
                playerInventory.Message.GetComponent<Text>().text = "Press and hold tab to open inventory";
                gM.ShowMessage = true;
                playerInventory.hasComeUp = true;
            }

            //Add the item to the players inventory
            playerInventory.inventory.Add(gameObject.name);
            Destroy(gameObject);    //Destroy the object
            SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(10, "ITEM TAKEN", 0, 0, 0);
            SimTracker.SimTracker.Instance().PushEvent(progreso);
        }
    }
}
