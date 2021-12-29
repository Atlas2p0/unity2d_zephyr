/*
* @Author: Eyad205798
* This script uses the same functions as MobStats.cs and full commentated code can be found @MobStats.cs
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour {
	public int maxHealth = 1;
	public Animator anim;
	public Transform playerTransform;
	[SerializeField] public GameObject levelManager;
	
	[Header("UI Components")]
	private int currentHealth;
	public int numOfHearts;
	public Image[] hearts;
	public Sprite fullHeart;
	public Sprite emptyHeart;
	public TextMeshProUGUI textScore;
	public int coinsCollected;

	// Use this for initialization
	void Start () {

		currentHealth = maxHealth;
		numOfHearts = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
		coinsCollected = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//Displaying hearts full or empty based on current health
		if(currentHealth == 3)
		{
			hearts[0].sprite = fullHeart;
			hearts[1].sprite = fullHeart;
			hearts[2].sprite = fullHeart;
		}
		else if(currentHealth == 2)
		{
			hearts[0].sprite = fullHeart;
			hearts[1].sprite = fullHeart;
			hearts[2].sprite = emptyHeart;
		}
		else if(currentHealth == 1)
		{
			hearts[0].sprite = fullHeart;
			hearts[1].sprite = emptyHeart;
			hearts[2].sprite = emptyHeart;
		}
	}
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		
		
		//Play hurt animation
		anim.SetTrigger("Hurt");

		if(currentHealth <= 0)
		{
			anim.SetBool("isDead",true);
			StartCoroutine(Die());
		}
	}
	IEnumerator Die()
	{
		//Display hearts all empty this has to be done here as it isn't possible in update
		//since the function Die is a coroutine
		hearts[0].sprite = emptyHeart;
		hearts[1].sprite = emptyHeart;
		hearts[2].sprite = emptyHeart;
		//Disable enemy
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
		GetComponent<PlayerMovement>().enabled = false;
		playerTransform = GetComponent<Transform>();
		this.enabled = false;
		yield return new WaitForSeconds(1.2f);
		playerTransform.position = new Vector2(playerTransform.position.x, playerTransform.position.y - 0.5f);
		anim.enabled = false;
		//renable object
		yield return new WaitForSeconds(1.2f);
		FindObjectOfType<LevelManager>().RespawnPlayer();
	}
	public void refillHP()
	{
		currentHealth = maxHealth;
	}
	public void incrementHealth()
	{
		if(currentHealth != maxHealth)
			currentHealth++;
	}
	public void incrementTextScore()
	{
		coinsCollected += 1;
		textScore.text = "X" + coinsCollected.ToString();
	}
}
