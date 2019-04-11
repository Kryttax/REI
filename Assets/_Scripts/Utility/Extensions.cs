using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

///<summary>
/// This class contains several extensions usefull while scripting with Unity API.
///</summary>
public static class Extensions 
{
    /// <summary>
    /// Returns a list of children attached to this gameobject in the hierarchy.
    /// </summary>
    /// <param name="gob"></param>
    /// <returns></returns>
    public static List<GameObject> GetChildrenList(this GameObject gob)
    {
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < gob.transform.childCount; i++)
        {
            list.Add(gob.transform.GetChild(i).gameObject);
        }
        return list;
    }

	/// <summary>
    /// Returns a list of children transform attached to this gameobject in the hierarchy.
    /// </summary>
    /// <param name="gob"></param>
    /// <returns></returns>
    public static List<Transform> GetChildrenTransformList(this GameObject gob)
    {
        List<Transform> list = new List<Transform>();

        for (int i = 0; i < gob.transform.childCount; i++)
        {
            list.Add(gob.transform.GetChild(i));
        }
        return list;
    }

    /// <summary>
    /// Returns an array of children attached to this gameobject in the hierarchy.
    /// </summary>
    /// <param name="gob"></param>
    /// <returns></returns>
    public static GameObject[] GetChildrenArray(this GameObject gob)
    {
        GameObject[] childrenArray = new GameObject[gob.transform.childCount];

        for (int i = 0; i < childrenArray.Length; i++)
        {
            childrenArray[i] = gob.transform.GetChild(i).gameObject;
        }

        return childrenArray;
    }

	/// <summary>
    /// Returns an array of children transform attached to this gameobject in the hierarchy.
    /// </summary>
    /// <param name="gob"></param>
    /// <returns></returns>
    public static Transform[] GetChildrenTransformArray(this GameObject gob)
    {
        Transform[] childrenArray = new Transform[gob.transform.childCount];

        for (int i = 0; i < childrenArray.Length; i++)
        {
            childrenArray[i] = gob.transform.GetChild(i);
        }

        return childrenArray;
    }

    public static GameObject FindChildByName(this List<GameObject> list, string name) 
    {
        foreach (GameObject gob in list) 
        {
            if (gob.name.Equals(name))
                return gob;
        }
        return null;
    }

    /// <summary>
    /// Returns the current active toggle in a toggle group.
    /// </summary>
    /// <param name="tGroup"></param>
    /// <returns></returns>
    public static Toggle GetActiveInToggleGroup(this ToggleGroup tGroup)
    {
        return tGroup.ActiveToggles().FirstOrDefault(); //Return the first active toggle in the group
    }
}
