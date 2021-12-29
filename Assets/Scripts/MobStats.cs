/*
* @Author: Eyad205798
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStats : MonoBehaviour {

	public int maxHealth = 2;
	private int currentHealth;
	public Animator anim;
	public Transform skeletonTransform; //Used for bringing enemy to ground after death
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
		
	}
	//This function plays take hit animation and if is dead plays dead and calls Die Coroutine
	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		Debug.Log(currentHealth);
		
		//Play hurt animation
		anim.SetTrigger("Hurt");

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
		GetComponent<Skeleton_Behaviour>().enabled = false;
		skeletonTransform = GetComponent<Transform>();
		this.enabled = false;
		//Wait for animation to finish
		yield return new WaitForSeconds(1.2f);

		//position trasformed by -0.5 on y axis so that enemy is directly on the ground after death
		skeletonTransform.position = new Vector2(skeletonTransform.position.x, skeletonTransform.position.y - 0.5f);
		anim.enabled = false;
	}
}
