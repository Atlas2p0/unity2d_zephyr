using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobStats : MonoBehaviour {

	public int maxHealth = 4;
	private int currentHealth;
	public Animator anim;
	public Transform skeletonTransform;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		Debug.Log(currentHealth);
		anim = GetComponent<Animator>();
		
	}
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
	IEnumerator Die()
	{
		Debug.Log("Enemy Died");
		
		//Disable enemy
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
		GetComponent<Skeleton_Behaviour>().enabled = false;
		skeletonTransform = GetComponent<Transform>();
		this.enabled = false;
		yield return new WaitForSeconds(1.2f);
		skeletonTransform.position = new Vector2(skeletonTransform.position.x, skeletonTransform.position.y - 0.5f);
		anim.enabled = false;
		
		
	}
	
	
}
