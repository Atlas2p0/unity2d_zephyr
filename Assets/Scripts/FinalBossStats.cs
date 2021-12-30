using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossStats : MonoBehaviour {

	public int maxHealth = 15;
	private int currentHealth;
	public Animator anim;
	public Transform finalBossTransform; //Used for bringing enemy to ground after death
	// Use this for initialization
	public GameObject Skeletons;
	private bool canSpawn;
	[SerializeField] private GameObject HealthBar;
	private Transform bar;
	[SerializeField] private Transform playerTransform;
	private int index;
	void Start () {
		currentHealth = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
		canSpawn = true;
		index = maxHealth;
		
	}
	//This function plays take hit animation and if is dead plays dead and calls Die Coroutine
	void Update()
	{
		if(currentHealth == 11 && canSpawn)
		{
			SpawnSkeletons();
		}
		
		Debug.Log((float) currentHealth / maxHealth);
		
	}
	public void TakeDamage(int damage)
	{

		HealthBar.GetComponent<FinalBossHealthBar>().setSize((float) currentHealth / maxHealth);

		currentHealth -= damage;
		Debug.Log(currentHealth);
		
		//Play hurt animation
		anim.SetTrigger("Hurt");
		Debug.Log("Final Boss Hit");

		if(currentHealth <= 0)
		{
			anim.SetBool("isDead",true);
			StartCoroutine(Die());
		}
	}
	//This Coroutine was used so i can wait for the death animation to finish before disabling the animator component
	IEnumerator Die()
	{
		Debug.Log("Enemy Died");
		
		//Disable enemy
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		GetComponent<FinalBossBehavior>().enabled = false;
		finalBossTransform = GetComponent<Transform>();
		this.enabled = false;
		//Wait for animation to finish
		yield return new WaitForSeconds(2.7f);

		//position trasformed by -0.5 on y axis so that enemy is directly on the ground after death
		finalBossTransform.position = new Vector2(finalBossTransform.position.x, finalBossTransform.position.y - 0.5f);
		anim.enabled = false;
	}
	private void SpawnSkeletons()
	{
		
		//Spawn enemy monsters infront of player
		Vector2 SpawnPosition = new Vector2(playerTransform.position.x + 6f, playerTransform.position.y);
		Vector2 SpawnPosition2 = new Vector2(playerTransform.position.x + 5f, playerTransform.position.y);
		Instantiate(Skeletons, SpawnPosition, Quaternion.identity);
		Instantiate(Skeletons, SpawnPosition2, Quaternion.identity);
		canSpawn = false;
	}
}
