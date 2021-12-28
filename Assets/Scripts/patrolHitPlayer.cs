using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolHitPlayer : MonoBehaviour {

	// Use this for initialization
	public float attackSpeed = 1f;
	public float canAttack;
	void Start()
	{
		canAttack = attackSpeed;
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player" && (canAttack >= attackSpeed))
		{
			Debug.Log("Player Damaged");
			FindObjectOfType<PlayerStats>().TakeDamage(1);
			canAttack = 0;
		}
		else
		{
			canAttack = attackSpeed;
		}
		
	}
}
