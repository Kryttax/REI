using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogReader : MonoBehaviour {

    //Private Variables
    //private GameObject logReaderObj;
    private GameManager gM;
    private AudioClip paperSound;
    private GameObject player;

    //Text input variable
    [TextArea(15, 20)]
    public string textToWrite = "Write Text Here...";    


	// Use this for initialization
	void Start () {

        //Get the gamemanager.
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //Get player
        player = GameObject.FindGameObjectWithTag("Player");

        //Get the log reader (Not needed I think(Did this quickly)).
        //logReaderObj = gM.logReaderObj;

        paperSound = Resources.Load<AudioClip>("Audio/SFX/paper sound");
	}
	
	// Update is called once per frame
	void Update () {
		


	}

    public void Interaction()
    {
        //Play sound
        player.GetComponent<AudioSource>().PlayOneShot(paperSound);

        //Add the text to the diary list.
        gM.diaryPages.Add(textToWrite);

        //Set the diary as active.
        gM.diaryIsActive = true;

        //Set this text as the main text to read.
        gM.diaryText.text = gM.diaryPages[gM.diaryPages.Count - 1];

        //Destroy the paper.
        Destroy(gameObject);
    }
}
