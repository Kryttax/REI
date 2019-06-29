using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuseBox : MonoBehaviour 
{
    #region Global Var Field
    public GameObject[] fuseHolders;

    [SerializeField]
    private Material mat;
    private GameObject player;
    private AudioClip fusePlaceSound;
    private AudioSource audioSource;
    private bool showMessage;
    private float elapedTime;
    private GameObject message;
    private bool canPlace = false;
    private MotorRotation windMillMotor;
    #endregion

    #region Properties
    //public bool CanRotate { get; set; } //Used in an old script
    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (GameObject.Find("Message"))
            message = GameObject.Find("Message");
        else 
            Debug.LogError("<color=red>Error:</color> No object named \"Message\" could be found on Canvas!", this);
    }

	// Use this for initialization
	void Start () 
    {
        if (transform.parent.parent.parent.GetComponentInChildren<MotorRotation>())
            windMillMotor = transform.parent.parent.parent.GetComponentInChildren<MotorRotation>();
        else 
            Debug.LogError("<color=red>Error:</color> FuseBox code is trying to find a Motor Script through triple parent referencing, Check code and hierarchy on " + this.gameObject.name, this);

        fusePlaceSound = (AudioClip)Resources.Load("Audio/SFX/InsertItem01");
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (GameObject.FindGameObjectsWithTag("FuseHolder").Length > 0)
            fuseHolders = GameObject.FindGameObjectsWithTag("FuseHolder");
        else
            Debug.LogError("Couldn't find any objects tagged fuseholder!");
        
        if (Resources.Load<Material>("Models/Assets/Materials/Box_0006_Green_Fuse_Light"))
            mat = Resources.Load<Material>("Models/Assets/Materials/Box_0006_Green_Fuse_Light");
        else
            Debug.LogError("<color=red>Error:</color> Missing material to change on broken fuse, check Resource path if it is the correct path!", this);

        //Deactivate the "missing item" message on canvas if active
        if (message.activeInHierarchy)
            message.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Track how long the message box has been shown
        if (showMessage)
        {
            elapedTime += Time.deltaTime;
            if (elapedTime >= 5f)
            {
                message.SetActive(false);
                showMessage = false;
            }
        }
    }

    public void Interaction()
    {
        if (canPlace)
        {
            if (player.GetComponent<InventoryScript>().inventory.Contains("Fuse"))
            {
                player.GetComponent<InventoryScript>().inventory.Remove("Fuse");    //Remove Fuse from inventory
                audioSource.PlayOneShot(fusePlaceSound);                                 //Play sound
                transform.parent.GetComponentInChildren<ParticleSystem>().Stop();   //Stop sparkling electricity particle system

                SimTracker.MilestoneEvent mlstn = new SimTracker.MilestoneEvent(GameManager.instance.GetSceneNumber(), player.transform.position.x,
                    player.transform.position.y, player.transform.position.z, "FUSE PLACED CORRECTLY");
                SimTracker.SimTracker.instance.PushEvent(mlstn);

                canPlace = false;                   //Can no longer place an object in the fusebox
                gameObject.tag = "Untagged";        //Make it non-interactable
                windMillMotor.IsWorking = true;     //Set the motors state to working

                //Turn on the mesh renderer to simulate that the fuse has been switched
                GetComponent<MeshRenderer>().enabled = true;

                //Change from red to green material on the fuseholders
                if (mat != null && fuseHolders.Length > 0) 
                {
                    foreach (GameObject fuseHolder in fuseHolders)
                    {
                        Material[] mats = fuseHolder.GetComponent<MeshRenderer>().materials;
                        mats[2] = mat;  //Fuseholder materials are located as the third material in the list on the corresponding mesh
                        fuseHolder.GetComponent<MeshRenderer>().materials = mats;
                    }
                }
            }
        }
        else if(player.GetComponent<InventoryScript>().inventory.Contains("Fuse") && !canPlace) //Player interacts, has the item but haven't "removed" the old one
        {
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = false;   //"Removing" fuse, deactivating it for simplicity
                canPlace = true;                                //Can now place the item in the fuse slot
            }
        }
        else if(!player.GetComponent<InventoryScript>().inventory.Contains("Fuse")) //Player tries to interact without the item in inventory
            {
                if (!message.activeInHierarchy)     //If not active in hierarchy, activate it and start cooldown counting
                {
                    elapedTime = 0;
                    showMessage = true;
                    message.SetActive(true);
                message.GetComponent<Text>().text = "You're missing the required item!";
                }
            }
    }

}
