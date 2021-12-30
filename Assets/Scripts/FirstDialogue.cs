using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDialogue : MonoBehaviour {
	public Dialogue dialogueManager;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			string[] dialogue = {
				"Press space to jump, and press arrow keys mid air to control your character"
			};
			dialogueManager.SetSentences(dialogue);
			dialogueManager.StartCoroutine(dialogueManager.TypeDialogue());
		}
	}
}
