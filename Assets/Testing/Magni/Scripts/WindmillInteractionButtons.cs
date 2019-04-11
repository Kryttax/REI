#pragma warning disable 0414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillInteractionButtons : MonoBehaviour 
{
    public Enum_WM.Position pos;
    public GameObject windMill;
    private WMMotorRotation motorScript;
    private GameManager gM;
    private AudioSource source;
    private AudioClip buttonPress;
    private AudioClip buzz;
    private FuseBox fuseB;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        if (windMill) 
        {
            motorScript = windMill.transform.GetComponentInChildren<WMMotorRotation>();
        }
        else Debug.LogError("No windmill tied to gameobject: " + this.gameObject.name);

        player = GameObject.FindGameObjectWithTag("Player");

        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        buttonPress = (AudioClip)Resources.Load("Audio/SFX/ButtonPush01");
        buzz = (AudioClip)Resources.Load("Audio/SFX/Buzz Fade Out-SoundBible.com-286120031");
        source = gameObject.AddComponent<AudioSource>();

        if (GameObject.Find("FuseBox"))
        {
            fuseB = GameObject.Find("FuseBox").GetComponent<FuseBox>();
        }
    }

    // Update is called onpos per frame
    void Update()
    {
        pos = motorScript.position;
        if (pos == gM.CorrectPos && !motorScript.IsRotating)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void Interaction()
    {
        player.GetComponent<InteractionScript>().Holding = true;
        if(windMill.name == "WindmillGL4 (2)")
        {
            //FixedWindMill();
        }
        else
        {
            WindmillPositions();
        }
    }

    private void startRotate(Enum_WM.Position pos) 
    {
        motorScript.position = pos;
        motorScript.IsRotating = true;
    }

    /*private void FixedWindMill()
    {
        if (!fuseB.CanRotate)
            source.PlayOneShot(buzz);
        else
        {
            WindmillPositions();
        }
    }*/

    private void WindmillPositions()
    {
        source.PlayOneShot(buttonPress);
        if (pos == Enum_WM.Position.zero && !motorScript.IsRotating)
        {
            pos = Enum_WM.Position.ninety;
            Debug.Log(transform.name + " : 90");
            startRotate(pos);
        }
        else if (pos == Enum_WM.Position.ninety && !motorScript.IsRotating)
        {
            pos = Enum_WM.Position.oneEighty;
            Debug.Log(transform.name + " : 180");
            startRotate(pos);
        }
        else if (pos == Enum_WM.Position.oneEighty && !motorScript.IsRotating)
        {
            pos = Enum_WM.Position.twoSeventy;
            Debug.Log(transform.name + " : 270");
            startRotate(pos);
        }
        else if (pos == Enum_WM.Position.twoSeventy && !motorScript.IsRotating)
        {
            pos = Enum_WM.Position.zero;
            Debug.Log(transform.name + " : 0");
            startRotate(pos);
        }
    }
}
