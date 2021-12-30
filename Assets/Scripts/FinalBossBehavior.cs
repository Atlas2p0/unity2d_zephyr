using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehavior : MonoBehaviour {

	[Header("Player Components")]
	[SerializeField] private GameObject playerObject;
	private Transform playerTransform;

	
	[Header("Behavior Variables")]
	[SerializeField] public float moveSpeed;
	
	private Animator anim;
	private bool facingRight; //used for flipping enemy
	private SpriteRenderer spriteRenderer; //used for flipping enemy
	public Rigidbody2D rb;

	[Header("Combat Variables")]
	[SerializeField] public float timer = 3f; //slash cooldown
	private float distance; //distance between enemy and player
	private float chaseDistance = 9f;

	[SerializeField] public int attackDamage = 1;
	[SerializeField] public float attackDistance; //Attack range, distance at which enemy can attack
	[SerializeField] public float attackRange = 0.5f;
	public LayerMask playerLayer;
	
	public Transform hitBox;
	private int attackMode;
	private bool cooling;
	private float initTimer; //used to initialize attack timer
	
	private bool isAttacking;
	[SerializeField] private float attackSpeed = 0.06f;
	private float canAttack1;
	
	private Random rand;
	int randomAttack;
	

	// Use this for initialization
	void Start () {
		
		playerTransform = playerObject.GetComponent<Transform>();
		anim = GetComponent<Animator>();
		facingRight = true;
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		initTimer = timer;
		canAttack1 = attackSpeed - 0.04f;
	}
	
	// Update is called once per frame
	void Update () {

		distance = Mathf.Abs(transform.position.x - playerTransform.position.x);
		if(distance > attackRange && (distance < chaseDistance))
		{
			
			Move();
			StopAttack();
			if(playerTransform.position.x > transform.position.x && !facingRight)
			{
				Flip();
			}
			else if(playerTransform.position.x < transform.position.x && facingRight)
			{
				Flip();
			}
		}
		else if(distance <= attackRange && cooling == false)
		{
			Attack();
		}
		if(distance > chaseDistance)
		{
			anim.SetBool("canWalk", false);
			StopAttack();
		}

		if(cooling)
		{
			Cooldown();
			if(randomAttack % 2 == 0)
				anim.SetBool("Attack1", false);
			else
				anim.SetBool("Attack2",false);
		}
		
	}
	public void Move()
	{
		anim.SetBool("canWalk", true);
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Final-Boss-Attack"))
		{
			Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
			transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
		}
	}
	public void Flip()
	{
		// transform.localScale = new Vector3(transform.localScale.x * - 1, transform.localScale.y, transform.localScale.z);
		transform.Rotate(0f,180f,0f);
		facingRight = !facingRight;
	}
	public void Attack()
	{
		timer = initTimer;

		if(attackSpeed <= canAttack1)
		{
			
			randomAttack = Random.Range(1,20);
			if(randomAttack % 2 == 0)
			{
				anim.SetBool("Attack1", true);
			}
			else
				anim.SetBool("Attack2", true);
			isAttacking = true;
			// BoxCollider2D instance = hitBox.GetComponent<BoxCollider2D>();//This is the hitbox of the attack
			Collider2D hitPlayer = Physics2D.OverlapCircle(hitBox.position, attackRange, playerLayer);
			// Damage player
			anim.SetBool("canWalk", false);//disable walking animation
			Debug.Log("hit");
			FindObjectOfType<PlayerStats>().TakeDamage(attackDamage);//call take damage
			canAttack1 = 0f;//start cooldown
		}
		else
		{
			canAttack1 += Time.deltaTime; //cooling down
		}

	}
	void Cooldown()
	{
		timer -= Time.deltaTime;
		if(timer <= 0 && cooling && isAttacking)
		{
			cooling = false;
			timer = initTimer;
		}
	}
	void OnDrawGizmosSelected()
	{
		if(hitBox == null)
			return;
		Gizmos.DrawWireSphere(hitBox.position, attackRange);
	}
	void StopAttack()
	{
		cooling = false;
		isAttacking = false;
		if(randomAttack % 2 == 0)
			anim.SetBool("Attack1", false);
		else
			anim.SetBool("Attack2", false);
	}
	public void TriggerCooling()
	{
		cooling = true;
	}
}
