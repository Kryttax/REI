using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MotorRotation : MonoBehaviour 
{
	#region GVF
	public float rotationSpeed;
    public int numberOfWindmill = 0;
	//public GameObject workingWindMill;

	[SerializeField]
	private bool isWorking = false;

	[SerializeField]
	private bool isRotating = false;

	[SerializeField]
	private bool isCorrectRotation = false;

	[SerializeField]
	private bool isDebugging = false;

    [SerializeField]
    private float amountOfPower = 550f;

    private WindMillManager windMillManager; 
	private WMBladeRotation _brScript;

    private GameManager gM;

	/// <summary>
	/// The vector that represents the correct rotation. Set from the value in Windmill Manager script
	/// </summary>
	private Vector3 correctRotation;
	
    #endregion

	#region Properties
	public bool IsRotating
    { 
		get { return isRotating; }
		set { isRotating = value; }
    }
	public bool IsCorrectRotation
    { 
		get { return isCorrectRotation; }
    }

    public bool IsWorking
    {
        get { return isWorking; }
        set { isWorking = value; }
    }
    #endregion

    // Use this for initialization
    void Start () 
	{
        if (GameObject.FindGameObjectWithTag("GameController"))
            gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		_brScript =  transform.GetComponentInChildren<WMBladeRotation>();

		if (GameObject.FindGameObjectWithTag("WindmillManager")) 
		{
			windMillManager = GameObject.FindGameObjectWithTag("WindmillManager").GetComponent<WindMillManager>();
			correctRotation = new Vector3(transform.rotation.x, 
										windMillManager.correctRotationValue, 
										transform.rotation.z);
		}
        else
            Debug.LogError("<color=red>Error:</color> No object in hierarchy tagged \"WindmillManager\" are present. Please tag one and attach WindMillManager script if not present!", this);

		//workingWindMill = GameObject.Find("WorkingWindMill").transform.GetChild(0).gameObject;

		//if (gameObject.Equals(workingWindMill))
			//isCorrectRotation = true;
		//gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		//Set default speed if the value is zero
		if (rotationSpeed == 0.0f) 
		{
			rotationSpeed = 20.0f;
			Debug.Log("Motor rotation speed was set to default, from 0 to 20 on: " + this.gameObject.name + ". Please check value in hierarchy if it's 0!", this);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Start rotating the motor as long as it's not in the correct position
		if (isRotating && !isCorrectRotation) 
		{
			this.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
			
			if (isDebugging) 
			{
				float distance = Vector3.Distance(transform.localEulerAngles, correctRotation);
				print("Distance from target rotation: " + distance);
			}
		}
		else if (!IsCorrectRotation)
		{
			float distance = Vector3.Distance(transform.localEulerAngles, correctRotation);
			
			if (isDebugging)
				print("Distance from target rotation when stopped: " + distance);

			if ((distance <= 10.0f && distance >= 0.0f) 
				|| (distance <= 360.0f && distance >= 350.0f)) 
			{
				print("Within the correct angle!");
				isCorrectRotation = true;
				_brScript.isRotating = true;

                //Power
                gM.powerVar = Random.Range(4f, 6f);
                gM.numOfWindmill = numberOfWindmill;

                if (SceneManager.GetActiveScene().name.Equals("01IntroLevel"))
                {
                    if (windMillManager.FillValue < 1.0f)
                        windMillManager.FillValue += 1.0f;
                }
                else
                {
                    if (windMillManager.FillValue < 1.0f)
                        windMillManager.FillValue += .25f;
                }
			}
		}
	}
}
