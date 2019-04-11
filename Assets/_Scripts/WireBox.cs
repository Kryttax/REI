using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBox : MonoBehaviour
{
    public float currentDamage = 100f;

    public bool isFixed;

    public Material fixedWireMat2;

    private DronePanel dronePanel;
    private GameObject fixedWireBoxPartSys;
//    private DroneWireBoxController droneWireBoxController;


    public void Repair (float repairAmount)
    {
        currentDamage -= repairAmount * Time.deltaTime;

        if (currentDamage <= 0)
        {
            dronePanel.fixedBoxes++;
            isFixed = true;
            Instantiate(fixedWireBoxPartSys, transform.position, transform.rotation);
            transform.GetChild(0).GetComponent<Renderer>().material = fixedWireMat2;
            gameObject.tag = "Untagged";
        }
    }


    void Start()
    {
        isFixed = false;
        dronePanel = GameObject.Find("DroneControlButton").GetComponent<DronePanel>();
        fixedWireBoxPartSys = Resources.Load<GameObject>("Prefabs/Particle Systems/FixedWireBoxPartSys") as GameObject;
        /*
        if (GameObject.FindGameObjectWithTag("DroneWireBoxManager"))
        {
            droneWireBoxController = GameObject.FindGameObjectWithTag("DroneWireBoxManager").GetComponent<DroneWireBoxController>();
        }
        else
        {
            Debug.Log("No DroneWireBoxManager found");
        }
        */
    }

}
