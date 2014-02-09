using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public bool 		hasSpawn = false;
	public bool 		hasEnter = false;
	public GameObject	beamPrefab;

	private float		jumpSpeed = 15f;
	private float		jumpAcc = 0.5f;
	private MoveScript	moveScripte;
	private GameObject 	kirby;
	private Vector3 	originalPosition;
	private float 		enemyScale = 4f;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	void Awake() {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		kirby = GameObject.FindWithTag("Player");
		originalPosition = transform.position;
		InvokeRepeating("EnemyAttack", 5, 1);
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
				Debug.Log(this + "attack kirby");
				SingletonScript.Instance.current_enemy = gameObject;
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
				} else { // kirby died
					Debug.Log(this + "collision kirby");
					Application.LoadLevel ("Vegetable Valley 1");
				}
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
			//Debug.Log (action);

			if (action >= 1) {
				Vector2 vel = rigidbody2D.velocity;
				vel.y = jumpSpeed;
				rigidbody2D.velocity = vel;
			} else {

			}
		}
	}
}
