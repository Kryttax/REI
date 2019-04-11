using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseGeneration : MonoBehaviour {

    private GameObject fuse;
    private GameObject tempFuse;
    private List<GameObject> fuses;
    private GameObject player;
    private FuseMinigameTrigger fuseGame;
    private float elapsedTime;
    private bool startTime = false;
    private bool canThrow = false;
    private bool addForce = false;
    private bool hasFuse = false;
    [SerializeField] private float force = 500f;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        fuse = Resources.Load<GameObject>("Prefabs/FuseBroke");
        fuseGame = GameObject.Find("FuseTrigger").GetComponent<FuseMinigameTrigger>();
	}
	
	// Update is called once per frame
	void Update () {

        if (tempFuse)
        {

            if (startTime)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > 0.3f)
                {
                    canThrow = true;
                    elapsedTime = 0f;
                }
            }

            if (Input.GetMouseButtonDown(0) && canThrow)
            {
                tempFuse.GetComponent<Rigidbody>().isKinematic = false;
                tempFuse.transform.parent = null;
                addForce = true;
                canThrow = false;
            }
        }

	}

    public void Interaction()
    {
        if (!tempFuse)
        {
            fuseGame.StartGame = true;
            tempFuse = Instantiate(fuse, transform.position, Quaternion.identity);
            tempFuse.AddComponent<Rigidbody>().isKinematic = true;
            tempFuse.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            tempFuse.transform.parent = player.transform;
            startTime = true;
        }
    }

    private void FixedUpdate()
    {
        if (addForce)
        {
            tempFuse.GetComponent<Rigidbody>().AddForce(player.transform.forward * force * Time.deltaTime, ForceMode.Impulse);
            tempFuse = null;
            addForce = false;
        }

    }
}
