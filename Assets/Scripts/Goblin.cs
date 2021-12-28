using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour {


	// Use this for initialization
	[Header("Game Objects")]
	public Transform rayCast;
	public LayerMask rayCastMask;

	[Header("Behavior Variables")]
	[SerializeField] public float rayCastLength;
	[SerializeField] public float moveSpeed;
	[SerializeField] public float timer; //slash cooldown

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

	[Header("Combat Variables")]
	[SerializeField] public int attackDamage = 1;
	[SerializeField] public float attackDistance;
	[SerializeField] public float attackRange = 0.5f;
	public LayerMask playerLayer;
	public Transform hitBox;
	[SerializeField] private float attackSpeed = 0.06f;
	private float canAttack;




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




	}
	void FixedUpdate()
	{
		if (hit.collider != null)
		{
			GoblinLogic();
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
	void GoblinLogic()
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

		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Goblinattack"))
		{
			Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

		}
	}
	//Not perfect but works
	void Attack()
	{
		timer = initTimer;
		attackMode = true;
		anim.SetBool("attack", true);
		if (attackSpeed <= canAttack)
		{
			BoxCollider2D instance = hitBox.GetComponent<BoxCollider2D>();
			Collider2D hitPlayer = Physics2D.OverlapCircle(hitBox.position, attackRange, playerLayer);
			// Damage player
			anim.SetBool("canWalk", false);

			Debug.Log("Player hit");
			hitPlayer.GetComponent<PlayerStats>().TakeDamage(attackDamage);
			canAttack = 0f;
		}
		else
		{
			canAttack += Time.deltaTime;
		}
	}
	void OnDrawGizmosSelected()
	{
		if (hitBox == null)
			return;
		Gizmos.DrawWireSphere(hitBox.position, attackRange);
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
		if (facingRight)

			spriteRenderer.flipX = true;


		else if (!facingRight)

			spriteRenderer.flipX = false;


		// hitBox.position.x = -1;
		facingRight = !facingRight;
	}
}
