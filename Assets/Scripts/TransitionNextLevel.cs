using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionNextLevel : MonoBehaviour {
	private int thisScene;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			thisScene = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(thisScene + 1);
		}
	}
}
