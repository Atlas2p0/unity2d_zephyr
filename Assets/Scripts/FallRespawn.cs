/*
*@Author: Eyad205798
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRespawn : MonoBehaviour {

	// Use this for initialization

	// if player falls off map respawn player at last checkpoint
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			// FindObjectOfType<PlayerMovement>().Death();
			FindObjectOfType<LevelManager>().RespawnPlayer();
		}
	}
}
