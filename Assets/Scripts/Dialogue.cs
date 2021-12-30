using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour {

	public TextMeshProUGUI textDisplay;
	private string[] dialogueSentences;
	private int index = 0;
	public float typingSpeed;
	public GameObject continueButton;
	public GameObject dialogueBox;
	public Rigidbody2D playerRb;
	void Start()
	{
		dialogueBox.SetActive(false);
		continueButton.SetActive(false);
		textDisplay.text = "";
	}
	void Update()
	{

	}

	public IEnumerator TypeDialogue()
	{
		dialogueBox.SetActive(true);
		playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
		foreach(char letter in dialogueSentences[index].ToCharArray())
		{
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);

			if(textDisplay.text == dialogueSentences[index])
			{
				continueButton.SetActive(true);
			}
		}
	}

	public void SetSentences(string[] sentences)
	{
		this.dialogueSentences = sentences;
	}
	//
	public void NextSentence()
	{
		Debug.Log("here");
		continueButton.SetActive(false);
		if(index < dialogueSentences.Length - 1)
		{
			index++;
			textDisplay.text = "";
		}
		else
		{
			textDisplay.text = "";
			continueButton.SetActive(false);
			dialogueBox.SetActive(false);
			this.dialogueSentences = null;
			index = 0;
			playerRb.constraints = RigidbodyConstraints2D.None;
			playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;

		}
	}
}
