using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	[SerializeField] public Animator anim;
	public GameObject player;
	public GameObject CurrentCheckpoint;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void RespawnPlayer()
	{
		player.GetComponent<PlayerMovement>().enabled = true;
		FindObjectOfType<PlayerMovement>().transform.position = CurrentCheckpoint.transform.position;
		player.GetComponent<Animator>().enabled = true;
		player.GetComponent<Animator>().SetBool("isDead", false);
		player.GetComponent<PlayerStats>().enabled = true;
		player.GetComponent<BoxCollider2D>().enabled = true;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
