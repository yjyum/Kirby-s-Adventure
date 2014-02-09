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
	public float		inhaleInterval = 0f;

	public GameObject	starPrefab;
	public GameObject	puffPrefab;
	public GameObject	beamPrefab;
	public GameObject	inhalePrefab;
	
	public Vector2 		vel;
	
	public bool			grounded = true;
	public int 			cameraRange = 6;
	// animator.powerType; 
	// 0:inhale	 1: beam 	2: spark	2:fire

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
		} else {
			rigidbody2D.gravityScale = normalGravity;
			horSpeed = walkSpeed;
		}

		LeftRightCommand (ref vel);
		UpCommand (ref vel);
		DownCommand (ref vel);
		ZCommand (ref vel);
		XCommand (ref vel);
		
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
			vel.x = transform.localScale.x * slideSpeed;
		}
		
		rigidbody2D.velocity = vel;
	}

	void FixedUpdate () {
		// enemy
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy"); 
		
		foreach (GameObject go in gos) {
			float distance = Mathf.Abs(transform.position.x - go.transform.position.x);
			//Debug.Log(go + " Distance : " + distance);
			EnemyScript es = (EnemyScript) go.GetComponent(typeof(EnemyScript));
			if (distance <= cameraRange && es.hasSpawn == false && es.hasEnter == false) {
				Debug.Log("Spawn");
				Debug.Log(go + " Distance : " + distance);
				es.hasEnter = true;
				es.Spwan ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}

	void OnCollisionEnter2D(Collision2D col) {
		/*
		if (col.gameObject.tag.Equals("Enemy")) {
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_slideAttack")) {//slide attack
				SingletonScript.Instance.toReset = true;
			} else { // kirby died
				Debug.Log(this + "collision kirby");
				Application.LoadLevel ("Vegetable Valley 1");
			}
		}
		*/
	}



	#region AttackFunction
	void inhale() {	
		inhaleInterval -= Time.deltaTime;
		if (inhaleInterval <= 0) {
			float dir = Mathf.Sign (transform.localScale.x);
			Vector3 startPos = transform.position;
			startPos.x += dir * renderer.bounds.size.x / 2;
			
			GameObject inh = Instantiate (inhalePrefab) as GameObject;
			inh.GetComponent<Inhale>().SetDirection (dir);
			inh.GetComponent<Inhale>().SetPos (startPos);
			
			inh = Instantiate (inhalePrefab) as GameObject;
			inh.GetComponent<Inhale>().SetDirection (dir);
			inh.GetComponent<Inhale>().SetPos (startPos);
			inhaleInterval = 0.1f;
		}
	}

	void shootStar() {
		Debug.Log ("kirby shoot star");

		float dir = Mathf.Sign (transform.localScale.x);
		Vector3 startPos = transform.position;
		startPos.x += dir * renderer.bounds.size.x / 2;

		GameObject star = 
			Instantiate (starPrefab, startPos, transform.rotation) as GameObject;
		star.GetComponent<StarPower>().SetDirection (dir);

		animator.SetFloat ("powerType", 0f);
	}

	void puffAttack() {
		Debug.Log ("kirby puff attack");
		
		float dir = Mathf.Sign (transform.localScale.x);
		Vector3 startPos = transform.position;
		startPos.x += dir * renderer.bounds.size.x / 2;

		GameObject puff = 
			Instantiate (puffPrefab, startPos, transform.rotation) 
			as GameObject;
		puff.GetComponent<PuffAttack>().SetDirection (dir);
	}

	void beamPower(){
		Debug.Log ("kirby executes beam");
		
		float dir = Mathf.Sign (transform.localScale.x);
		Vector3 startPos = transform.position;
		startPos.x += dir * renderer.bounds.size.x / 2;
		
		GameObject beam = 
			Instantiate (beamPrefab, startPos, 
						 Quaternion.Euler (dir*new Vector3(0f, 0f, 90f))) 
			as GameObject;
		beam.GetComponent<BeamPower>().SetDirection (dir);
		beam.GetComponent<BeamPower>().SetAimTag ("Enemy");
	}
	
	void executeEnemyPower(float power) {
		if (power == 1.0f) {
			beamPower();
		}
	}
	#endregion



	#region ActionControl

	#region LeftRightCommand
	void LeftRightCommand(ref Vector2 vel) {
		// "LEFT" & "RIGHT" command: walk left and right
		float horizontal = Input.GetAxis ("Horizontal");

		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")
		    && !animator.GetBool ("duck")) {
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
	}
	#endregion

	#region UpCommand
	void UpCommand(ref Vector2 vel) {
		// "UP" command: Take in air and float in the air
		//				 Flap arms to fly higher while floating
		//				 Pass through a doorfly 
		float vertical = Input.GetAxis ("Vertical");

		if (Input.GetKey (KeyCode.UpArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")
		    && !animator.GetBool ("duck")
		    && !animator.GetBool ("withEnemy")
		    && !changeScene.atDoor) {
			vel.y = vertical * flySpeed;		
			animator.SetBool ("withAir", true);
			animator.speed = 3;
		} else {
			animator.speed = 1;
		}
	}
	#endregion

	#region DownCommand
	void DownCommand(ref Vector2 vel) {
		// "DOWN" command: Duck when standing still
		//				   Swallow current item in Kirby's mouth 
		//				   Copy power
		if (Input.GetKeyDown (KeyCode.DownArrow) 
		    && animator.GetBool("withEnemy")) { // copy power
			animator.SetBool("withEnemy", false);
			executeEnemyPower(animator.GetFloat("powerType"));
			vel.x = 0;
		}

		if (Input.GetKey (KeyCode.DownArrow)) { // duck
			animator.SetBool("duck", true);
			vel.x = 0;
		}

		if (Input.GetKeyUp (KeyCode.DownArrow)) { 
			if (animator.GetBool("withEnemy")) { // swallow
				animator.SetBool("withEnemy", false);
			}
			animator.SetBool("duck", false);
		}
	}
	#endregion

	#region ZCommand
	void ZCommand(ref Vector2 vel) {
		// "Z" command: jump
		if (Input.GetKeyDown (KeyCode.Z) 
		    && (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_stand")
		    	|| animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_walk")
		    	|| animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_dash"))) {
			if (grounded) {
				vel.y = jumpSpeed;
				animator.speed = 0;
				animator.SetBool("jump", true);
			}
		}

		if (Input.GetKey (KeyCode.Z)
		    && animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_jump")) {
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
			animator.SetBool("jump", false);
		}
	}
	#endregion

	#region XCommand
	void XCommand(ref Vector2 vel) {
		// "X" command: inhale when Kirby doesn't have a copy ability
		//              Shoot out a star when an item is in Kirby's mouth 
		//              Use power when Kirby has a copy ability 
		//              Puff attack while floating
		if (Input.GetKeyDown (KeyCode.X)) {
			if (animator.GetBool("withEnemy")) { // shoot star
				animator.SetBool("withEnemy", false);
				shootStar();
			} else if (animator.GetBool("withAir")) { // puff attack
				animator.SetBool("withAir", false);
				puffAttack();
			} else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")) { //use power
				animator.SetBool("executing", true);
				executeEnemyPower(animator.GetFloat("powerType"));
				vel.x = 0;
			}
		} 
		
		if (Input.GetKey (KeyCode.X) 
		    && animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")) { 
			if (animator.GetFloat("powerType") == 0f) { // inhale
				inhale ();
				vel.x = 0;
			}
		}

		if (Input.GetKeyUp (KeyCode.X)) {
			animator.SetBool("executing", false);
		}
	}
	#endregion

	#endregion
}
