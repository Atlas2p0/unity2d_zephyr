/*
* @Author: Eyad205798
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrollingEnemyStats : MonoBehaviour {

	public int maxHealth = 2;
	private int currentHealth;
	public Animator anim;
	public Transform enemyTransform; //Used for bringing enemy to ground after death
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
		
	}
	//This function plays take hit animation and if is dead plays dead and calls Die Coroutine
	public void enemyTakeDamage(int damage)
	{
		Debug.Log("Damage taken");
		currentHealth -= damage;
		Debug.Log(currentHealth);
		
		//Play hurt animation
		anim.SetTrigger("Hurt");

		if(currentHealth <= 0)
		{
			StartCoroutine(Die());
			anim.SetBool("isDead",true);
		}
	}
	//This Coroutine was used so i can wait for the death animation to finish before disabling the animator component
	IEnumerator Die()
	{
		
		//Disable enemy
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		GetComponent<EnemyPatrol>().moveSpeed = 0;
		GetComponent<EnemyPatrol>().enabled = false;
		
		enemyTransform = GetComponent<Transform>();
		this.enabled = false;
		
		//Wait for animation to finish
		yield return new WaitForSeconds(0.9f);

		//position trasformed by -0.5 on y axis so that enemy is directly on the ground after death
		enemyTransform.position = new Vector2(enemyTransform.position.x, enemyTransform.position.y - 0.91f);
		anim.enabled = false;
	}
}
