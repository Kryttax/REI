using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CableCar : MonoBehaviour {

    private Transform player;
    private Vector3 offset;
    private AudioSource source;
    private GameObject buttonOne;
    private GameObject buttonTwo;
    private Transform car;

    public bool isMove = false;
    public float speed = 5f;
    public bool finished = false;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
        buttonOne = GameObject.Find("GreenCallButton");
        buttonTwo = GameObject.Find("RedCallButton");
        car = transform.parent;

	}
	
	// Update is called once per frame
	void Update () {

        if (isMove)
        {
            car.Translate(transform.right * speed * Time.deltaTime);
            buttonOne.GetComponent<CableCarDoor>().move = false;
            //offset = player.position - transform.position;
        }

	}

    private void LateUpdate()
    {
        if (isMove)
            player.position = new Vector3(transform.parent.GetChild(0).position.x, player.position.y, transform.parent.GetChild(0).position.z);


    }

    public void Interaction()
    {
        if (finished)
        {
            source.Play();
            isMove = true;
            buttonTwo.tag = "Untagged";
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (finished)
        {
            if (other.transform.Equals(player) && !isMove)
                buttonTwo.tag = "Interact";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (finished)
        {
            if (other.transform.Equals(player))
            {
                buttonTwo.tag = "Untagged";
                player.parent = null;
            }
        }
    }
}
