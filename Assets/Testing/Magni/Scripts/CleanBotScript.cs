using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanBotScript : MonoBehaviour {

    public bool isBrokenRail = false;
    private ControlPanelTopDown topDown;

	// Use this for initialization
	void Start () {

        topDown = GameObject.Find("ControlPanel").GetComponent<ControlPanelTopDown>();

        if (gameObject.name == "Cube (6)")
        {
            isBrokenRail = true;
        }

        if (gameObject.name == "Cube (7)")
        {
            isBrokenRail = true;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CleaningTrigger"))
        {
            topDown.isCleaning = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CleaningTrigger"))
        {
            topDown.isCleaning = false;
            Destroy(other.gameObject);
        }
    }
}
