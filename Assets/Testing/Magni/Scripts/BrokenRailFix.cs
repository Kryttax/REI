using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenRailFix : MonoBehaviour {

    //Private Vars
    [Header("Assign the ReplacementRail(Number same as number of trigger):")]
    [SerializeField] private GameObject brokenRail;
    private ControlPanelTopDown topDown;
    private GameObject cleaningBot6;
    private GameObject cleaningBot7;

	// Use this for initialization
	void Start () {

        //Set the broken rail as false (Refrenced in the editor)
        brokenRail.SetActive(false);

        //Find the top down script
        topDown = GameObject.Find("ControlPanel").GetComponent<ControlPanelTopDown>();

        cleaningBot6 = GameObject.Find("Cube (6)");
        cleaningBot7 = GameObject.Find("Cube (7)");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //When the fix Cart is colliding with this trigger, activate the broken rail, set the point to current pos and set wait bool
        if (other.gameObject.CompareTag("FixCart"))
        {
            brokenRail.SetActive(true);
            topDown.ClickPoint = other.gameObject.transform.position;
            topDown.IsWaiting = true;

            if(gameObject.name == "RailPointBrokenTrigger1")
            {
                cleaningBot7.GetComponent<CleanBotScript>().isBrokenRail = false;
            }
            else if (gameObject.name == "RailPointBrokenTrigger2")
            {
                cleaningBot6.GetComponent<CleanBotScript>().isBrokenRail = false;
            }
        }

    }

}
