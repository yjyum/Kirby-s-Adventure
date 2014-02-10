using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public bool 		canJump = true;
	public bool 		hasPower = true;
	public int 			powerType = 0;

	public bool 		hasSpawn = false;
	public bool 		hasEnter = false;

	public AudioClip 	scoreSound;
	public AudioClip 	loseBloodSound;
	public AudioClip 	beamSound;
	public AudioClip 	loseLifeSound;

	private float 		speed;
	private float		jumpSpeed = 4f;
	private float		jumpAcc = 3f;
	private MoveScript	moveScripte;
	private GameObject 	kirby;
	private Vector3 	originalPosition;
	private float 		enemyScale = 4f;
	private float		powerTime = 0.8f;
	private float 		sparkInterval = 0f;
	private float		fireInterval = 0.2f;
	private bool		executing = false;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	public GameObject	beamPrefab;
	public GameObject	sparkPrefab;
	public GameObject	firePrefab;

	void Awake() {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		kirby = GameObject.FindWithTag("Player");
		originalPosition = transform.position;
		speed = moveScripte.speed;
		InvokeRepeating("EnemyAttack", 0f, 1.8f);
	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(kirby.transform.position.x/2.5f - transform.position.x/4f) > cameraRange && hasSpawn == false) {
			hasEnter = false;
		}

		if (hasSpawn == true) {
			if (Mathf.Abs(kirby.transform.position.x/2.5f - transform.position.x/4f) <= KirbyScript.kirbyAttackDis) {
			//	Debug.Log(this + "current enemy change");
				SingletonScript.Instance.current_enemy = gameObject;
			}

			if (Mathf.Abs(kirby.transform.position.x/2.5f - transform.position.x/4f) <= enemyAttackDis) {
			//	Debug.Log(this + "attack kirby");
				SingletonScript.Instance.current_enemy = gameObject;
			}
		}

		if (executing) {
			powerTime -= Time.deltaTime;
			if (powerTime <= 0) {
				powerTime = 1f;
				executing = false;
				moveScripte.speed = speed;
			} else {
			//	Debug.Log ("enemy executs power");
				moveScripte.speed = 0f;
			}

			if (powerType == 2) {
				sparkInterval -= Time.deltaTime;
				if (sparkInterval <= 0) {
					GameObject spark = 
						Instantiate (sparkPrefab, 
						             new Vector3(transform.position.x + (int)Random.Range(-1,2)*(renderer.bounds.size.x/2 + 0.2f),
						            transform.position.y + (int)Random.Range(-1,2)*(renderer.bounds.size.x/2 + 0.2f),
						             transform.position.z), 
						             transform.rotation) as GameObject;
					spark.GetComponent<Spark>().SetAimTag ("Player");
					sparkInterval = 0.1f;
				}
			}

			if (powerType == 3) {
				fireInterval -= Time.deltaTime;
				if (fireInterval <= 0) {
					float dir = -Mathf.Sign (transform.localScale.x);
					GameObject fire = 
						Instantiate (firePrefab, 
						             new Vector3(transform.position.x + dir*renderer.bounds.size.x/2,
						             transform.position.y, transform.position.z), 
						             transform.rotation) as GameObject;
					fire.GetComponent<fire>().SetAimTag ("Player");
					fire.GetComponent<fire>().SetDirection (dir);
					fireInterval = 0.2f;
				}
			}
		}
	}

	void FixedUpdate() {
		Vector2 vel = rigidbody2D.velocity;

		if (vel.y > 0) {
			vel.y += jumpAcc * Time.deltaTime;
		}

		rigidbody2D.velocity = vel;
	}

	public void Spwan () {
		moveScripte.enabled = true;

		if (kirby.transform.position.x/2.5f <= transform.position.x/4f) {
			moveScripte.speed = -2f;
		} else {
			moveScripte.speed = 2f;
		}
		Debug.Log ("kirby " +kirby.transform.position.x/2.5f + " enemy " +  transform.position.x/4f);
		transform.localScale = new Vector3 (enemyScale,enemyScale , 0);
		hasSpawn = true;
		collider2D.enabled = true;
		transform.position = originalPosition;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (hasSpawn) {
			// enemy reach edge
			if (col.gameObject.tag.Equals("EdgePlatform")) {
				Debug.Log(this + "enemy collision edge 1");
				Reset();
			}
			if (col.gameObject.tag.Equals("BottonPlatform")) {
				Debug.Log(this + "enemy collision edge 2");
				moveScripte.ChangeDirection();
			}
			// collide with kirby
			if (col.gameObject.tag.Equals("Player")) {
				if (SingletonScript.Instance.kirby_animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_slideAttack")) {//slide attack
					Reset();

					PlaySoundEffect(scoreSound, false, false, 0.4f);
				} else { // kirby died
					Debug.Log(SingletonScript.Instance.kirby_life);
					SingletonScript.Instance.kirby_life --;
					SingletonScript.Instance.kirby_animator.Play("kirby_revive");
					SingletonScript.Instance.kirby_animator.SetBool ("withAir", false);
					SingletonScript.Instance.kirby_animator.SetBool ("withEnemy", false);
					SingletonScript.Instance.kirby_animator.SetBool ("executing", false);
					SingletonScript.Instance.kirby_animator.SetFloat ("powerType", 0f);
					Reset();

					PlaySoundEffect(loseBloodSound, false, false, 0.4f);

					if (SingletonScript.Instance.kirby_life % 6 == 0) {
						Application.LoadLevel (Application.loadedLevel);
					}
				}

				SingletonScript.Instance.score += 600;
			}
		}
	}

	public void Reset() {
		transform.position = originalPosition;
		moveScripte.enabled = false;
		transform.localScale = new Vector3 (0, 0, 0);
		hasSpawn = false;
	}

	private void EnemyAttack() {
		if (hasSpawn) {
			float action = Random.Range(0f, 10f)*10%2;

			if (action >= 1 && canJump) {
				Vector2 vel = rigidbody2D.velocity;
				vel.y = jumpSpeed;
				rigidbody2D.velocity = vel;
			} 

			if (action < 1 && hasPower){
				executing = true;

				float dir = -Mathf.Sign (transform.localScale.x);
				Vector3 startPos = transform.position;
				startPos.x += dir * renderer.bounds.size.x / 2;
<<<<<<< HEAD

				if (powerType == 1) {
					GameObject beam = 
						Instantiate (beamPrefab, startPos, 
					             	Quaternion.Euler (dir*new Vector3(0f, 0f, 90f))) 
							as GameObject;
					beam.GetComponent<BeamPower>().SetDirection (dir);
					beam.GetComponent<BeamPower>().SetAimTag ("Player");
				} else if (powerType == 2) {
					GameObject spark = 
						Instantiate (sparkPrefab, transform.position, transform.rotation) as GameObject;
					spark.GetComponent<Spark>().SetAimTag ("Player");
				} else if (powerType == 3) {
					GameObject fire = 
						Instantiate (firePrefab, transform.position, transform.rotation) as GameObject;
					fire.GetComponent<fire>().SetAimTag ("Player");
					fire.GetComponent<fire>().SetDirection (dir);
				}
=======
				
				GameObject beam = 
					Instantiate (beamPrefab, startPos, 
					             Quaternion.Euler (dir*new Vector3(0f, 0f, 90f))) 
						as GameObject;
				beam.GetComponent<BeamPower>().SetDirection (dir);
				beam.GetComponent<BeamPower>().SetAimTag ("Player");
				beam.GetComponent<BeamPower>().SetAudio (scoreSound, loseBloodSound, loseLifeSound);

				PlaySoundEffect(beamSound, false, false, 0.4f);
>>>>>>> 261e5557a51e013fcc64d200d720f1f2de19c695

				speed = moveScripte.speed;
			}
		}
	}

	void PlaySoundEffect(AudioClip clip, bool loop, bool onAwake, float vol) {
		AudioSource audio = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
		audio.clip = clip;
		audio.loop = loop;
		audio.playOnAwake = onAwake;
		audio.volume = vol;
		audio.Play();
	}
}
