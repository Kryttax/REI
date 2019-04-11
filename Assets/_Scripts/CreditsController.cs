using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    private bool hasStopped = false;

	void Awake ()
    {
        Time.timeScale = 1f;
	}
	

	void Update ()
    {
        if (!hasStopped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpeedUp();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SlowDown();
            }
            return;
        }
    }


    private void SpeedUp()
    {
        Time.timeScale = 10f;
    }


    private void SlowDown()
    {
        Time.timeScale = 1f;
    }


    public void OnCreditsStop()
    {
        hasStopped = true;
        SlowDown();

        Invoke("GoToMainMenu", 5f);
    }


    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
