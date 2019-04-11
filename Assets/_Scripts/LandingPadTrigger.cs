using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingPadTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drone"))
        {
            GameObject.FindGameObjectWithTag("PanelDrone").SetActive(false);
            GameObject.Find("DroneControlButton").GetComponent<DronePanel>().isDrone = false;
            GameObject.Find("DroneControlButton").GetComponent<DronePanel>().moveBack = true;
        }
    }
}
