using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead_Behaviour : MonoBehaviour
{
	public Transform rayCast;
	public LayerMask rayCastMask;
	public float attackDistance;
    public float rayCastLength;
    public float moveSpeed;
    public float timer; //slash cooldown
	private RaycastHit2D hit;

	private GameObject target;
	private Animator anim;
	private float distance; //distance between enemy and player
	private bool attackMode;
	private bool inRange;
	private bool cooling;
	private float initTimer;
	private bool facingRight;
	private SpriteRenderer spriteRenderer;


	void Awake()
	{
		initTimer = timer; //store initial value of timer
		anim = GetComponent<Animator>();
		facingRight = false;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Update is called once per frame
	void Update()
	{

		if (inRange)
		{
			Vector2 playerPosition = new Vector2(target.transform.position.x, 0f);
			if (playerPosition.x > transform.position.x)
			{
				hit = Physics2D.Raycast(rayCast.position, Vector2.right, rayCastLength, rayCastMask);
				if (!facingRight)
					Flip();
			}
			else if (playerPosition.x < transform.position.x)
			{
				hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
				if (facingRight)
					Flip();

			}
			RaycastDebugger();
		}

		if (hit.collider != null)
		{
			SkeletonLogic();
		}
		else if (hit.collider == null)
		{
			inRange = false;
		}
		if (inRange == false)
		{
			anim.SetBool("canWalk", false);
			StopAttack();
		}



	}
	void OnTriggerEnter2D(Collider2D trig)
	{
		if (trig.gameObject.tag == "Player")
		{
			target = trig.gameObject;
			inRange = true;
		}
	}
	void SkeletonLogic()
	{
		distance = Vector2.Distance(transform.position, target.transform.position);
		if (distance > attackDistance)
		{
			Move();
			StopAttack();
		}
		else if (distance <= attackDistance && cooling == false)
		{
			Attack();
		}
		if (cooling)
		{
			Cooldown();
			anim.SetBool("attack", false);
		}
	}
	void Move()
	{
		anim.SetBool("canWalk", true);

		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Undead_Exec_attack"))
		{
			Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

		}
	}
	void Attack()
	{
		timer = initTimer;
		attackMode = true;

		anim.SetBool("canWalk", false);
		anim.SetBool("attack", true);
	}
	void Cooldown()
	{
		timer -= Time.deltaTime;
		if (timer <= 0 && cooling && attackMode)
		{
			cooling = false;
			timer = initTimer;
		}
	}
	void StopAttack()
	{
		cooling = false;
		attackMode = false;
		anim.SetBool("attack", false);
	}
	void RaycastDebugger()
	{
		if (distance > attackDistance)
		{
			Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);

		}
		if (distance < attackDistance)
		{
			Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);

		}
	}

	public void TriggerCooling()
	{
		cooling = true;
	}
	private void Flip()
	{
		if (!facingRight)
			spriteRenderer.flipX = true;
		else if (facingRight)
			spriteRenderer.flipX = false;

		facingRight = !facingRight;
	}
}

