using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour {

    public enum ColorEnum
    {
        blue,
        green,
        yellow,
        red,
    }

    public ColorEnum cE;

	// Use this for initialization
	void Start () {
        Interaction();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interaction()
    {
        if (cE == ColorEnum.red)
        {
            cE = ColorEnum.blue;
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (cE == ColorEnum.blue)
        {
            cE = ColorEnum.yellow;
            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (cE == ColorEnum.yellow)
        {
            cE = ColorEnum.green;
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (cE == ColorEnum.green)
        {
            cE = ColorEnum.red;
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

}
