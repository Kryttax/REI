using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseScript : MonoBehaviour {

    //private GameObject message;
    private GameObject player;
    private InventoryScript inv;

	// Use this for initialization
	void Start () {

        //message = GameObject.Find("Mess");
        player = GameObject.FindGameObjectWithTag("Player");
        inv = player.GetComponent<InventoryScript>();
        //message.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, 50f * Time.deltaTime, Space.World);
	}

   
    public void Interaction()
    {
        if(inv.inventory.Count == 0 && !inv.hasComeUp)
        {
            inv.Message.SetActive(true);
            inv.hasComeUp = true;
        }
    }

}
