﻿using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Transform exit;
    static Transform last;
	private AudioSource source;
	public AudioClip teleportAudio;
	private Vector3 plus = new Vector3 (0.5f,0.0f,0.0f);

	void Start ()
	{
		source = GetComponent<AudioSource>();
	}

    void OnTriggerEnter(Collider other)
    {
        //source.pitch = Random.Range (0.8f, 1.2f);
        Vector3 pos = GameObject.FindGameObjectWithTag("Player").transform.position;
        SimTracker.MilestoneEvent evnt = new SimTracker.MilestoneEvent(GameManager.instance.GetSceneNumber(), pos.x, pos.y, pos.z,
            "TP USED");
        SimTracker.SimTracker.instance.PushEvent(evnt);
        source.PlayOneShot(teleportAudio);
        if (exit == last)
            return;
        TeleportToExit(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (exit == last)
            last = null;
    }
    void TeleportToExit(Collider other)
    {
        last = transform;
		other.transform.position = exit.transform.position - plus;
		Quaternion originalRot = other.transform.rotation;    
		other.transform.rotation = originalRot * Quaternion.AngleAxis(180, Vector3.up);
    }
}
