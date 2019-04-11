using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerStation : MonoBehaviour {

    private CableCar cableCar;
    private WindMillManager wMM;
    private GameObject message;
    private bool showMessage;
    private bool canPlaySound = true;
    private float elapsedTime = 0f;
    private AudioClip buttonPressSound;
    private AudioClip buzzSound;
    private AudioSource source;
    private AudioClip powerSound;
    private GameObject elec;
    private GameObject bottomPower;

    private void Awake()
    {
        if (GameObject.Find("Message"))
            message = GameObject.Find("Message");

    }

    // Use this for initialization
    void Start () {

        elec = transform.parent.parent.GetChild(5).gameObject;
        elec.SetActive(false);

        if (GameObject.Find("New_Cable_Cart"))
            cableCar = GameObject.Find("New_Cable_Cart").transform.GetChild(4).GetChild(3).GetComponent<CableCar>();

        if (GameObject.Find("WindMillManager"))
            wMM = GameObject.Find("WindMillManager").GetComponent<WindMillManager>();

        if (GetComponent<AudioSource>())
            source = GetComponent<AudioSource>();

        bottomPower = GameObject.Find("Top2Colored");

        buttonPressSound = (AudioClip)Resources.Load("Audio/SFX/ButtonPush01");
        buzzSound = (AudioClip)Resources.Load("Audio/SFX/Error01");
        powerSound = Resources.Load<AudioClip>("Audio/SFX/ElectricalBuzz02");

    }
	
	// Update is called once per frame
	void Update () {

        if (showMessage)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 5f)
            {
                message.SetActive(false);
                showMessage = false;
                canPlaySound = true;
            }
        }

    }

    public void Interaction()
    {
        if (wMM.AllCorrect)
        {
            GetComponent<Animator>().SetTrigger("Power");
            cableCar.finished = true;
            source.PlayOneShot(buttonPressSound);
            message.SetActive(true);
            message.GetComponent<Text>().text = "All systems are working again! Go to the cable car to move to the next island";
            showMessage = true;
            elapsedTime = 0f;
            source.PlayOneShot(powerSound);
            transform.tag = "Untagged"; //To prevent further spamming of the button
            elec.SetActive(true);
            if (bottomPower)
            {
                bottomPower.GetComponent<Renderer>().material.color = Color.green;
                bottomPower.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
            }
        }
        else
        {
            if (canPlaySound) 
            {
                source.PlayOneShot(buzzSound);
                canPlaySound = false;
            }

            if (!message.activeInHierarchy) 
            {
                message.GetComponent<Text>().text = "You need full power to turn on the cable car";
                message.SetActive(true);
                showMessage = true;
                elapsedTime = 0f;
            }
        }
    }
}
