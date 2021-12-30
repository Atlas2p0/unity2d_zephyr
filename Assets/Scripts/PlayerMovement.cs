/*
* @Author: Eyad205798
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("Components")]
	private Rigidbody2D rb;
	private Animator anim;

	[Header("Layer Masks")]
	[SerializeField] private LayerMask groundLayer;


	[Header("Movement Variables")]

	[SerializeField]private float movementAcceleration = 18f;
	[SerializeField]private float maxMoveSpeed = 6f;
	[SerializeField]private float groundLinearDrag = 7f; //Decelration
	private float horizontalDirection;
	private float verticalDirection;
	private bool isFacingRight;
	private bool changingDirection; // variable to check if the player changed direction (prevents slight slide when player changes directions)
	

	[Header("Jump Variables")]
	[SerializeField] private float jumpForce = 17.7f;
	[SerializeField] private float airLinearDrag = 5f; // air friction
	[SerializeField] private float jump_FallAcceleration = 8.8f;
	[SerializeField] private float lowJump_FallAcceleration = 5.6f;
	[SerializeField] private float hangTime = 0.1f; //This variable along with hangTimeCounter are used to implement the coyote jump mechanic (where a player can jump if they are slightly off the platform edge)
	[SerializeField] private AudioSource JumpSoundEffect ;
	private float hangTimeCounter; //This variable is used for decreasing hang time 
	[SerializeField] private float maxJumpHeight = 23f;

	[Header("Dash Variables")]
	[SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = .3f;
    [SerializeField] private float dashBufferLength = .1f;
    private float dashBufferCounter;
    private bool isDashing;
    private bool hasDashed;
	private bool dashedOnGround;
	private bool canDash;
	[SerializeField] private float dashMaxHorizontal;
	[SerializeField] private float dashMaxVertical;
	[SerializeField] private AudioSource DashSoundEffect ;

	[Header("Ground Collision Vars")]
	//[SerializeField] private float groundRayCastLength;
	private BoxCollider2D boxCollider;
	private bool grounded;

	

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>(); //Getting rb2D component from this game object
		boxCollider = GetComponent<BoxCollider2D>();
		anim = GetComponent<Animator>();
		isFacingRight = true;


	}
	
	// Update is called once per frame
	void Update () {
		
		//Movement and collisions
		if(dashBufferCounter > 0 && !hasDashed)
		{
			canDash = true;
			
		}
		else
			canDash = false;
		
		
		horizontalDirection = GetInput().x; //Setting horizontal direction variable to the input on the x axis once per frame by calling @GetInput function
		verticalDirection = GetInput().y; //Setting horizontal direction variable to the input on the x axis once per frame by calling @GetInput function

		if (Input.GetKeyDown(KeyCode.X))
		{
			dashBufferCounter = dashBufferLength;
		}
        else
			dashBufferCounter -= Time.deltaTime;
		if (canDash) StartCoroutine(Dash(horizontalDirection, verticalDirection));
		

		// all movement asid from dashing is under this condition as the dash function uses coroutines

		if(!isDashing)
		{
			moveCharacter(); //@MoveCharacter function (Defined below) gets called to help move the character
			fallAcceleration();
			if(horizontalDirection < 0f && isFacingRight)
			{
				Flip();
			}
			else if(horizontalDirection > 0f && !isFacingRight)
			{
				Flip();
			}

			//Player jump code

			if(Input.GetKeyDown(KeyCode.Space) && hangTimeCounter > 0f)
			{
				maxJumpHeight = rb.position.y + 23f;
				if(rb.position.y < maxJumpHeight)
				{
					Jump();
				}
				if(Input.GetKey(KeyCode.Space))
				{
					rb.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
				}
				
			}

			if(grounded && rb.velocity.x != 0)
			{
				applyGroundLinearDrag(); //@applyGroundLinearDrag function (Defined below)
				hangTimeCounter = hangTime; //reset hang time counter
			}
			else if(grounded)
			{
				hangTimeCounter = hangTime; // reset hang time counter
			}

			//else player in air

			else
			{
				//if character is in the air decrease hangTimeCounter and apply fall deceleration and air friction
				applyAirDrag(); //@applyAirDrag function (Defined below)
				fallAcceleration(); //@fallAcceleration function (Defined below)
				hangTimeCounter -= Time.deltaTime; //decrease hang time counter as time passes
			}
			
			CheckCollisions();
			fallAcceleration();

		}
		
		//Animation
		if(isDashing)
		{
			anim.SetBool("isDashing", true);
			anim.SetBool("isGrounded", false);
			anim.SetBool("isJumping", false);
			anim.SetBool("hasDashed", true);
			anim.SetBool("isFalling", false);
			if(verticalDirection != 0)
			{
				anim.SetBool("isVerticalDash", true);
			}
			else
				anim.SetBool("isVerticalDash", false);
		}
		else
		{
			anim.SetBool("isGrounded", grounded);
			anim.SetFloat("horizontalDirection", Mathf.Abs(horizontalDirection));
		}

		if(rb.velocity.y < 0f)
		{
			anim.SetBool("isJumping", false);
			anim.SetBool("isFalling", true);
		}


	}
	void FixedUpdate()
	{

		//this if-else condition sets the changing direction variable = true  right arrow was being pressed and then pressed left and vice versa
		if(rb.velocity.x > 0f && horizontalDirection < 0f)
		{
			changingDirection = true;
		}
		else if(rb.velocity.x < 0f && horizontalDirection > 0f)
		{
			changingDirection = true;
		}
		else
		{
			changingDirection = false;
		}

		
	}
	/*
	* creates vector 2 variable of the raw values of the virtual x and y axis and returns the vector
	*
	*/
	private Vector2 GetInput()
	{
		return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		
	}
	/*
	* This function uses the rigidbody component to add the force exerted on the body horizontally
	* If the body's movement speed is at the max movement speed specified it will clamp the velocity
	*/
	private void moveCharacter()
	{
		rb.AddForce(new Vector2(horizontalDirection,0f) * movementAcceleration);
		if(Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
		{
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
		}
	}
	/*
	* This function applies deceleration the the rigid body when the movement key is no longer being pressed
	*/
	private void applyGroundLinearDrag()
	{
		if(Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
		{
			rb.drag = groundLinearDrag;
		}
		else
		{
			rb.drag = 0;
		}
	}

	// The function maintains horizontal velocity as well as applies the jump force to the rb to make the player jump
	private void Jump()
	{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			
			// rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			if(Mathf.Abs(rb.velocity.y) >= maxJumpHeight)
			{
				rb.AddForce(Vector2.down * rb.gravityScale, ForceMode2D.Impulse);
			}
			
			hangTimeCounter = 0; //set the counter to 0 to make sure there is no double jump off platform edge

			//Note: ForceMode2D uses the rigidbody2D to apply a force to the body using its  mass

			//Animation
			anim.SetBool("isJumping", true);
			anim.SetBool("isFalling", false);
			anim.SetBool("isDashing", false);
			JumpSoundEffect.Play();
	}
	//This function is used to check for ground collision with player
	private void CheckCollisions()
	{
		/*
		*Ray casting method checks for ground by shooting a ray from the bottom of the player object body
		* if then the ray hits the layer mask called ground then we know the player the grounded value is set to true
		*/

		//set grounded to true if the player is touching the ground by using box casting

		grounded = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,0,Vector2.down, 0.1f, groundLayer); //Casting a box under the players body to check if the player is grounded

		if(grounded)
		{
			hangTimeCounter = hangTime; 
			anim.SetBool("isJumping", false);
			anim.SetBool("isFalling", false);
			anim.SetBool("isDashing", false);
			hasDashed = false;
			
		}
		

	}
	
	//This function applies air friction to the rigidbody
	private void applyAirDrag()
	{
		if(!grounded)
			rb.drag = airLinearDrag;
		else
			rb.drag = 0;
	}
	//This function is for air fall acceleration due to gravity
	private void fallAcceleration()
	{
		if(rb.velocity.y < 0)
		{
			rb.gravityScale = jump_FallAcceleration;
		}
		else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb.gravityScale = lowJump_FallAcceleration;
		}
		else
		{
			rb.gravityScale = 1f;
		}
	}
	void Flip()
	{
		isFacingRight = !isFacingRight;
		transform.Rotate(0f, 180f, 0f);
	}

	//Dash function applies dash force to the player on x and y axis based on buttons pressed as time passes until dash time is over
	IEnumerator Dash(float x, float y)
    {
		DashSoundEffect.Play();
        float dashStartTime = Time.time;
        hasDashed = true;
        isDashing = true;
		canDash = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.drag = 0f;
		if(grounded)
		{
			dashedOnGround = true;
		}

        Vector2 dir;
        if (x != 0f || y != 0f) dir = new Vector2(x,y);
        else
        {
            if (isFacingRight) dir = new Vector2(1f, 0f);
            else dir = new Vector2(-1f, 0f);
        }

        while (Time.time < dashStartTime + dashLength)
        {
            rb.velocity = dir.normalized * dashSpeed;
			
			//This if condition insures that if the player is holding down the dash key the character will not keep dashing
			if(Mathf.Abs(rb.velocity.y) >= dashMaxVertical || Mathf.Abs(rb.velocity.x) >= dashMaxHorizontal || Input.GetKeyDown(KeyCode.Space))
			{
				
				dashMaxHorizontal += rb.position.x;
				dashMaxVertical += rb.position.y;
				
				StopCoroutine(Dash(0, 0));
				rb.AddForce(Vector2.down * rb.gravityScale, ForceMode2D.Impulse);
				fallAcceleration();
			}
            yield return null;
        }

        isDashing = false;
    }
	
}
