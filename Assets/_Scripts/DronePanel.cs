using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class DronePanel : MonoBehaviour {

    private GameObject drone;
    private Transform cameraPoint;
    private Transform screenPoint;
    [SerializeField] private bool startMove = false;
    public bool moveBack = false;
    private bool getPos = true;
    private Quaternion origRot;
    private Vector3 origPos;
    private GameObject cam;
    private GameObject fps;
    private GameObject panel;
    public bool isDrone = false;
    [SerializeField] private float elapsedTime = 0f;
    private bool startTime = false;
    private bool doOnce = true;
    private GameObject droneText;
    public int fixedBoxes = 0;

	// Use this for initialization
	void Start () {

        droneText = GameObject.FindGameObjectWithTag("PanelDrone").transform.GetChild(0).gameObject;
        droneText.transform.parent.gameObject.SetActive(false);
        drone = GameObject.Find("Drone");
        drone.GetComponent<DroneController>().enabled = false;
        cameraPoint = drone.transform.GetChild(1);
        screenPoint = transform.parent.GetChild(1);
        fps = GameObject.FindGameObjectWithTag("Player");
        cam = fps.transform.GetChild(0).gameObject;
        panel = GameObject.Find("PanelDrone");

    }
	
	// Update is called once per frame
	void Update () {



        if (startMove)
        {

            //Move the original rotation and position of the camera
            if (getPos)
            {
                //panel.GetComponent<Animator>().SetBool("FadeOut", true);
                fps.GetComponent<FirstPersonController>().isTopDown = true;
                origRot = cam.transform.rotation;
                origPos = cam.transform.position;
                getPos = false;
            }


            //Move the camera to the top down position and right rotation and disable the movement the fps controller
            cam.transform.position = Vector3.Slerp(cam.transform.position, screenPoint.position, 5f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, screenPoint.rotation, 2f * Time.deltaTime);

            //When the camera is in top down mode, show the cursor
            if (Vector3.Distance(cam.transform.position, screenPoint.position) <= 0.01f)
            {

                isDrone = true;

                cam.gameObject.AddComponent<DroneMouseLook>();

                //panel.GetComponent<Animator>().SetBool("FadeOut", false);
                //panel.GetComponent<Animator>().SetBool("FadeIn", true);

                cam.transform.rotation = cameraPoint.transform.rotation;

                drone.GetComponent<DroneController>().enabled = true;
                drone.GetComponent<Rigidbody>().isKinematic = false;

                getPos = true;

                startMove = false;

            }

        }

        if (moveBack)
        {
            if(getPos)
            {
                Destroy(cam.gameObject.GetComponent<DroneMouseLook>());
                drone.GetComponent<DroneController>().enabled = false;
                drone.GetComponent<Rigidbody>().isKinematic = true;
                //panel.GetComponent<Animator>().SetBool("FadeOut", true);
                //panel.GetComponent<Animator>().SetBool("FadeIn", false);
                getPos = false;
            }


            if (doOnce)
            {
                cam.transform.position = screenPoint.position;
                cam.transform.rotation = screenPoint.rotation;
                doOnce = false;
            }

                //panel.GetComponent<Animator>().SetBool("FadeOut", false);
                //panel.GetComponent<Animator>().SetBool("FadeIn", true);

            cam.transform.position = Vector3.Slerp(cam.transform.position, origPos, 5f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, origRot, 2f * Time.deltaTime);

            if (Vector3.Distance(cam.transform.position, origPos) <= 0.01f)
            {

                fps.GetComponent<FirstPersonController>().isTopDown = false;

                getPos = true;

                moveBack = false;

                doOnce = true;

            }

        }

        if (isDrone)
        {
            cam.transform.position = cameraPoint.position;
            //cam.transform.rotation = cameraPoint.rotation;

            droneText.transform.parent.gameObject.SetActive(true);
            droneText.GetComponent<Text>().text = fixedBoxes + " Of 3";
        }

    }

    public void Interaction()
    {
        startMove = true;
    }
}
