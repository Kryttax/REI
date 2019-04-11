using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButtons : MonoBehaviour {

    private GameObject elevator;

	// Use this for initialization
	void Start () {

        elevator = transform.parent.parent.GetChild(4).gameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interaction()
    {
        if (gameObject.name == "ButtonUp")
        {
            elevator.GetComponent<ElevatorScript>().up = true;
            elevator.GetComponent<ElevatorScript>().down = false;

        }
        else if(gameObject.name == "ButtonDown")
        {
            elevator.GetComponent<ElevatorScript>().down = true;
            elevator.GetComponent<ElevatorScript>().up = false;
        }

    }
}
