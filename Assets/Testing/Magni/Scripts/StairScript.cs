using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class StairScript : MonoBehaviour {

    //Public Variables
    public bool isMounted = false;
    public bool mounting = false;

    //Private varialbes
    private Transform player;
    private Vector3 charPos;
    private float bottom;
    private float height;
    private float num;
    private float heightPos;
    private float ladderTime = 0f;
    [SerializeField] private AudioClip[] ladderSounds;
    private AudioSource source;

	// Use this for initialization
	void Start () {

        //Calculation of the stair, height and bottom.
        height = gameObject.GetComponent<Renderer>().bounds.size.y;
        bottom = transform.position.y - (height / 2);
        heightPos = transform.position.y + (height / 2);

        ladderSounds = Resources.LoadAll<AudioClip>("Audio/SFX/LadderSounds");

	}
	
	// Update is called once per frame
	void Update () {

        //If is mounted to the ladder.
        if (isMounted)
        {

            IfMounted();

        }

        //If in the process of mounting the ladder.
        if(mounting)
        {

            //Set stair tag to untagged
            transform.tag = "Untagged";

            //Slerp to the right position.
            player.position = Vector3.Slerp(player.position, new Vector3(transform.position.x + transform.forward.x, player.position.y, transform.position.z + transform.forward.z), 5f * Time.deltaTime);

            //If in the right position, mount the stair and enable the box collider.
            if (Vector3.Distance(new Vector3(transform.position.x + transform.forward.x, player.position.y, transform.position.z + transform.forward.z), player.position) <= 0.1f)
            {
                isMounted = true;
                mounting = false;
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }

	}

    public void Interaction(Transform sender)
    {

        //Get the player object.
        player = sender;

        //Start mounting the ladder.
        mounting = true;

        //Disable the box collider.
        gameObject.GetComponent<BoxCollider>().enabled = false;

    }

    private void IfMounted()
    {

        //Move up if pressing W and down if pressing S.
        Vector3 temp = player.position;
        temp.y += (Input.GetAxis("Vertical") * 5f * Time.deltaTime);

        if(Input.GetAxisRaw("Vertical") != 0)
        {
            ladderTime += Time.deltaTime;
            if (ladderTime >= 0.5f)
            {
                player.GetComponent<AudioSource>().PlayOneShot(ladderSounds[Random.Range(0, 4)]);
                ladderTime = 0f;
            }
        }

        player.position = temp;

        //Dismount if at the top.
        if (player.position.y >= heightPos + 1.5f)
        {
            transform.tag = "Interact";
            isMounted = false;
        }

        //Dismount if at the bottom.
        if (player.position.y <= bottom)
        {
            transform.tag = "Interact";
            isMounted = false;
        }

        //Dismount if pressing space.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.tag = "Interact";
            isMounted = false;
        }
    }

}
