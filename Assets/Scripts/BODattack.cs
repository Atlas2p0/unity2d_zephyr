using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODattack : MonoBehaviour {

	public int attackDamage = 20;

	public Vector3 attackOffset;
	public float attackRange = 1f;
	public LayerMask attackMask;
	float nextAttackTime = 0.5f;
	public float attackRate = 0.5f;

	public void Attack()
	{
		Debug.Log("Here We got");
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
			colInfo.GetComponent<PlayerStats>().TakeDamage(attackDamage);
		}
	}

	void OnDrawGizmosSelected()
	{
		Vector3 pos = transform.position;
		pos += transform.right * attackOffset.x;
		pos += transform.up * attackOffset.y;

		Gizmos.DrawWireSphere(pos, attackRange);
	}
}
