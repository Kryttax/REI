using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTriggerScript : MonoBehaviour {

    public float timeToTransition = 4f;

    private GameObject player;
    private float timer;
    private bool isCounting = false;
    private GameObject endPanel;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

        if (GameObject.Find("EndPanel"))
            endPanel = GameObject.Find("EndPanel");

	}
	
	// Update is called once per frame
	void Update () {

        if (isCounting)
        {
            timer += Time.deltaTime;
            if (timer >= timeToTransition)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(player))
        {
            isCounting = true;

            if (endPanel)
                endPanel.GetComponent<Animator>().SetTrigger("Fade");
        }

    }

}
