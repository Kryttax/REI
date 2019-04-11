using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	/// <summary>
	/// This is a generic Singleton class.
	/// Inherit this class to make the derived class have singleton pattern.
	///
	/// REMEMBER: 
	///	Override awake function and call base.Awake() !
	/// If there are several objects of a singleton component in the hierarchy, e.g several GameManagers,
	/// this script will keep the one appearing last in the hierarchy.
    /// </summary>
public class GenericSingletonMonobehaviour<T> : MonoBehaviour where T : Component
{
	private static T instance;	//Static instance belonging to class

	public static T Instance 	//Property of <T> instance
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<T> ();

           		if (instance == null) 
				{
             		GameObject obj = new GameObject ();
             		obj.name = typeof(T).Name;
             		instance = obj.AddComponent<T>();
           		}
			}
			return instance;
        }
    }
	public virtual void Awake ()
    {
		if (instance == null) 
	   	{
         	instance = this as T;
         	DontDestroyOnLoad (this.gameObject);
    	} 
		else 
      		Destroy (gameObject);

		print("Singleton awake run on: " + typeof(T).Name);
  	}
}