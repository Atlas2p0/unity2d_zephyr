using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollected : MonoBehaviour {

	public GameObject collectible;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerStats>().incrementTextScore();
			collectible.GetComponent<BoxCollider2D>().enabled = false;
			Destroy(collectible);
			this.enabled = false;
		}
	}
}
