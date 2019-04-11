using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearGame : MonoBehaviour {

	private GameObject g1;
	private GameObject g2;
	private GameObject g3;
	private GameObject prize;
	public bool rotating = false;
	public bool stop = true;
	public float angle;
	private float i = 1.0f;
    public float indivAngle = 90f;
	public bool correct = false;
	private GameManager gm;
	public bool done = false;


	// Use this for initialization
	void Start () {

		g1 = GameObject.Find ("ggear");
		g2 = GameObject.Find ("ggear (1)");
        g3 = GameObject.Find("ggear (2)");

		gm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager>();
		g1.GetComponent<Animator> ().enabled = false;
		g2.GetComponent<Animator> ().enabled = false;
		g3.GetComponent<Animator> ().enabled = false;

    }

	// Update is called once per frame
	void Update () {

		angle = transform.rotation.eulerAngles.z;
		checkAngles ();
		if (rotating) {
			rotate ();
		}
		
	}
		
	void rotate ()
	{
		if (i % 2 == 0) {
			transform.Rotate (Vector3.back * Time.deltaTime * 100f);
		} else {
			transform.Rotate (Vector3.forward * Time.deltaTime * 100f);
		}
	}

	void checkAngles ()
	{
		if ((angle > indivAngle) && (angle < (indivAngle + 20f)) && stop) {
			Debug.Log (gameObject.name + ": check");
			gm.num++;
			if (gm.num == 3) {
				g1.GetComponent<Animator> ().enabled = true;
				g2.GetComponent<Animator> ().enabled = true;
				g3.GetComponent<Animator> ().enabled = true;
			}
			gameObject.tag = "Untagged";
			Destroy (this);

			
		}
	}
		
	public void Interaction()
	{
        if (stop)
        {
            rotating = true;
            stop = false;
            i++;
        }
        else if (!stop)
        {
            rotating = false;
            stop = true;
        }
    }

}

