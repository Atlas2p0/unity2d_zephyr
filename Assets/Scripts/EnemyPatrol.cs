using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {

	[HideInInspector]
	public bool mustPatrol;
	private bool mustFlip;
	private Rigidbody2D rb;
	
	[SerializeField] public float moveSpeed;
	public Transform groundCheckPos;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	private bool isFacingLeft;
	public Collider2D bodyCollider;
	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		mustPatrol = true;
		isFacingLeft = true;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(mustPatrol);
		if(mustPatrol)
		{
			Patrol();
		}
		
	}
	void FixedUpdate()
	{
		if(mustPatrol)
		{
			mustFlip = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
		}
	}
	void Patrol()
	{
		if(mustFlip || bodyCollider.IsTouchingLayers(wallLayer))
		{
			Flip();
		}
		Debug.Log("Here");
		rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);

	}
	void Flip()
	{
		mustPatrol = false;
		transform.localScale = new Vector2 (transform.localScale.x * -1,  transform.localScale.y);
		moveSpeed *= -1;
		mustPatrol = true;

	}
}
