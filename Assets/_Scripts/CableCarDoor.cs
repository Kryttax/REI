using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableCarDoor : MonoBehaviour {

    private GameObject door;
    private Transform point;
    private Transform origPoint;
    public bool move;

	// Use this for initialization
	void Start () {

        door = GameObject.Find("Cart_Door1");
        point = GameObject.Find("DoorPoint").transform;
        origPoint = GameObject.Find("DoorPointDown").transform;

    }
	
	// Update is called once per frame
	void Update () {

        if (move)
            door.transform.position = Vector3.Slerp(door.transform.position, point.position, 3f * Time.deltaTime);
        else
            door.transform.position = Vector3.Slerp(door.transform.position, origPoint.position, 3f * Time.deltaTime);

	}

    public void Interaction()
    {
        move = !move;
    }
}
