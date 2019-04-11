using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFlashing : MonoBehaviour 
{
	//---------- GVF -----------//
	private List<ButtonInteractionWM> buttons = new List<ButtonInteractionWM>();
	private Color currentColor;
    private Color yellowColor = new Color();

	//---------- PROPERTIES ---------//
	public Color CurrentFlashingColor { get {return currentColor;} }
	
	
	// Use this for initialization
	void Start () 
	{
		//Add buttons to list
		foreach (GameObject gob in transform.gameObject.GetChildrenList()) 
		{
			if (gob.GetComponent<ButtonInteractionWM>())
				buttons.Add(gob.GetComponent<ButtonInteractionWM>());
		}

        ColorUtility.TryParseHtmlString("#fffa00", out yellowColor); //Set the yellow color through hex value
		
		//Start flashing the buttons
		InvokeRepeating("FlashMaterial", 0f, 1.0f);
	}

	///<Summary>
	///	A method to flash the material of a button with a very specific hierarchy setup: Parent with this script --> child with ButtonInteractionWM script. Works best with InvokeRepeating!
	///</Summary>
	private void FlashMaterial() 
    {
		foreach(ButtonInteractionWM button in buttons) 
		{
			if (button.IsFlashing) 
			{
				currentColor = button.gameObject.GetComponent<Renderer>().material.color;
				if (currentColor.Equals(Color.red)) 
        		{
            		button.gameObject.GetComponent<Renderer>().material.color = yellowColor;
        		} 
				else
        		{
            		button.gameObject.GetComponent<Renderer>().material.color = Color.red;
        		}
        		currentColor = button.gameObject.GetComponent<Renderer>().material.color;
			}
		}
    }
}
