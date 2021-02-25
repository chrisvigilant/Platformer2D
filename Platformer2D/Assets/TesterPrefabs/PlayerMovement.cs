using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	public Rigidbody2D rb;

	public float horizontal;
	public float jumpForce = 20f;
	public LayerMask groundLayers;
	public Transform feet;


	private int running, jumping, direction;

	Animator animator;
	
    private void Start()
    {
		animator = GetComponent<Animator>();
		running = Animator.StringToHash("Run");
		jumping = Animator.StringToHash("Jump");
		direction = Animator.StringToHash("Direction");
	}

    public void Update(){
		horizontal = Input.GetAxisRaw("Horizontal");

		if (Input.GetButtonDown("Jump") && isGrounded()){
			Jump();
		}
	}

	public void  FixedUpdate(){
		Vector2 movement = new Vector2(horizontal * movementSpeed, rb.velocity.y);

		rb.velocity = movement;

		if ((Input.GetKey("d")) || (Input.GetKey("a")))
		{
			animator.SetBool(running, true);
			animator.SetFloat(direction, horizontal);
		}

		if (rb.velocity.x == 0)
		{
			animator.SetBool(running, false);
		}
	}

	void Jump(){
		Vector2 movement = new Vector2(rb.velocity.x, jumpForce);

		rb.velocity = movement;

		animator.SetTrigger(jumping);
	}

	public bool isGrounded()
    {
		Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.05f, groundLayers);

        if (groundCheck)
        {
			return true;
        }  
		
		return false;
    }

}
