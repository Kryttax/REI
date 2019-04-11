using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that open doors, rotating in Y-space relative to its parent and local euler angle
/// </summary>
[RequireComponent(typeof(MessageScript))]
public class OpenDoor : MonoBehaviour 
{
    [Header("The angle the door will open to!")]
    [Range(-180f,180f)]
    public float openAngle = -160.0f;
    [Header("Door object to open.")]
    [Tooltip("The object to be opened. Leave this empty to use the transform of this game object!")]
    public GameObject objectToOpen;
    [Header("The time this interpolation will take. Is one second by default.")]
    public float timeTakenDuringLerp = 1.0f;

    [Header("Rotation values! Not to be fiddled with!")]
    [SerializeField]
    private Vector3 _closedStateRotation; //Start rotation for the interpolation
    [SerializeField]
    private Vector3 _openStateRotation;   //End rotation for this interpolation
    private float _timeStartedLerping;  //The Time.time value when we started the interpolation

    //Whether we are currently interpolating or not
    private bool _isLerping = false;
    private bool isOpen = false;    //Whether the door is open or not
    public bool isDebug = false;    //Are we debugging information?
    public bool interact = false;   //Can be toggled in the editor for testing purposes

    private MessageScript msgScript;

    private void Awake()
    {   
        //Get a reference to the message script component
        msgScript = GetComponent<MessageScript>();
        
        //Get a reference to the object that is supposed to be rotated, reference this transform if none are given in inspector
        if (objectToOpen == null)
            objectToOpen = transform.gameObject;

        _closedStateRotation = objectToOpen.transform.localEulerAngles;
        _openStateRotation = new Vector3(objectToOpen.transform.localEulerAngles.x, openAngle, objectToOpen.transform.localEulerAngles.z);

        if (isDebug) print(string.Format("Object: {0} \nClosed state rotation: {1} \nOpen state rotation: {2}", objectToOpen.name, _openStateRotation, _openStateRotation));
    }

    private void Update() 
    {
        if (interact) {
            Interaction();
            interact = false;
        }

        //If the open angle is changed in the editor while in playmode, update it!
        if (_openStateRotation.y != openAngle)
            _openStateRotation = new Vector3(objectToOpen.transform.localEulerAngles.x, openAngle, objectToOpen.transform.localEulerAngles.z);

    }

    public void Interaction() 
	{
        StartCoroutine(rotateObject(objectToOpen));

        //Update UI to correct text based on door state
        if(!isOpen)
            msgScript.interactionMessage = "Close door [LMB]";
        else
            msgScript.interactionMessage = "Open door [LMB]";
    }

    IEnumerator rotateObject(GameObject gameObjectToRotate) 
    {
        //If player interacts while door is rotating, break
        if (_isLerping)
            yield break;
        
        //If not, we start rotating
        _isLerping = true;
        _timeStartedLerping = Time.time;

        if (isDebug) 
        {
            print(string.Format("{0}, from {1} to {2}", 
                                    isOpen ? "Closing door!" : "Opening door!", 
                                    isOpen ? _openStateRotation : _closedStateRotation,
                                    isOpen ? _closedStateRotation : _openStateRotation));
        }

        while (_isLerping) 
        {
            // We want percentage = 0.0 when Time.time = _timeStartedLerping
            // and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            // In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            // "Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;   //Get the time since we started lerping
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.
            if (!isOpen)
                gameObjectToRotate.transform.localEulerAngles = Vector3.Lerp(_closedStateRotation, _openStateRotation, percentageComplete);
            else 
                gameObjectToRotate.transform.localEulerAngles = Vector3.Lerp(_openStateRotation, _closedStateRotation, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false and update isOpen accordingly
            if (percentageComplete >= 1.0f) 
            {
                _isLerping = false;
                isOpen = !isOpen;
            }

            yield return null;  //Keep checking while-condition in next frame
        }
    }
}
