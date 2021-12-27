using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
	
	public Animator anim;
	public Transform attackPoint;

	public float attackRange = 0.5f;

	public LayerMask enemyLayer;
	public int attackDamage = 1;
	public float attackRate = 1;
	float nextAttackTime = 0f;

	void Start()
	{
		anim = GetComponent<Animator>();
	}
	// Update is called once per frame
	void Update () {
		if(Time.time >= nextAttackTime)
		{
			if(Input.GetKeyDown(KeyCode.Z))
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate;

			}

		}
	}
	void Attack ()
	{
		//Play an attack animation
		anim.SetTrigger("Attack");

		//Detect enemies in range of attack

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
		
		// Damage enemies
		foreach(Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<MobStats>().TakeDamage(attackDamage);
		}

	}
	void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
			return;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}
