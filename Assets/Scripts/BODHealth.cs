using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODHealth : MonoBehaviour {

	public int health = 6;

	public void TakeDamage(int damage)
	{
		health -= damage;
		GetComponent<Animator>().SetTrigger("hurtted");
		if (health <= 0)
		{
			Die();
		}
	}
	void Die()
    {
		GetComponent<Animator>().SetBool("isDead", true);
		Destroy(gameObject);
    }
}
