using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMMotorRotation : MonoBehaviour {

	#region GVF
	public float rotationSpeed;
	//public float startAngle;
	public Enum_WM.Position position;
	[SerializeField]
	private bool isRotating = false;
	[SerializeField]
	private bool debugAngles = false;
	[SerializeField]
	private bool isDebugging = false;
    private GameManager gM;
    #endregion

    #region Properties
	public bool IsRotating
    { 
		get { return isRotating; }
		set { isRotating = value; }
    }
    #endregion
    // Use this for initialization
    void Start () 
	{
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        //Mechanic testing
        //Rotate(90.0f);

        if (gM.CorrectPos == position && !isRotating)
        {
            transform.GetChild(0).GetComponent<WMBladeRotation>().isRotating = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<WMBladeRotation>().isRotating = false;
        }
		
		if (isRotating) 
		{
			switch (position) 
			{
				case Enum_WM.Position.ninety: 
				{
					if (isDebugging)
						print("Rotating to 90 degrees");

					Rotate(90.0f);
					break;
				}
				case Enum_WM.Position.oneEighty: 
				{
					if (isDebugging)
						print("Rotating to 180 degrees");

					Rotate(180.0f);
					break;
				}
				case Enum_WM.Position.twoSeventy: 
				{
					if (isDebugging)
						print("Rotating to 270 degrees");

					Rotate(270f);
					break;
				}
				case Enum_WM.Position.zero: 
				{
					if (isDebugging)
						print("Rotating to 0 degrees");

					Rotate(360.0f);
					break;
				}
			}
		}
	}
	
	private void Rotate(float angle) 
	{
		Vector3 targetRot = new Vector3(0.0f, angle, 0.0f);
        Debug.Log("TargetRot: " + targetRot);
        //float distance = Vector3.Angle(transform.forward, targetRot);
		float distance = Vector3.Distance(transform.localEulerAngles, targetRot);

		//Debugging
		if (debugAngles) 
		{
			print("Distance between motor angles and target angle: " + distance 
				+ "\nMotor angle: " + transform.localEulerAngles.ToString());
		}

		if (distance <= 10.0f && distance >= .5f) 
		{
			if (debugAngles)
				print("Slow rotation speed.\nMotor angle: " + transform.localEulerAngles.ToString());

            this.transform.Rotate(targetRot, (rotationSpeed * Time.deltaTime) / 2.0f);
		}
        else if (distance >= .5f)
        {
            if (debugAngles)
                print("Normal Rotation speed.\nMotor angle: " + transform.localEulerAngles.ToString());

            //If the motor is close to its destination rotation, half the rotation speed is applied
            this.transform.Rotate(targetRot, rotationSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetRot), rotationSpeed * Time.deltaTime);
        }
        else
        {
			if (isDebugging)
				print("Angle is now set to: " + transform.localEulerAngles.ToString());

			transform.localEulerAngles = targetRot;
			isRotating = false;
		}
 
	}
}
