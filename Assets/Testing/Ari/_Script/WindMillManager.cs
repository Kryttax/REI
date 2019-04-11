using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindMillManager : MonoBehaviour 
{
	#region Global Variables
	[Tooltip("The value that is set as correct rotation. Param: Y-Component. The resulting Vector will be Vector3(x, value, z)" )]
	[Range(0f, 360.0f)]
	public float correctRotationValue = 0f;

	[SerializeField]
	private GameObject[] windMills;		//An array holding all windmills
	private GameObject powerBarUI;		//Reference to the power bar UI on canvas
	[SerializeField] private Image powerBarImage;
	[SerializeField] private bool allCorrect = false;	//Set to true if all windmills are in the correct rotation
	private int count = 0;				//Used to count every windmill that is correctly rotated
	private float fillValue = 0f;

	#endregion

	#region Properties
    public bool AllCorrect { get { return allCorrect; } }
    public GameObject[] WindMills { get { return windMills; } }

    public float FillValue
    {
        get { return fillValue; }
        set { fillValue = value; }
    }
    #endregion

    // Use this for initialization
    void Start () 
	{
		if (GameObject.FindGameObjectWithTag("Windmill") && GameObject.FindGameObjectsWithTag("Windmill").Length > 0)
			windMills = GameObject.FindGameObjectsWithTag("Windmill");
		else
			Debug.LogError("<color=red>Error:</color> No object in hierarchy tagged \"Windmill\" are present. Please tag every windmill parent object.", this);

		if (GameObject.FindGameObjectWithTag("PowerBar")) 
		{
			powerBarUI = GameObject.FindGameObjectWithTag("PowerBar");
			powerBarImage = powerBarUI.transform.GetChild(0).GetChild(0).GetComponent<Image>();
			//fillPowerBar.fillAmount = .3f; //Test
		}
        else
            Debug.LogError("<color=red>Missing:</color> No object on canvas tagged \"PowerBar\"! Ensure it's there and correctly tagged.", this);

	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check if windmills are correctly rotated
		if (windMills.Length > 0)
			WindMillCheck();
		
		//Update power bar UI mask fill if fillvalue is changed when a windmill is rotated correctly
		if (powerBarImage.fillAmount < fillValue)
 			powerBarImage.fillAmount += Time.deltaTime / 10.0f;
		else
			powerBarImage.fillAmount = fillValue;
	}

    private void WindMillCheck()
    {
        if (!allCorrect)
        {

			foreach(GameObject windMill in windMills) 
			{
				if (windMill.GetComponentInChildren<MotorRotation>()) 
				{
					MotorRotation _motorScript = windMill.GetComponentInChildren<MotorRotation>();
					if (_motorScript.IsCorrectRotation)
						continue;
					else
						return;
				}
				else
					Debug.Log("Gameobject: " + windMill.name + "does not have a child with component: Motorscript!");
			}
			allCorrect = true;
			/*
            count = 0;	//Set count to zero
            foreach (GameObject windMill in windMills)
            {
                if (windMill.transform.GetChild(0).GetComponent<MotorRotation>())
                {
                    MotorRotation _motorScript = windMill.transform.GetChild(0).GetComponent<MotorRotation>();
                    if (_motorScript.IsCorrectRotation)
                        count++;
                }
            }

            if (count == windMills.Length)
            {
                allCorrect = true;
            } */
        }
    }
}
