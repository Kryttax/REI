using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour {

    //Variables
    private Transform cam;
    //private InventoryScript invenScr;
    private RaycastHit mainHit;
    [SerializeField] private Canvas mainCanvas;
    private GameObject text;
    private GameObject interactedObject;
    private Transform tempHit;
    

    public StairScript StairScript { get; set; }

    
    //Properties
    public bool Holding { get; set; }

	// Use this for initialization
	void Start () {
        cam = transform.GetChild(0);
        //invenScr = GetComponent<InventoryScript>();
        text = mainCanvas.transform.GetChild(1).gameObject;
        text.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        //If the log reader is active, disable movement and enable 
        //movement if mouse button is pressed as well as deactivating the log reader.
        if (mainCanvas.transform.GetChild(6).gameObject.activeSelf)
        {
            gameObject.GetComponent<FirstPersonController>().enabled = false;
            if (Input.GetMouseButtonDown(1))
            {
                mainCanvas.transform.GetChild(6).gameObject.SetActive(false);
                gameObject.GetComponent<FirstPersonController>().enabled = true;
            }
        }

        //Distance check for windmill button holding.
        if (tempHit && interactedObject)
        {
            if (Vector3.Distance(transform.position, tempHit.position) >= 5f)
            {
                //interactedObject.transform.GetChild(0).GetComponent<MotorRotation>().IsRotating = false;
                if (interactedObject.GetComponentInChildren<MotorRotation>()) 
                {
                    if (interactedObject.GetComponentInChildren<MotorRotation>().IsRotating)
                         interactedObject.GetComponentInChildren<MotorRotation>().IsRotating = false;

                }
                else if (interactedObject.GetComponent<ValveHandler>()) 
                {
                    if (interactedObject.GetComponent<ValveHandler>().IsRotating)
                        interactedObject.GetComponent<ValveHandler>().IsRotating = false;
                }
            }
        }

        //Interaction if statement when left mouse button is pressed or let go.
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            if (Physics.Raycast(cam.position, cam.forward, out hit, 3f))
            {

                if (hit.transform.CompareTag("Interact"))
                {

                    if (hit.transform.name == "Stairs")
                    {
                        hit.transform.gameObject.SendMessage("Interaction", transform, SendMessageOptions.DontRequireReceiver);
                        StairScript = hit.transform.GetComponent<StairScript>();
                    }
                    else
                    {
                        //Added Dont Require Reciever to prevent errors if interactable has no Interaction method - Ari
                        hit.transform.gameObject.SendMessage("Interaction", SendMessageOptions.DontRequireReceiver);
                    }

                    if (hit.transform.GetComponent<ButtonInteractionWM>())
                    {
                        interactedObject = hit.transform.GetComponent<ButtonInteractionWM>().windMill;
                        tempHit = hit.transform;
                    }
                    else if (hit.transform.GetComponent<ValveHandler>()) 
                    {
                        if (hit.transform.GetComponent<ValveHandler>().CanRotate) 
                        {
                            interactedObject = hit.transform.gameObject;
                            tempHit = hit.transform;
                        }
                    }

                }

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Holding = false;

            //when player releases button on "hold to interact" objects, stop current interaction
            if(interactedObject) 
            {
                if (interactedObject.GetComponentInChildren<MotorRotation>())
                    interactedObject.GetComponentInChildren<MotorRotation>().IsRotating = false;
                else if (interactedObject.GetComponent<ValveHandler>())
                    interactedObject.GetComponent<ValveHandler>().IsRotating = false;
                
                interactedObject = null;
                tempHit = null;
            }
        }


        //Interaction message when raycast hits interact tag colliders.
        if (Physics.Raycast(cam.position, cam.forward, out mainHit, 3f))
        {
            if (mainHit.transform.CompareTag("Interact"))
            {
                if (mainHit.transform.GetComponent<ValveHandler>() || mainHit.transform.GetComponent<ButtonInteractionWM>())
                    text.GetComponent<Text>().text = "Hold [LMB]";
                else if (mainHit.collider.gameObject.GetComponent<MessageScript>())
                    text.GetComponent<Text>().text = mainHit.collider.gameObject.GetComponent<MessageScript>().interactionMessage;
                else text.GetComponent<Text>().text = "Interact [LMB]";

                text.SetActive(true);
            }
            else
            {
                text.SetActive(false);
            }
        }
        else
        {
            text.SetActive(false);
        }
    }
}
