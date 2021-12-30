/*
* @Author: Eyad205798
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Behaviour : MonoBehaviour {

	// Use this for initialization
	[Header("Game Objects")]
	public Transform rayCast; //used for chasing player
	public LayerMask rayCastMask; 
	private GameObject target; // Player object
	[SerializeField] public GameObject skeletonSword;
	// public Transform skeletonTransform;
	
	[Header("Behavior Variables")]
	[SerializeField] public float rayCastLength; //Length at which enemy keeps following player
	[SerializeField] public float moveSpeed;

	private RaycastHit2D hit;
	
	private Animator anim;
	private bool facingRight; //used for flipping enemy
	private SpriteRenderer spriteRenderer; //used for flipping enemy

	[Header("Combat Variables")]
	[SerializeField] public float timer; //slash cooldown
	private float distance; //distance between enemy and player
	private bool attackMode;
	private bool inRange; //Check if player is in chase range
	private bool cooling;
	private float initTimer; //used to initialize attack timer
	[SerializeField] public int attackDamage = 1;
	[SerializeField] public float attackDistance; //Attack range, distance at which enemy can attack
	[SerializeField] public float attackRange = 0.5f;
	public LayerMask playerLayer;
	public Transform hitBox;
	[SerializeField] private float attackSpeed = 0.06f;
	private float canAttack;

	[SerializeField] private AudioSource AttackSoundEffect;
	



	void Awake()
	{
		initTimer = timer; //store initial value of timer
		anim = GetComponent<Animator>();
		facingRight = false;
		spriteRenderer = GetComponent<SpriteRenderer>();
		canAttack = attackSpeed - 0.2f;
	}
	// Update is called once per frame
	void Update () {

		//This if condition handles player detection and enemy sprite flipping if player is behind enemy
		if(inRange)
		{
			Vector2 playerPosition = new Vector2(target.transform.position.x, 0f);
			if(playerPosition.x > transform.position.x)
			{	
				hit = Physics2D.Raycast(rayCast.position, Vector2.right, rayCastLength, rayCastMask);
				if(!facingRight)
					Flip();
			}
			else if(playerPosition.x < transform.position.x)
			{
				hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
				if(facingRight)
					Flip();
				
			}
			RaycastDebugger();
		}

		

		
	}
	void FixedUpdate()
	{
		if(hit.collider != null)
		{
			SkeletonLogic();
		}
		else if(hit.collider == null)
		{
			inRange = false; 
		}
		if(inRange == false)
		{
			anim.SetBool("canWalk", false);
			StopAttack();
		}

	}
	void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.gameObject.tag == "Player")
		{
			target = trig.gameObject;
			inRange = true;
		}
	}
	//This function handles player chasing, attacking and ceasing attack
	void SkeletonLogic()
	{
		//Calculate distance between enemy and player
		distance = Vector2.Distance(transform.position, target.transform.position);

		if(distance > attackDistance)
		{
			//Move towards player if not in attack distance
			Move();
			//Disable attack animation
			StopAttack();
		}

		//Attack if not cooling down and player is in attack distance
		else if(distance <= attackDistance && cooling == false)
		{
			Attack();
		}

		if(cooling)
		{
			Cooldown();
			anim.SetBool("attack", false);
		}
	}
	void Move()
	{
		anim.SetBool("canWalk", true);

		//If enemy is not attacking move towards player
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Skeleton_attack"))
		{
			Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

		}
	}
	//Not perfect but works
	void Attack()
	{
		timer = initTimer;
		AttackSoundEffect.Play();


		//This if else condition makes sure that attack is only called once since this function is being called in update
		//If this is not being done the player gets damaged more than once since attack gets called alot in one attack

		if(attackSpeed <= canAttack)
		{
			attackMode = true;
			anim.SetBool("attack", true);
			// BoxCollider2D instance = hitBox.GetComponent<BoxCollider2D>();//This is the hitbox of the attack
			Collider2D hitPlayer = Physics2D.OverlapCircle(hitBox.position, attackRange, playerLayer);
			// Damage player
			anim.SetBool("canWalk", false);//disable walking animation
			if(hitPlayer != null)
				FindObjectOfType<PlayerStats>().TakeDamage(attackDamage);//call take damage
			canAttack = 0f;//start cooldown
		}
		else
		{
			canAttack += Time.deltaTime; //cooling down
		}
	}

	//Drawing hitbox for attack for debugging
	void OnDrawGizmosSelected()
	{
		if(hitBox == null)
			return;
		Gizmos.DrawWireSphere(hitBox.position, attackRange);
	}

	//Attack animation cooldown
	void Cooldown()
	{
		timer -= Time.deltaTime;
		if(timer <= 0 && cooling && attackMode)
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

	//Drawing Raycast to debug enemy chase
	void RaycastDebugger()
	{
		if(distance > attackDistance)
		{
			Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
			
		}
		if(distance < attackDistance)
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
		
		//flipping the sprite itself was the only way to get the enemy to flip for some reason

		if(!facingRight)
		{
			spriteRenderer.flipX = true;	
		}
		else if(facingRight)
		{
			spriteRenderer.flipX = false;
		}
		// hitBox.position.x = -1;
		facingRight = !facingRight;
	}
}
