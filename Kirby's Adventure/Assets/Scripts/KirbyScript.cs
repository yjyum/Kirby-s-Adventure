using UnityEngine;
using System.Collections;

public class KirbyScript : MonoBehaviour {

	public float		walkSpeed = 6f;
	public float 		dashSpeed = 7.2f;
	public float		flySpeed = 4.4f;
	public float		fallSpeed = 3.2f;
	public float   	 	horSpeed = 6f;
	public float		jumpSpeed = 8f;
	public float		jumpAcc = 13f;

	public float 		kirbyScale = 2.5f;
	public float 		normalGravity = 2.8f;
	public float 		flyGravity = 0.4f;
	
	public Vector2 		vel;
	
	public bool			grounded = true;

	private Animator 	animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		SingletonScript.Instance.kirby_animator = animator;
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		
		Vector2 vel = rigidbody2D.velocity;

		if (animator.GetBool("withAir")) {
			rigidbody2D.gravityScale = flyGravity;
			horSpeed = flySpeed;
			vel.y = -fallSpeed; 
		}
		else {
			rigidbody2D.gravityScale = normalGravity;
			horSpeed = walkSpeed;
		}
		
		
		
		// "LEFT" & "RIGHT" command: walk left and right
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")) {
			vel.x = horizontal * horSpeed;
		}
		if (vel.x > 0) {
			transform.localScale = new Vector3(kirbyScale*1f,kirbyScale*1f,1f);
			animator.SetBool("Direction", true);
		} else if (vel.x < 0) {
			transform.localScale = new Vector3(-1f*kirbyScale,1f*kirbyScale,-1f);
			animator.SetBool("Direction", true);
		} else{
			animator.SetBool("Direction", false);
			animator.SetBool("dash", false);
		}
		
		// "UP" command: Take in air and float in the air
		//				 Flap arms to fly higher while floating
		//				 Pass through a doorfly TODO
		if (Input.GetKey (KeyCode.UpArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")) {
			rigidbody2D.gravityScale = flyGravity;
			vel.y = vertical * flySpeed;		
			animator.SetBool ("withAir", true);
			animator.speed = 3;
		} else {
			animator.speed = 1;
		}
		
		// "DOWN" command: Duck when standing still
		//				   Swallow current item in Kirby's mouth TODO
		if (Input.GetKey (KeyCode.DownArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")) {
			vel.x = 0;
			animator.SetBool("duck", true);
		} else {
			animator.SetBool("duck", false);
		}
		
		// "Z" command: jump
		if (Input.GetKeyDown (KeyCode.Z) 
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_withAir")) {
			if (grounded) {
				vel.y = jumpSpeed;
				animator.speed = 0;
				animator.SetBool("jump", true);
			}
		}
		if (Input.GetKey (KeyCode.Z)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")) {
			if (!grounded && vel.y > 0) {
				vel.y += jumpAcc * Time.deltaTime;
				if (vel.y > 0) {
					animator.speed = 0;
				} else {
					animator.speed = 1;
				}
				animator.SetBool("jump", false);
			}
		}
		
		// "X" command: inhale when Kirby doesn't have a copy ability
		//              Shoot out a star when an item is in Kirby's mouth TODO
		//              Use power when Kirby has a copy ability TODO
		//              Puff attack while floating
		if (Input.GetKeyDown (KeyCode.X)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")) {
			if (animator.GetBool("withAir")) {
				if (SingletonScript.Instance.current_enemy) {
					if ((SingletonScript.Instance.current_enemy.transform.position - transform.position).magnitude <= 2) {
						Destroy(SingletonScript.Instance.current_enemy);
					}
				}
				animator.SetBool("withAir", false);
			} else {
				animator.SetBool("inhale", true);
			}
		} 
		if (Input.GetKeyUp (KeyCode.X)) {
			animator.SetBool("inhale", false);
		}
		
		// "DOUBLE LEFT" & "DOUBLE RIGHT" command: dash
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_walk")) {
			if ((Input.GetKeyDown(KeyCode.RightArrow) && vel.x > 0)
			    ||(Input.GetKeyDown(KeyCode.LeftArrow) && vel.x < 0)) {
				vel.x = horizontal * dashSpeed;
				animator.SetBool("dash", true);
			}
		}
		
		// "DOWN" + "Z"/"X" command: slide attack
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")) {
			animator.SetBool("slide", false);
			if (Input.GetKeyDown(KeyCode.Z)
			    ||Input.GetKeyDown(KeyCode.X)) {
				animator.SetBool("slide", true);
			}
		}
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_slideAttack")) {
			vel.x = transform.localScale.x * dashSpeed;
		}
		
		rigidbody2D.velocity = vel;
	}

	void FixedUpdate() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
	}
	void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		// kirby died
		if (col.gameObject.tag.Equals("Enemy")) {
			Debug.Log("collision kirby");
			Application.LoadLevel ("Vegetable Valley 1");
		}
	}
}
