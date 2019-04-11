using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public class ValveHandler : MonoBehaviour 
{
	#region Global variable field

	[Range(0f, 50f)]
	public float rotationSpeed = 20.0f;
	private bool showMessage = false;
	private GameObject player, blade;
	private GameObject missingItemMessage;
	private float elapsedTime = 0.0f;
	[SerializeField] private bool canRotate = false;
	[SerializeField] private bool isRotating = false;
	[SerializeField] private bool isDebug = false;

	private Vector3 correctRotation;
	#endregion

	#region Properties
    public bool IsRotating
    {
        get { return isRotating; }
        set { isRotating = value; }
    }

	public bool CanRotate
    {
        get { return canRotate; }
        set { canRotate = value; }
    }
	#endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
	{
		if (GameObject.Find("Message"))
       		missingItemMessage = GameObject.Find("Message");
    	else 
        	Debug.LogError("<color=red>Error:</color> No object named \"Message\" could be found on Canvas!", this);
	}

	// Use this for initialization
	void Start () 
	{
		//Gotta find that player beyond yonder...
		if (GameObject.FindGameObjectWithTag("Player")) 
		{
			player = GameObject.FindGameObjectWithTag("Player");
		}
		else
			Debug.LogError("<color=red>Error:</color> No gameobject tagged Player could be found. Ensure player object is in the scene and tagged correctly!", this);

		//Blade me baby!
		if (GameObject.FindGameObjectWithTag("Blade")) 
		{
			blade = GameObject.FindGameObjectWithTag("Blade");
			correctRotation = blade.transform.localEulerAngles; //Given that the blade is at rotation 0,0,0 in inspector at start, offset is set below
			
			if (isDebug)
				print("Blade's euler angle at start: " + blade.transform.eulerAngles);

			blade.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -60f);	//Set the rotation to 60 in z-axis, so it has an offset to correct position
		}
		else
			Debug.LogError("<color=red>Error:</color> No gameobject tagged Blade could be found. Ensure the blade to be rotated is tagged correctly!", this);

		//Deactivate the "missing item" message on canvas if active
		if (missingItemMessage.activeInHierarchy)
			missingItemMessage.SetActive(false);

		//If valve is tagged something else than Interact, tag it Interact...
		if (!transform.tag.Equals("Interact"))
			transform.tag = "Interact";
		
		//Simulate the item is missing
		GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (showMessage)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 5f)
            {
                missingItemMessage.SetActive(false);
                showMessage = false;
            }
        }

		//if player is interacting with the valve
		if (isRotating && canRotate) 
		{
			//Rotate the valve
			this.transform.parent.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

			//Rotate the blade
			blade.transform.Rotate(-Vector3.back, (rotationSpeed / 4.0f) * Time.deltaTime);
			
			if (isDebug)
				print("Distance from correct rotation: " + Vector3.Distance(blade.transform.eulerAngles, correctRotation));
			float distance = Vector3.Distance(blade.transform.eulerAngles, correctRotation);
			if (distance >= 359.0f || distance <= 1.0f) 
			{
				canRotate = false;
				blade.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
				transform.tag = "Untagged";

				if (GetComponentInParent<MotorRotation>())
					GetComponentInParent<MotorRotation>().IsWorking = true;
			}
		}
	}

	public void Interaction() 
	{
		//Player has the required item
		if (player.GetComponent<InventoryScript>().inventory.Contains("ValveWheel") && !canRotate) 
		{
			player.GetComponent<InventoryScript>().inventory.Remove("ValveWheel");
			canRotate = true;
			GetComponent<MeshRenderer>().enabled = true;
		}
		//Player does not have the required item
		else if (!player.GetComponent<InventoryScript>().inventory.Contains("ValveWheel") && !canRotate) 
		{
			//If message box is not active
			if (!missingItemMessage.activeInHierarchy) 
			{
				elapsedTime = 0;
                showMessage = true;
                missingItemMessage.SetActive(true);
                missingItemMessage.GetComponent<Text>().text = "You're missing the required item!";
			}
		}
		else //Can rotate wheel and blade
		{
			isRotating = true;
		}
	}
}
