using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// This class loads all needed controller and managers in the scene.
/// It instantiates prefabs located in /Resources/Prefabs/
/// This class is executed in edit mode.
///</summary>
[ExecuteInEditMode]
public class SingletonLoader : MonoBehaviour 
{
	private void Update ()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        //if (GameManager.Instance == null)
                
            //Instantiate gameManager prefab
            //Instantiate(Resources.Load("/Prefabs/GameManager", typeof(GameObject)));
            
            //Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
            //if (SoundManager.instance == null)
                
                //Instantiate SoundManager prefab
                //Instantiate(soundManager);
    }
}
