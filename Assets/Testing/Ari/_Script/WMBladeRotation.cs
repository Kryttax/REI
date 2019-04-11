using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WMBladeRotation : MonoBehaviour 
{
	#region GVF
	[Header("Good start speed is zero")]
	public float speed;
	[Header("Good max speed would be 40 floats")]
	public float maxSpeed;
	[Space(20)]
	[SerializeField]
	public bool isRotating = false;
	#endregion
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isRotating) 
		{
			if (speed < maxSpeed) 
			{
				speed += (Time.deltaTime * 2.0f);
			}
			else speed = maxSpeed;

			this.transform.Rotate(Vector3.left, speed * Time.deltaTime);
		}
		else 
		{
			if (speed > .0f) 
			{
				speed -= (Time.deltaTime * 3.0f);
				this.transform.Rotate(Vector3.left, speed * Time.deltaTime);
			}
			else if (speed <= 15.0f && speed > .0f) 
			{
				speed -= Time.deltaTime;
				this.transform.Rotate(Vector3.left, speed * Time.deltaTime);
			}
			else if (speed <= 5.0f && speed > .0f) 
			{
				speed -= (Time.deltaTime / 5.0f);
				this.transform.Rotate(Vector3.left, speed * Time.deltaTime);
			}
			else speed = .0f;


			
		}
	}
}
