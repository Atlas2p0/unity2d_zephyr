using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletone : MonoBehaviour {
	public float maxSpeed = 3f;
	public bool isFacingRight = false;
	private Animator anim;


	public void Flip()
    {
		isFacingRight = !isFacingRight;
		transform.localScale = new Vector3(-(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
			anim.SetTrigger("ZephyrRange");
            // Damage Player
            Flip();
        }
        else
        {
			Flip();
        }
    }
    private void FixedUpdate()
    {
        if(isFacingRight == true)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            anim.SetFloat("speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            anim.SetFloat("speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        }
    }
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
