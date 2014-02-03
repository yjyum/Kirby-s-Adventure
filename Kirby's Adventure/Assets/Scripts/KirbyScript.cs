using UnityEngine;
using System.Collections;

public class KirbyScript : MonoBehaviour {

	public float		walkSpeed = 2.5f;
	public float 		dashSpeed = 3f;
	public float 		slideSpeed = 1.7f;
	public float		flySpeed = 2.3f;
	public float		fallSpeed = 1.7f;
	public float   	 	horSpeed = 2.5f;
	public float		jumpSpeed = 6.5f;
	public float		jumpAcc = 0.5f;

	public float 		kirbyScale = 2.5f;
	public float 		normalGravity = 1f;
	public float 		flyGravity = 0.2f;

	public static float kirbyAttackDis = 1.5f;

	
	public Vector2 		vel;
	
	public bool			grounded = true;

	private Animator 	animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		if (SingletonScript.Instance) {
			SingletonScript.Instance.kirby_animator = animator;
		}
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
		//				 Pass through a doorfly 
		if (Input.GetKey (KeyCode.UpArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")
		    && !changeScene.atDoor) {
			rigidbody2D.gravityScale = flyGravity;
			vel.y = vertical * flySpeed;		
			animator.SetBool ("withAir", true);
			animator.speed = 3;
		} else {
			animator.speed = 1;
		}
		
		// "DOWN" command: Duck when standing still
		//				   Swallow current item in Kirby's mouth 
		if (Input.GetKeyDown (KeyCode.DownArrow) 
		    && animator.GetBool("withEnemy")) { // swallow
			animator.SetBool("swallow", true);
		}
		if (Input.GetKey (KeyCode.DownArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_inhale")) { // duck
			vel.x = 0;
			animator.SetBool("duck", true);
		} else {
			animator.SetBool("duck", false);
		}
		if (Input.GetKeyUp (KeyCode.DownArrow)) { // swallow
			animator.SetBool("swallow", false);
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
		if (Input.GetKeyUp (KeyCode.Z)
		    && animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_jump")) {
			if (!grounded && vel.y > 0) {
				vel.y = 0f;
			}
		}
		
		// "X" command: inhale when Kirby doesn't have a copy ability
		//              Shoot out a star when an item is in Kirby's mouth TODO
		//              Use power when Kirby has a copy ability TODO
		//              Puff attack while floating
		if (Input.GetKeyDown (KeyCode.X)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")) {
			if (animator.GetBool("withEnemy")) { // shoot star
				animator.SetBool("withEnemy", false);
				shootStar();
			}
			if (animator.GetBool("withAir")) { // puff attack
				if (EnemyInDis(kirbyAttackDis)) {
					Destroy(SingletonScript.Instance.current_enemy);
				}
				animator.SetBool("withAir", false);
			} else {
				animator.SetBool("inhale", true);
			}
		} 

		if (Input.GetKey (KeyCode.X) && animator.GetBool("inhale")) { // inhale
			if (EnemyInDis(kirbyAttackDis)) {
				Debug.Log (this + "inhale enemy");
				Destroy(SingletonScript.Instance.current_enemy);
				animator.SetBool ("withEnemy", true);
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
<<<<<<< HEAD
			vel.x = transform.localScale.x * slideSpeed;
=======
			vel.x = transform.localScale.x * dashSpeed;
			if (SingletonScript.Instance.current_enemy) {
				if ((SingletonScript.Instance.current_enemy.transform.position - transform.position).magnitude <= 2) {
					Destroy(SingletonScript.Instance.current_enemy);
				}
			}
>>>>>>> f4af94c88ccd198749c035e050c49c20991992ab
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
			Debug.Log(this + "collision kirby");
			Application.LoadLevel ("Vegetable Valley 1");
		}
	}


	// helper func
	GameObject EnemyInDis(float distance) {
		if (SingletonScript.Instance) {
			if (SingletonScript.Instance.current_enemy) {
				if ((SingletonScript.Instance.current_enemy.transform.position 
				     - transform.position).magnitude <= distance) {
					return SingletonScript.Instance.current_enemy;
				}
			}
		}
		return null;
	}

	void shootStar() {
		GameObject star = GameObject.Instantiate (GameObject.Find("shootStar"), 
		                                          transform.position, 
		                                          Quaternion.identity) as GameObject;
		star.rigidbody2D.AddForce (transform.forward * 2);

	}
}
