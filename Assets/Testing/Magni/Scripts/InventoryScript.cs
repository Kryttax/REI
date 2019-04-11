using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour {

	public List<string> inventory = new List<string>();
    private List<GameObject> images;
    //private GameObject mainCanvas;
    private GameObject panel;
    private bool inInventory = false;
    private GameObject message;
    public bool hasComeUp = false;
    private GameObject logMessage;
    private GameManager gM;

    public GameObject Message
    {
        get { return message; }
        set { message = value; }
    }

    private void Awake()
    {
        logMessage = GameObject.Find("LogReaderMessage");
        Message = GameObject.Find("Message");
    }

    // Use this for initialization
    void Start () {

        //mainCanvas = GameObject.Find("AimCanvasTest");
        panel = GameObject.Find("InventoryPanel");
        images = panel.GetChildrenList();
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (message)
        {
            if (Message.activeSelf == true)
                Message.SetActive(false);
        }


        logMessage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Tab))
        {
            if (message)
            {
                if (message.activeSelf == true)
                    message.SetActive(false);
            }

            inInventory = true;
        }
        else
        {
            inInventory = false;
        }

        if (inInventory)
        {

            panel.SetActive(true);

            if(gM.diaryPages.Count != 0)
                logMessage.SetActive(true);

            for (int i = 0; i < inventory.Count; i++)
            {
                images[i].SetActive(true);
            }
        }
        else
        {

            for (int i = 0; i < images.Count; i++)
            {
                images[i].SetActive(false);
            }
            panel.SetActive(false);

            if(logMessage.activeSelf == true)
                logMessage.SetActive(false);

        }

        if (inventory.Count >= 1)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                images[i].GetComponent<RawImage>().texture = (Texture)Resources.Load("Textures/UI/" + inventory[i]);
            }
        }

	}

}
