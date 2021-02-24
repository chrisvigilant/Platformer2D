using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	public Rigidbody2D rb;

	public float mx;
	public float jumpForce = 20f;
	public LayerMask groundLayers;
	public Transform feet;

	public int running, running_left, idle, jumping;

	Animator animator;
	
    private void Start()
    {
		animator = GetComponent<Animator>();
		running = Animator.StringToHash("Run");
		running_left = Animator.StringToHash("RunLeft");
		//idle = Animator.StringToHash("Idle");
		jumping = Animator.StringToHash("Jump");
	}

    public void Update(){
		mx = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump") && isGrounded()){
			Jump();
		}
	}

	public void  FixedUpdate(){
		Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

		rb.velocity = movement;

		if (Input.GetKey("d")){
			animator.SetBool(running, true);
		}
		else if (Input.GetKey("a"))
		{
			animator.SetBool(running_left, true);
		}

		if (rb.velocity.x == 0)
		{
			animator.SetBool(running, false);
			animator.SetBool(running_left, false);
		}
	}

	void Jump(){
		Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

		rb.velocity = movement;

		animator.SetTrigger(jumping);
	}

	public bool isGrounded(){
		Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.5f, groundLayers);

		if (groundCheck != null){
			return true;
		}

		return false;
	}
}
