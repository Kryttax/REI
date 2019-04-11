using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour {

	private GameObject[] spawns;
	private Vector3 contactPoint;
	private GameObject player;
	private Vector3 initial;
	public int delay;
    private GameObject tempPlayer;

	// Use this for initialization
	void Start () {
		//Get the waypoints in the scene
		if (GameObject.FindGameObjectWithTag ("Spawn"))
			spawns = GameObject.FindGameObjectsWithTag ("Spawn");

		player = GameObject.FindGameObjectWithTag ("Player");
		initial = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {


		
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
            //System.Threading.Thread.Sleep(delay);
            tempPlayer = col.gameObject;
            Time.timeScale = 0.5f;
            StartCoroutine(DelayRespawn());

		}
			

	}

	private Vector3 SpawnCalculation()
	{
		//Get the highest number posible
		float nearestSqrMag = float.PositiveInfinity;

		//Make a vector as a information holder
		Vector3 nearestVector3 = Vector3.zero;

		//Similar to Vector.Distance but is gets the closest point in each for and removes the second closest and so on
		for (int i = 0; i < spawns.Length; i++)
		{
			float sqrMag = (spawns[i].transform.position - contactPoint).sqrMagnitude;
			if (sqrMag < nearestSqrMag)
			{
				nearestSqrMag = sqrMag;
				nearestVector3 = spawns[i].transform.position;
			}
		}

		//Return the closest	
		return nearestVector3;


	}

    IEnumerator DelayRespawn()
    {
        yield return new WaitForSeconds(delay);
        contactPoint = tempPlayer.transform.position;
        tempPlayer.transform.position = SpawnCalculation();
        Time.timeScale = 1f;
    }
}
