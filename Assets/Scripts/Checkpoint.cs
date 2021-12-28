/*
* @Author: Eyad205798
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	//If player collides with check point call level manager to update current checkpoint
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			FindObjectOfType<LevelManager>().CurrentCheckpoint = this.gameObject;
		}
	}
}
