/*
 * @Author: Eyad205798
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
	
	[Header("Objects needed")]
	public Animator anim;
	public LayerMask enemyLayer; //To check if player slash hit enemy
	public LayerMask patrollingEnemyLayer; //To check if player slash hit patrolling enemy
	public LayerMask finalBossLayer; //To check if player slash hit patrolling enemy

	[Header("Combat Vars")]
	public Transform attackPoint; //Position of the attack object attached to the player

	public float attackRange = 0.5f; //Attack area of effect

	public int attackDamage = 1; //Damage dealt
	public float attackRate = 0.5f; //Speed at which player can attack 
	float nextAttackTime = 0f; //Used to add cooldown on player attacks

	void Start()
	{
		anim = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () 
	{
		//This if condition checks if the player can attack again after attacking
		
		if(Time.time >= nextAttackTime)
		{
			if(Input.GetKeyDown(KeyCode.Z))
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate; //reset cooldown
			}

		}
	}
	void Attack ()
	{
		//Play attack animation
		anim.SetTrigger("Attack");

		//Detect enemies in range of attack
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer); //use position of attack and attack range and detect if enemy has been hit

		
		// Damage enemies
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<MobStats>().TakeDamage(attackDamage); //call take damage function from mobstats
			// enemy.GetComponent<patrollingEnemyStats>().TakeDamage(attackDamage); //call take damage function from mobstats	
			
		}
		Collider2D[] hitPatrollingEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, patrollingEnemyLayer);
		foreach(Collider2D enem in hitPatrollingEnemies)
		{
			Debug.Log("here");
			patrollingEnemyStats patrollingEnemy = FindObjectOfType<patrollingEnemyStats>();
			patrollingEnemy.enemyTakeDamage(attackDamage);
		}
		Collider2D hitFinalBoss = Physics2D.OverlapCircle(attackPoint.position, attackRange, finalBossLayer);
		if(hitFinalBoss != null)
			FindObjectOfType<FinalBossStats>().TakeDamage(1);
	}	
	//Draw attack range for debugging
	void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
