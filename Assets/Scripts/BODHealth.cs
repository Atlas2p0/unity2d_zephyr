using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BODHealth : MonoBehaviour {
	public Animator anim;
	public int health = 6;
	public GameObject doorToNextLevel;
	void Start()
	{
		anim = GetComponent<Animator>();
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
		GetComponent<Animator>().SetTrigger("hurtted");
		if (health <= 0)
		{
			StartCoroutine(Die());
		}
	}
	IEnumerator Die()
    {
		anim.SetBool("isDead", true);
		yield return new WaitForSeconds(1.2f);
		Destroy(gameObject);
		Instantiate(doorToNextLevel, new Vector2(22.65f, -2.1f), Quaternion.identity);
    }
}
