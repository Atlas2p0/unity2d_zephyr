using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePoint2 : MonoBehaviour {

	public Dialogue dialogueManager;
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			string[] dialogue = {
				"Press X to Dash. If you press arrow keys while pressing X at the same time you can control your dash direction, You can not dash again in the air, you have to touch the ground to be able to dash again"
			};
			dialogueManager.SetSentences(dialogue);
			dialogueManager.StartCoroutine(dialogueManager.TypeDialogue());
		}
	}
}
