using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.EventSystems;

public class ControlPanelTopDown : MonoBehaviour {

    //Private vars
    private bool isMovingUp = false;
    private Transform point;
    private GameObject cam;
    private FirstPersonController fps;
    private GameManager gM;
    private Vector3 origPos;
    private Quaternion origRot;
    private bool getPos = true;
    private bool isMovingDown = false;
    private GameObject[] wayPoints;
    private Vector3 clickPoint;
    private bool cartMove = false;
    private Vector3 railPoint;
    private GameObject fixCart;
    private Quaternion origRotFixCart;
    private bool getCarRotation = false;
    private bool moveToRailPoint = true;
    private bool moveToRailPoint2 = false;
    private bool canMoveCart = true;
    private bool moveBack = false;
    private Vector3 origPosistionBegin;
    private bool isChosen = false;
    private bool isWaiting = false;
    private float elapsedTime = 0f;
    [SerializeField] private float waitTime = 3f;
    [SerializeField] private GameObject chosenCleaningRobot;
    [SerializeField] private bool cleaningChosen = false;
    private bool cleaningMove = false;
    private GameObject[] cleaningRobots;
    private GameObject uiElement3;
    private GameObject topDownUIElement;
    private bool startMovingFix = false;
    private bool moveForwardClean = true;
    private bool moveBackClean = false;
    private bool getPosOrigClean = true;
    private Vector3 posOrigClean;
    private CleanBotScript cBS;
    public bool isCleaning;

    //Properties
    public bool IsChosen
    {
        get { return isChosen; }
        set { isChosen = value; }
    }

    public Vector3 ClickPoint
    {
        get { return clickPoint; }
        set { clickPoint = value; }
    }

    public bool IsWaiting
    {
        get { return isWaiting; }
        set { isWaiting = value; }
    }

    // Use this for initialization
    void Start () {

        //Top down element, buttons and such
        topDownUIElement = GameObject.FindGameObjectWithTag("UITopDown");

        //The elements to set inactive when in top down
        uiElement3 = GameObject.Find("CrosshairImage");

        //Find an array of cleaning robots
        cleaningRobots = GameObject.Find("CleaningRobots").GetChildrenArray();

        //Camera point for top down
        point = GameObject.FindGameObjectWithTag("CamPoint").transform;

        //Camera on the player
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        //FPS Script
        fps = cam.transform.parent.GetComponent<FirstPersonController>();

        //Get the game manager
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //Get the waypoints in the scene
        if (GameObject.FindGameObjectWithTag("WayPoints"))
            wayPoints = GameObject.FindGameObjectWithTag("WayPoints").GetChildrenArray();

        //Get the fix cart AI robot
        fixCart = GameObject.FindGameObjectWithTag("FixCart");

        //The posision where the fix cart began
        origPosistionBegin = fixCart.transform.position;

        //Set the buttons inactive
        topDownUIElement.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        //Wait time that is used when fixing the rail, for animation
        if (IsWaiting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= waitTime)
            {
                IsWaiting = false;
                elapsedTime = 0f;
            }
        }

        if (isMovingUp)
        {

            //Move the original rotation and position of the camera
            if (getPos)
            {
                origRot = cam.transform.rotation;
                origPos = cam.transform.position;
                getPos = false;
            }

            //Move the camera to the top down position and right rotation and disable the movement the fps controller
            cam.transform.position = Vector3.Slerp(cam.transform.position, point.position, 10f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, point.rotation, 5f * Time.deltaTime);
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, 75f, Time.deltaTime * 5f);
            fps.isTopDown = true;

            //When the camera is in top down mode, show the cursor
            if (Vector3.Distance(cam.transform.position, point.position) <= 0.01f)
            {
                gM.IsTopDown = true;
                isMovingUp = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                uiElement3.SetActive(false);

                topDownUIElement.SetActive(true);
            }

        }

        if (cleaningMove)
        {
            if (getPosOrigClean)
            {
                posOrigClean = chosenCleaningRobot.transform.position;
                getPosOrigClean = false;
            }

            if (moveForwardClean)
            {
                if(!isCleaning)
                    chosenCleaningRobot.transform.position = Vector3.MoveTowards(chosenCleaningRobot.transform.position, new Vector3(railPoint.x, railPoint.y, clickPoint.z), Time.deltaTime * 40f);
                else
                    chosenCleaningRobot.transform.position = Vector3.MoveTowards(chosenCleaningRobot.transform.position, new Vector3(railPoint.x, railPoint.y, clickPoint.z), Time.deltaTime * 15f);
            }

            if(Vector3.Distance(chosenCleaningRobot.transform.position, new Vector3(railPoint.x, railPoint.y, clickPoint.z)) <= 0.01f)
            {
                moveBackClean = true;
                moveForwardClean = false;
            }

            if(moveBackClean)
            {
                chosenCleaningRobot.transform.position = Vector3.MoveTowards(chosenCleaningRobot.transform.position, posOrigClean, Time.deltaTime * 40f);

                if (Vector3.Distance(chosenCleaningRobot.transform.position, posOrigClean) <= 0.01f)
                {
                    moveBackClean = false;
                    cleaningMove = false;
                    moveForwardClean = true;
                    getPosOrigClean = true;
                }
            }

        }

        if (canMoveCart && IsChosen && !IsWaiting && startMovingFix)
        {
            cartMove = true;

            //Move the rail to the closest point to the clicked point on the main rail
            if (moveToRailPoint)
                fixCart.transform.position = Vector3.MoveTowards(fixCart.transform.position, railPoint, 50f * Time.deltaTime);

            //Check distance of fixing AI cart to the rail point on the main rail
            if (Vector3.Distance(fixCart.transform.position, railPoint) <= 0.1f)
            {
                //Stop moving on the main rail
                moveToRailPoint = false;

                //Get the original rotation of the cart
                if (!getCarRotation)
                {
                    origRotFixCart = fixCart.transform.rotation;
                    getCarRotation = true;
                }

                //Check which direction to turn to and then turn
                if(clickPoint.z - origPosistionBegin.z < 0f)
                    fixCart.transform.Rotate(transform.up * 60f * Time.deltaTime);
                else if (clickPoint.z - origPosistionBegin.z > 0f)
                    fixCart.transform.Rotate(transform.up * -60f * Time.deltaTime);

                //When fully turned, start moving on the second rail
                if (Quaternion.Angle(fixCart.transform.rotation, origRotFixCart) >= 90f)
                {
                    moveToRailPoint2 = true;
                }
            }

            //Move to the closest point of the clicked point according to Z position
            if (moveToRailPoint2)
            {
                fixCart.transform.position = Vector3.MoveTowards(fixCart.transform.position, new Vector3(railPoint.x, railPoint.y, clickPoint.z), 30f * Time.deltaTime);

                //Reset
                if (Vector3.Distance(fixCart.transform.position, new Vector3(railPoint.x, railPoint.y, clickPoint.z)) <= 0.1f)
                {
                    getCarRotation = false;
                    moveToRailPoint = true;
                    moveToRailPoint2 = false;
                    //cartMove = false;
                    canMoveCart = false;
                    moveBack = true;
                }

            }

        }

        if (moveBack && IsChosen && !isWaiting)
        {
            //Move back to the main rail
            if (moveToRailPoint)
                fixCart.transform.position = Vector3.MoveTowards(fixCart.transform.position, railPoint, 30f * Time.deltaTime);

            //Check distance and stop moving ton the second rail
            if (Vector3.Distance(fixCart.transform.position, railPoint) <= 0.1f)
            {
                moveToRailPoint = false;

                //Get original cart rotation
                if (!getCarRotation)
                {
                    origRotFixCart = fixCart.transform.rotation;
                    getCarRotation = true;
                }

                //As before, just reverced
                if (clickPoint.z - origPosistionBegin.z < 0f)
                    fixCart.transform.Rotate(transform.up * -60f * Time.deltaTime);
                else if (clickPoint.z - origPosistionBegin.z > 0f)
                    fixCart.transform.Rotate(transform.up * 60f * Time.deltaTime);

                //When finished rotating, start moving on the main rail
                if (Quaternion.Angle(fixCart.transform.rotation, origRotFixCart) >= 90f)
                {
                    moveToRailPoint2 = true;
                }
            }


            if (moveToRailPoint2)
            {
                //Move back to its original position
                fixCart.transform.position = Vector3.MoveTowards(fixCart.transform.position, origPosistionBegin, 50f * Time.deltaTime);

                //Reset
                if (Vector3.Distance(fixCart.transform.position, origPosistionBegin) <= 0.1f)
                {
                    getCarRotation = false;
                    moveToRailPoint = true;
                    moveToRailPoint2 = false;
                    canMoveCart = true;
                    moveBack = false;
                    cartMove = false;
                    startMovingFix = false;
                }

            }
        }

        //Quit top down mode of Q is pressed
        if (gM.IsTopDown && Input.GetKeyDown(KeyCode.Q))
            isMovingDown = true;

        if (gM.IsTopDown)
        {

            //If the cart is at the beginning and mouse is pressed, start point calculation
            if (((!cartMove && !moveBack) && IsChosen && !IsWaiting && !EventSystem.current.IsPointerOverGameObject()) || (cleaningChosen && !cleaningMove && !EventSystem.current.IsPointerOverGameObject()))
                PointClick();

            /*
            if (Input.GetMouseButtonDown(0) && !IsWaiting)
            {
                Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("FixCart") && !cleaningMove)
                    {
                        //Click to choose the fixing robot
                        IsChosen = true;
                        cleaningChosen = false;
                        chosenCleaningRobot = null;
                    }
                    else if (hit.collider.gameObject.CompareTag("Cleaning") && canMoveCart)
                    {
                        //Click to choose the cleaning robot
                        chosenCleaningRobot = hit.collider.gameObject;
                        cleaningChosen = true;
                        IsChosen = false;
                        cartMove = false;
                    }
                }
            }
            */

        }

        if(isMovingDown)
        {
            //Move Camera back to original position
            topDownUIElement.SetActive(false);
            cam.transform.position = Vector3.Slerp(cam.transform.position, origPos, 10f * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, origRot, 5f * Time.deltaTime);
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, 60f, Time.deltaTime * 5f);

            //Activate everything and make everything normal in first person
            if (Vector3.Distance(cam.transform.position, origPos) <= 0.01f)
            {
                fps.isTopDown = false;
                gM.IsTopDown = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                getPos = true;
                isMovingDown = false;

                uiElement3.SetActive(true);
            }

        }

	}

    public void Interaction()
    {
        //Make the camera move up to camera point when interacted with
        isMovingUp = true;
    }

    private void PointClick()
    {
        //Send out a raycast to get a click point
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 1000f) && !EventSystem.current.IsPointerOverGameObject())
        {
            clickPoint = hit.point;

            //Start point calculation
            if (IsChosen)
            {
                railPoint = PointCalculation();
            }
            else if (cleaningChosen)
            {
                chosenCleaningRobot = RobotCalculation();
                railPoint = PointCalculation();
            }

            //Start moving the cart to the calculated point
            if (IsChosen)
                startMovingFix = true;
            else if (cleaningChosen && !chosenCleaningRobot.GetComponent<CleanBotScript>().isBrokenRail)
                cleaningMove = true;
        }
    }

    private Vector3 PointCalculation()
    {
        //Get the highest number posible
        float nearestSqrMag = float.PositiveInfinity;

        //Make a vector as a information holder
        Vector3 nearestVector3 = Vector3.zero;

        //Similar to Vector.Distance but is gets the closest point in each for and removes the second closest and so on
        for (int i = 0; i < wayPoints.Length; i++)
        {
            float sqrMag = (wayPoints[i].transform.position - clickPoint).sqrMagnitude;
            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestVector3 = wayPoints[i].transform.position;
            }
        }

        //Return the closest
        return nearestVector3;
    }

    private GameObject RobotCalculation()
    {
        //Get the highest number posible
        float nearestSqrMag = float.PositiveInfinity;

        //Make a vector as a information holder
        GameObject nearestRobot = null;

        //Similar to Vector.Distance but is gets the closest point in each for and removes the second closest and so on
        for (int i = 0; i < cleaningRobots.Length; i++)
        {
            float sqrMag = (cleaningRobots[i].transform.position - clickPoint).sqrMagnitude;
            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestRobot = cleaningRobots[i];
            }
        }

        //Return the closest
        return nearestRobot;
    }

    public void Fixing()
    {
        //Click to choose the fixing robot
        if(!cleaningMove)
        {
            IsChosen = true;
            cleaningChosen = false;
            chosenCleaningRobot = null;
        }
    }

    public void Cleaning()
    {
        //Click to choose the cleaning robot
        if (!cartMove)
        {
            Debug.Log("Press Clean Button");
            cleaningChosen = true;
            IsChosen = false;
            //cartMove = false;
        }
    }
}
