using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class textos : MonoBehaviour {

	public float fadeTime = 5.0f;

	// Use this for initialization
	void Start () {
		
		Invoke ("call", fadeTime);
		
	}
	public IEnumerator FadeTextToZero(float t, Text i)
	{
			i.color = new Color (i.color.r, i.color.g, i.color.b, 1);
			while (i.color.a > 0.0f) {
				i.color = new Color (i.color.r, i.color.g, i.color.b, i.color.a - (0.3f * (Time.deltaTime / t)));
				yield return null;
			}
	}

	void call () {
		StartCoroutine(FadeTextToZero(1f, GetComponent<Text>()));
	}


		
}


