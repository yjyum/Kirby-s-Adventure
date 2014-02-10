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
	private float		beamTime = 0.8f;
	private bool		executing = false;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	public GameObject	beamPrefab;

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
		if (Mathf.Abs(kirby.transform.position.x - transform.position.x) > cameraRange && hasSpawn == false) {
			hasEnter = false;
		}

		if (hasSpawn == true) {
			if (Mathf.Abs(kirby.transform.position.x - transform.position.x) <= KirbyScript.kirbyAttackDis) {
			//	Debug.Log(this + "current enemy change");
				SingletonScript.Instance.current_enemy = gameObject;
			}

			if (Mathf.Abs(kirby.transform.position.x - transform.position.x) <= enemyAttackDis) {
			//	Debug.Log(this + "attack kirby");
				SingletonScript.Instance.current_enemy = gameObject;
			}
		}

		if (executing) {
			beamTime -= Time.deltaTime;
			if (beamTime <= 0) {
				beamTime = 1f;
				executing = false;
				moveScripte.speed = speed;
			} else {
			//	Debug.Log ("enemy executs power");
				moveScripte.speed = 0f;
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
		moveScripte.speed = -2f;
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
				
				GameObject beam = 
					Instantiate (beamPrefab, startPos, 
					             Quaternion.Euler (dir*new Vector3(0f, 0f, 90f))) 
						as GameObject;
				beam.GetComponent<BeamPower>().SetDirection (dir);
				beam.GetComponent<BeamPower>().SetAimTag ("Player");
				beam.GetComponent<BeamPower>().SetAudio (scoreSound, loseBloodSound, loseLifeSound);

				PlaySoundEffect(beamSound, false, false, 0.4f);

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
