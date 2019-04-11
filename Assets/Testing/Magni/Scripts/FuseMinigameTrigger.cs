using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseMinigameTrigger : MonoBehaviour {

    //Vars
    [SerializeField] private int endAmountOfFuses = 5;
    [SerializeField] private float endTime = 60f;
    [SerializeField] private int amountOfFuses;
    private GameObject fuse;
    private bool startGame = false;
    [SerializeField] private float elapsedTime;
    private TextMesh timeText;
    private TextMesh amoutText;
    private GameObject[] grinders;
    private GameObject poof;
    private GameObject fusePile;
    [SerializeField] private float timeShoot = 0f;
    private bool shoot = true;
    private GameObject shootParticle;
    private AudioClip poofSound;
    private AudioSource source;
    private AudioClip pewSound;

    //Properties
    public int AmountOfFuses
    {
        get { return amountOfFuses; }
        set { amountOfFuses = value; }
    }

    public bool StartGame
    {
        get { return startGame; }
        set { startGame = value; }
    }

    private void Awake()
    {
        fuse = GameObject.Find("Fuse");
        fuse.SetActive(false);
    }

    // Use this for initialization
    void Start () {

        poofSound = Resources.Load<AudioClip>("Audio/SFX/Misc/Poof");
        shootParticle = transform.parent.GetChild(4).gameObject;
        fusePile = GameObject.Find("FusePile");
        poof = transform.parent.GetChild(3).gameObject;
        grinders = GameObject.FindGameObjectsWithTag("Grinders");
        timeText = transform.parent.GetChild(2).GetComponent<TextMesh>();
        amoutText = transform.parent.GetChild(1).GetComponent<TextMesh>();
        source = GetComponent<AudioSource>();
        pewSound = Resources.Load<AudioClip>("Audio/SFX/Misc/Pew");

    }
	
	// Update is called once per frame
	void Update () {

        if (startGame)
        {
            timeText.text = "Time: " + (int)elapsedTime + " / " + endTime;
            amoutText.text = "Hit: " + AmountOfFuses + " / " + endAmountOfFuses;

            elapsedTime += Time.deltaTime;

            grinders[0].transform.Rotate(transform.right * Time.deltaTime * 25f);
            grinders[1].transform.Rotate(-transform.right * Time.deltaTime * 25f);

            if (elapsedTime >= endTime)
            {
                AmountOfFuses = 0;
                elapsedTime = 0f;
                startGame = false;
            }
        }

        if(AmountOfFuses >= endAmountOfFuses)
        {
            fuse.SetActive(true);
            fuse.GetComponent<Animator>().SetTrigger("Throw");
            shootParticle.GetComponent<ParticleSystem>().Play();
            shootParticle.GetComponent<AudioSource>().PlayOneShot(pewSound);

            shoot = true;

            elapsedTime = 0f;
            startGame = false;
            AmountOfFuses = 0;
            fusePile.transform.tag = "Untagged";

        }


        if (shoot)
        {
            timeShoot += Time.deltaTime;

            if (timeShoot >= 1f)
            {
                fuse.GetComponent<CapsuleCollider>().enabled = true;
                shoot = false;
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BrokenFuse"))
        {
            poof.GetComponent<ParticleSystem>().Play();
            AmountOfFuses++;
            other.gameObject.tag = "Untagged";
            source.PlayOneShot(poofSound);
        }
    }
}
