﻿using UnityEngine;
using System.Collections;

public class ParticlePlaybackTime : MonoBehaviour {

	public float playBackSpeed;

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem>().playbackSpeed = playBackSpeed;
	}
}
