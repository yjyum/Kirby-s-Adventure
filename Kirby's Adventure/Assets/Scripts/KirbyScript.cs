﻿using UnityEngine;
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
	public float		sparkInterval = 0f;
	public float		fireInterval = 0.2f;

	public GameObject	starPrefab;
	public GameObject	puffPrefab;
	public GameObject	beamPrefab;
	public GameObject	sparkPrefab;
	public GameObject	inhalePrefab;
	public GameObject	firePrefab;
	public GameObject	discardStarPrefab;
	
	public Vector2 		vel;
	
	public bool			grounded = true;
	public int 			cameraRange = 6;
	// animator.powerType; 
	// 0:inhale	 1: beam 	2: spark	2:fire

	// kirby audio source
	public AudioClip jumpSound;
	public AudioClip inhaleSound;
	public AudioClip beamSound;
	public AudioClip flySound;
	public AudioClip loseBloodSound;
	public AudioClip loseLifeSound;
	public AudioClip gameOverSound;
	public AudioClip starSound;
	public AudioClip puffSound;
	public AudioClip slideSound;
	public AudioClip scoreSound;
	public AudioClip doorSound;
	public AudioClip sparkSound;
	public AudioClip fireSound;
	public AudioClip winSound;

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
		if (transform.position.y < 4f) {
			PlaySoundEffect (loseLifeSound, false, false, 0.4f);
			SingletonScript.Instance.kirby_life --;
			Vector3 pos = transform.position;
			pos.y += 3;
			transform.position = pos;
		} else {
			float horizontal = Input.GetAxis ("Horizontal");
		
			Vector2 vel = rigidbody2D.velocity;

			if (animator.GetBool ("withAir")) {
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

			if (Input.GetKey (KeyCode.Space) 
			    && !animator.GetBool("withEnemy")
			    && animator.GetFloat("powerType")!=0 ) {
				animator.SetFloat ("powerType", 0f);

				GameObject discardStar = 
					Instantiate(discardStarPrefab, new Vector3(transform.position.x, transform.position.y, -3f), transform.rotation)
					as GameObject;
				discardStar.rigidbody2D.velocity = 
					new Vector3(-Mathf.Sign(transform.localScale.x) * 5f, 5f);
			}
		
			// "DOUBLE LEFT" & "DOUBLE RIGHT" command: dash
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("kirby_walk")) {
				if ((Input.GetKeyDown (KeyCode.RightArrow) && vel.x > 0)
					|| (Input.GetKeyDown (KeyCode.LeftArrow) && vel.x < 0)) {
					vel.x = horizontal * dashSpeed;
					animator.SetBool ("dash", true);
				}
			}
		
			// "DOWN" + "Z"/"X" command: slide attack
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("kirby_duck")) {
				animator.SetBool ("slide", false);
				if (Input.GetKeyDown (KeyCode.Z)
					|| Input.GetKeyDown (KeyCode.X)) {
					animator.SetBool ("slide", true);
				}
			}
			if (animator.GetCurrentAnimatorStateInfo (0).IsName ("kirby_slideAttack")) {
				vel.x = transform.localScale.x * slideSpeed;

				PlaySoundEffect (slideSound, false, false, 0.4f);
			}
		
			rigidbody2D.velocity = vel;
		}
	}

	void FixedUpdate () {
		// enemy
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy"); 
		
		foreach (GameObject go in gos) {
			float distance = Mathf.Abs(transform.position.x - go.transform.position.x);
			//Debug.Log(go + " Distance : " + distance + go);
			EnemyScript es = (EnemyScript) go.GetComponent(typeof(EnemyScript));

			if (es && es.hasSpawn == false && distance > 6f) {
			//	Debug.Log("Spawn");
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
		if (col.gameObject.tag.Equals ("Powerup")) {
			Debug.Log("kirby on collision with" + col.gameObject);
			SingletonScript.Instance.score += 10000;
			Destroy(col.gameObject);
		}
	}

	public void PlaySoundEffect(AudioClip clip, bool loop, bool onAwake, float vol) {
		AudioSource audio = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
		audio.clip = clip;
		audio.loop = loop;
		audio.playOnAwake = onAwake;
		audio.volume = vol;
		audio.Play();
		Destroy(audio, clip.length);
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
			inh.GetComponent<Inhale>().SetRange (0.1f, 0f);
			inh.GetComponent<Inhale>().SetAudio (scoreSound);

			inh = Instantiate (inhalePrefab) as GameObject;
			inh.GetComponent<Inhale>().SetDirection (dir);
			inh.GetComponent<Inhale>().SetPos (startPos);
			inh.GetComponent<Inhale>().SetRange (0.5f , 0.2f);
			inh.GetComponent<Inhale>().SetAudio (scoreSound);

			inh = Instantiate (inhalePrefab) as GameObject;
			inh.GetComponent<Inhale>().SetDirection (dir);
			inh.GetComponent<Inhale>().SetPos (startPos);
			inh.GetComponent<Inhale>().SetAudio (scoreSound);
			
			inh = Instantiate (inhalePrefab) as GameObject;
			inh.GetComponent<Inhale>().SetDirection (dir);
			inh.GetComponent<Inhale>().SetPos (startPos);
			inh.GetComponent<Inhale>().SetAudio (scoreSound);
			inhaleInterval = 0.1f;

			PlaySoundEffect(inhaleSound, false, false, 0.4f);
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
		star.GetComponent<StarPower>().SetAudio (scoreSound, winSound);

		animator.SetFloat ("powerType", 0f);

		PlaySoundEffect(starSound, false, false, 0.4f);
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
		puff.GetComponent<PuffAttack>().SetAudio (scoreSound);

		PlaySoundEffect(puffSound, false, false, 0.4f);
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
		beam.GetComponent<BeamPower>().SetAudio (scoreSound, loseBloodSound, loseLifeSound);

		PlaySoundEffect(beamSound, false, false, 0.4f);
	}

	void sparkPower(){
		Debug.Log ("kirby executes spark");

		sparkInterval -= Time.deltaTime;
		if (sparkInterval <= 0) {		
			GameObject spark = 
				Instantiate (sparkPrefab, 
				             new Vector3(transform.position.x + (int)Random.Range(-1,2)*(renderer.bounds.size.x/2+0.2f),
				             transform.position.y + (int)Random.Range(-1,2)*(renderer.bounds.size.x/2+0.2f),
				             transform.position.z), 
				             transform.rotation) as GameObject;
			spark.GetComponent<Spark>().SetAimTag ("Enemy");
			spark.GetComponent<Spark>().SetAudio (scoreSound, loseBloodSound, loseLifeSound);
			sparkInterval = 0.05f;

			PlaySoundEffect(sparkSound, false, false, 0.4f);
		}
	}

	void firePower(){
		Debug.Log ("kirby executes fire");

		fireInterval -= Time.deltaTime;
		if (fireInterval <= 0) {
			float dir = Mathf.Sign (transform.localScale.x);
			GameObject fire = 
				Instantiate (firePrefab, 
					         new Vector3(transform.position.x + dir*renderer.bounds.size.x/2,
					         transform.position.y, transform.position.z), 
					         transform.rotation) as GameObject;
			fire.GetComponent<fire>().SetAimTag ("Enemy");
			fire.GetComponent<fire>().SetDirection (dir);
			fire.GetComponent<fire>().SetAudio (scoreSound, loseBloodSound, loseLifeSound);
			fireInterval = 0.2f;

			PlaySoundEffect(fireSound, false, false, 0.4f);
		}
	}
	
	void executeEnemyPower(float power) {
		if (power == 1.0f) {
			beamPower();
		} else if (power == 2.0f) {
			sparkPower ();
		} else if (power == 3.0f) {
			firePower();
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

		GameObject door = GameObject.FindGameObjectWithTag ("Door");
		if (door) {
			float distance = Mathf.Abs(door.transform.position.x-transform.position.x);
			if (Input.GetKeyDown (KeyCode.UpArrow)
				&& distance < 4f && transform.position.y < 7f) {
				PlaySoundEffect (doorSound, false, false, 0.4f);
				Application.LoadLevel("Vegetable Valley 2");
			}
		}

		if (Input.GetKey (KeyCode.UpArrow)
		    && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")
		    && !animator.GetBool ("duck")
		    && !animator.GetBool ("withEnemy")
		    && !changeScene.atDoor) {
			vel.y = vertical * flySpeed;		
			animator.SetBool ("withAir", true);
			animator.speed = 3;

			PlaySoundEffect(flySound, false, false, 0.4f);
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

				PlaySoundEffect(jumpSound, false, false, 0.4f);
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

		if (Input.GetKeyDown (KeyCode.Z) && animator.GetBool("withEnemy")) {
			if (grounded) {
				vel.y = jumpSpeed;
			}
			PlaySoundEffect(jumpSound, false, false, 0.4f);
		}
		
		if (Input.GetKey (KeyCode.Z) && animator.GetBool("withEnemy")) {
			if (!grounded && vel.y > 0) {
				vel.y += jumpAcc * Time.deltaTime;
			}
		}
		
		if (Input.GetKeyUp (KeyCode.Z) && animator.GetBool("withEnemy")) {
			if (!grounded && vel.y > 0) {
				vel.y = 0f;
			}
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
			} else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_duck")
			           && !animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")) { //use power
				animator.SetBool("executing", true);
				executeEnemyPower(animator.GetFloat("powerType"));
				vel.x = 0;
			}
		} 
		
		//holding power: inhale, spark, fire
		if (Input.GetKey (KeyCode.X) 
		    && animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")) { 
			if (animator.GetFloat("powerType") == 0f) { 
				inhale ();
				vel.x = 0;
			} else if (animator.GetFloat("powerType") == 2f) {
				sparkPower();
				vel.x = 0;
			} else if (animator.GetFloat("powerType") == 3f) {
				firePower();
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
