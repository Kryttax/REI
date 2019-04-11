using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInventory : MonoBehaviour {

    private GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interaction()
    {
        player.GetComponent<InventoryScript>().inventory.Add(gameObject.name);
        Destroy(gameObject);
    }
}
