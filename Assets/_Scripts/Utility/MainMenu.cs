using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private GameObject start;
    private GameObject levels;
    private GameObject wind;
    private GameObject solar;
    private GameObject geo;
    private GameObject backbutton;
    private GameObject levelsGameObject;
    private GameObject pressAnyButtonGameObject;
    private GameObject creditsButton;

    private bool pushedAnyKey = false;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pressAnyButtonGameObject = GameObject.Find("PressAnyButton");
        levelsGameObject = GameObject.Find("Levels");
        backbutton = GameObject.Find("BackButton");
        levels = GameObject.Find("LevelButton");
        start = GameObject.Find("StartButton");
        creditsButton = GameObject.Find("Credits");
        //		wind = GameObject.Find ("WindButton");
        //		solar = GameObject.Find ("SolarButton");
        //		geo = GameObject.Find ("GeoButton");

        //		wind.SetActive (false);
        //		solar.SetActive (false);
        //		geo.SetActive (false);
        start.SetActive(false);
        levels.SetActive(false);
        levelsGameObject.SetActive(false);
        creditsButton.SetActive(false);
    }

    private void Update()
    {
        if (!pushedAnyKey)
        {
            if (Input.anyKey)
            {
                start.SetActive(true);
                levels.SetActive(true);
                levelsGameObject.SetActive(false);
                pressAnyButtonGameObject.SetActive(false);
                creditsButton.SetActive(true);
                pushedAnyKey = true;
            }
        }
        else
            return;
    }


    public void OnLevelClick (){
		wind.SetActive (true);
		solar.SetActive (true);
		geo.SetActive (true);
		start.SetActive (false);
		levels.SetActive (false);
	}


    public void LevelSelectButton()
    {
        start.SetActive(false);
        levels.SetActive(false);
        levelsGameObject.SetActive(true);
    }


    public void Back()
    {
        start.SetActive(true);
        levels.SetActive(true);
        levelsGameObject.SetActive(false);
    }


	public void LoadSceneByInde(int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
	}
}