using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePoint3 : MonoBehaviour {

	public Dialogue dialogueManager;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			string[] dialogue = {
				"Press Z to Attack Enemies with your Sword"
			};
			dialogueManager.SetSentences(dialogue);
			dialogueManager.StartCoroutine(dialogueManager.TypeDialogue());
		}
	}
}
