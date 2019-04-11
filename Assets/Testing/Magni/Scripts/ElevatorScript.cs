using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {

    public float elevatorSpeed = 5f;
    public bool up = false;
    public bool down = false;

    private GameObject downPoint;
    private GameObject upPoint;
    private AudioSource source;
    private AudioClip eleSound;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player");
    }

    // Use this for initialization
    void Start () {
        gameObject.AddComponent<BoxCollider>().isTrigger = true;
        eleSound = (AudioClip)Resources.Load("Audio/SFX/elevator sound", typeof(AudioClip));
        downPoint = transform.parent.GetChild(2).gameObject;
        upPoint = transform.parent.GetChild(3).gameObject;
        source = gameObject.AddComponent<AudioSource>();
        source.clip = eleSound;
        source.loop = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (up)
        {
            transform.position = Vector3.MoveTowards(transform.position, upPoint.transform.position, elevatorSpeed * Time.deltaTime);
        }
        else if (down)
        {
            transform.position = Vector3.MoveTowards(transform.position, downPoint.transform.position, elevatorSpeed * Time.deltaTime);
        }

        /*
        if (!up && !down && player.transform.parent != null)
            player.transform.parent = null;
            */

        
        if (Vector3.Distance(upPoint.transform.position, transform.position) <= 0.0f || Vector3.Distance(downPoint.transform.position, transform.position) <= 0.0f)
        {
            source.Stop();
            up = false;
            down = false;
        }
        

	}

    public void Interaction()
    {
        if(Vector3.Distance(downPoint.transform.position, transform.position) <= 0.0f)
        {
            source.Play();
            up = true;
        }
        else if (Vector3.Distance(upPoint.transform.position, transform.position) <= 0.0f)
        {
            source.Play();
            down = true;
        }
    }

    
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            player.transform.parent = null;
        }
    }
    */
}
