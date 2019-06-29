using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWireBoxController : MonoBehaviour
{
    [SerializeField] private GameObject[] wireBoxes;
    [SerializeField] private bool allFixed = false;
    //private int wireBoxesNeededToFix = 0;

    public bool AllFixed
    {
        get { return allFixed; }
        set { allFixed = value; }
    }


    void Start ()
    {
        if (GameObject.FindGameObjectWithTag("BrokenWireBox") && GameObject.FindGameObjectsWithTag("BrokenWireBox").Length > 0)
        {
            wireBoxes = GameObject.FindGameObjectsWithTag("BrokenWireBox");
        }
        else
        {
            Debug.Log("No broken wire boxes found");
        }
        //wireBoxesNeededToFix = wireBoxes.Length;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!allFixed)
        {
            foreach (GameObject wireBox in wireBoxes)
            {
                WireBox _wireBoxScript = wireBox.GetComponentInChildren<WireBox>();
                if (_wireBoxScript.isFixed)
                    continue;
                else
                    return;
            }
            SimTracker.ProgressEvent progreso = new SimTracker.ProgressEvent(1,"DRON COMPLETE",0,0,0);
            SimTracker.SimTracker.instance.PushEvent(progreso);
            allFixed = true;
        }
	}
}
