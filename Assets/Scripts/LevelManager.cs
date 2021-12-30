/*
* @Author: Eyad205798
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	[SerializeField] public Animator anim;
	public GameObject player;
	public GameObject CurrentCheckpoint;
	[SerializeField] private AudioSource ReviveSound;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//Re-enable all player child scripts and objects
	public void RespawnPlayer()
	{
		ReviveSound.Play();
		if(SceneManager.GetActiveScene().buildIndex == 4)
		{
			SceneManager.LoadScene(4);
		}
		else if(SceneManager.GetActiveScene().buildIndex == 6)
		{
			SceneManager.LoadScene(6);
		}
		else
		{
			player.GetComponent<PlayerMovement>().enabled = true;
			FindObjectOfType<PlayerMovement>().transform.position = CurrentCheckpoint.transform.position;
			player.GetComponent<Animator>().enabled = true;
			player.GetComponent<Animator>().SetBool("isDead", false);
			player.GetComponent<PlayerStats>().enabled = true;
			player.GetComponent<PlayerStats>().refillHP();
			player.GetComponent<BoxCollider2D>().enabled = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		
	}
}
