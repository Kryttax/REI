using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public int laserLength = 20;
    private float repairStrength = 100f;

    public float tiltRotationAmount = 20f;
    public float upForce;
    public float movementForwardSpeed = 400f;
    public float movementStrafeSpeed = 300f;
    private float tiltStrafeAmount;
    private float tiltStrafeVelocity;

    public float maximumHorizontalVelocity = 10f;
    public float maximumVerticalVelocity = 10f;
    public float timeToLimitVelocity = 5f;
    public float timeToSmoothVelocity = 0.95f;

    private float tiltAmountForward = 0f;
    private float tiltVelocityForward;

    private float wantedYRotation;
    private float currentYRotation;
    private float rotateAmountByKeys = 3.5f;
    private float rotationYVelocity;

    private Vector3 velocityToSmoothDampToZero;

    private Rigidbody droneRB;
    private Transform droneModel;

    private bool playerHasControl;

    private Transform middlePoint;

    private LineRenderer laser;
    private GameObject weldingPartSys;
    private GameObject burntSmokePartSys;
    private Camera cam;


    void Awake()
    {
        droneRB = GetComponent<Rigidbody>();
        droneModel = this.transform.GetChild(0);
        middlePoint = GameObject.Find("DroneMiddlePoint").transform;
        playerHasControl = false;
        laser = gameObject.GetComponentInChildren<LineRenderer>();
        laser.enabled = false;

        weldingPartSys = Resources.Load<GameObject>("Prefabs/Particle Systems/WeldingSparksPartSys") as GameObject;
        burntSmokePartSys = Resources.Load<GameObject>("Prefabs/Particle Systems/BurnSmokePartSys") as GameObject;
        cam = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Camera>();
    }


    private void Update()
    {
        Vector3 pos = middlePoint.position;
        pos.y = transform.position.y;
        middlePoint.position = pos;

        LaserControls();

    }


    void FixedUpdate()
    {
        Quaternion targRot = Quaternion.LookRotation(middlePoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targRot, 10f * Time.deltaTime);

        MovementUpDown();
        MovementForward();
        //Rotation();
        StrafeAmount();
        ClampSpeedValues();
        droneRB.AddRelativeForce(Vector3.up * upForce);
        // Since tilting the gameobject also changes our heading, we might want to only tilt the model, but this might be subjective
//      droneModel.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, droneRB.rotation.z));
//      droneRB.rotation = Quaternion.Euler(new Vector3(tiltAmountForward, currentYRotation, tiltStrafeAmount));
    }


    void MovementUpDown()
    {
        // Stabilization
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            if (Input.GetButton("Ascend") || (Input.GetButton("Descend")))
            {
                droneRB.velocity = droneRB.velocity;
            }
            if (!Input.GetButton("Ascend") && (!Input.GetButton("Descend")) && (!Input.GetKey(KeyCode.LeftArrow) && (!Input.GetKey(KeyCode.RightArrow))))
            {
                droneRB.velocity = new Vector3(droneRB.velocity.x, Mathf.Lerp(droneRB.velocity.y, 0, Time.deltaTime * 5f), droneRB.velocity.z);
                upForce = 281f;
            }
            if (!Input.GetButton("Ascend") && (!Input.GetButton("Descend")) && (Input.GetKey(KeyCode.LeftArrow) || (!Input.GetKey(KeyCode.RightArrow))))
            {
                droneRB.velocity = new Vector3(droneRB.velocity.x, Mathf.Lerp(droneRB.velocity.y, 0, Time.deltaTime * 5f), droneRB.velocity.z);
                upForce = 110f;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.RightArrow)))
            {
                upForce = 410f;
            }
        }

         if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            upForce = 135f;
        }

        if (Input.GetButton("Ascend"))
        {
            upForce = 400f;
            // Stabilization
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce = 500f;
            }

        }
        else if (Input.GetButton("Descend"))
        {
            upForce = -250f;
        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            upForce = 98.1f;
        }
    }


    void MovementForward()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            droneRB.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, tiltRotationAmount * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f);
        }
    }


    void Rotation ()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }


    void StrafeAmount()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRB.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * movementStrafeSpeed);
            tiltStrafeAmount = Mathf.SmoothDamp(tiltStrafeAmount, -tiltRotationAmount * Input.GetAxis("Horizontal"), ref tiltStrafeVelocity, 0.1f);
        }
        else
        {
            tiltStrafeAmount = Mathf.SmoothDamp(tiltStrafeAmount, 0, ref tiltStrafeVelocity, 0.1f);
        }
    }


    void ClampSpeedValues () // For limiting the velocity of the drone
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRB.velocity = Vector3.ClampMagnitude(droneRB.velocity, Mathf.Lerp(droneRB.velocity.magnitude, maximumHorizontalVelocity, Time.deltaTime * timeToLimitVelocity));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            droneRB.velocity = Vector3.ClampMagnitude(droneRB.velocity, Mathf.Lerp(droneRB.velocity.magnitude, maximumHorizontalVelocity, Time.deltaTime * timeToLimitVelocity));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            droneRB.velocity = Vector3.ClampMagnitude(droneRB.velocity, Mathf.Lerp(droneRB.velocity.magnitude, maximumVerticalVelocity, Time.deltaTime * timeToLimitVelocity));
        }

        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            droneRB.velocity = Vector3.SmoothDamp(droneRB.velocity, Vector3.zero, ref velocityToSmoothDampToZero, timeToSmoothVelocity); // Should Time.deltaTime be in here?
        }
    }


    void LaserControls ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }


    IEnumerator FireLaser()
    {
        laser.enabled = true;

        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, cam.transform.forward);
            RaycastHit hit;

            laser.SetPosition(0, ray.origin);

            if (Physics.Raycast(ray, out hit, laserLength))
            {
                laser.SetPosition(1, hit.point);
                if (hit.collider.tag == "BrokenWireBox")
                {
                    Instantiate(weldingPartSys, hit.point, transform.rotation);
                    WireBox damageAmount = hit.collider.GetComponent<WireBox>();
                    if (damageAmount != null)
                    {
                        damageAmount.Repair(repairStrength);
                    }
                }
                // Add other particle effects for when the laser hits other objects.
                else if (hit.collider)
                {
                    Instantiate(burntSmokePartSys, hit.point, transform.rotation);
                }
            }
            else
            {
                laser.SetPosition(1, ray.GetPoint(laserLength));
            }

            yield return null;
        }

        laser.enabled = false;
    }
}
