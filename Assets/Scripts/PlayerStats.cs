using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
	public int maxHealth = 3;
	private int currentHealth;
	public Animator anim;
	public Transform playerTransform;
	[SerializeField] public GameObject levelManager;

	[Header("Hearts bar")]
	public int numOfHearts;
	public Image[] hearts;
	public Sprite fullHeart;
	public Sprite emptyHeart;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(currentHealth > numOfHearts)
        {
			currentHealth = numOfHearts;
        }

		for(int i = 0; i < hearts.Length; i++)
        {
			if(i < currentHealth)
            {
				hearts[i].sprite = fullHeart;
            }
            else
            {
				hearts[i].sprite = emptyHeart;
            }
			if(i < numOfHearts)
            {
				hearts[i].enabled = true;
            }
            else
            {
				hearts[i].enabled = false;
            }
        }
	}
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		Debug.Log(currentHealth);
		
		//Play hurt animation
		anim.SetTrigger("Hurt");
		if (currentHealth <= 0)
		{
			anim.SetBool("isDead",true);
			StartCoroutine(Die());
		}
	}

	IEnumerator Die()
	{
		Debug.Log("Player Dead");
		
		//Disable enemy
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
		GetComponent<PlayerMovement>().enabled = false;
		playerTransform = GetComponent<Transform>();
		this.enabled = false;
		yield return new WaitForSeconds(1.2f);
		playerTransform.position = new Vector2(playerTransform.position.x, playerTransform.position.y - 0.5f);
		anim.enabled = false;
		//renable object
		currentHealth = maxHealth;
		levelManager.GetComponent<LevelManager>().RespawnPlayer();
		
	
		
		
	}
	
}
