using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private bool hasSpawn;
	private bool hasEnterCamera;
	private MoveScript moveScripte;
	private GameObject kirby;
	private Vector3 originalPosition;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	void Start () {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
		hasEnterCamera = false;
		transform.localScale = new Vector3 (0, 0, 0);
		kirby = GameObject.FindWithTag("Player");
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if ((kirby.transform.position - transform.position).magnitude > cameraRange) {
			hasEnterCamera = false;
		}

		if (hasSpawn == false) {
			if ((kirby.transform.position - transform.position).magnitude <= cameraRange && hasEnterCamera == false) {
				Spwan ();
				hasEnterCamera = true;
			}
		} else {
			if ((kirby.transform.position - transform.position).magnitude <= KirbyScript.kirbyAttackDis) {
			//	Debug.Log(this + "current enemy change");
				SingletonScript.Instance.current_enemy = gameObject;
			}

			if ((kirby.transform.position - transform.position).magnitude <= enemyAttackDis) {
				Debug.Log(this + "attack kirby");
				SingletonScript.Instance.current_enemy = gameObject;
				EnemyAttack();
			}
		}

		if (SingletonScript.Instance.toReset && gameObject == SingletonScript.Instance.current_enemy) {
			Reset ();
			SingletonScript.Instance.toReset = false;
		}
	}

	private void Spwan () {
		transform.localScale = new Vector3 (3f, 3f , 0);
		hasSpawn = true;
		collider2D.enabled = true;
		moveScripte.enabled = true;
	}

	void OnCollisionEnter2D(Collision2D col) {
		// enemy reach edge
		if (col.gameObject.tag.Equals("EdgePlatform")) {
			Debug.Log(this + "enemy collision edge");
			Reset();
		}
		// collide with kirby
		if (col.gameObject.tag.Equals("Player") && hasSpawn) {
			if (SingletonScript.Instance.kirby_animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_slideAttack")) {//slide attack
				SingletonScript.Instance.toReset = true;
			} else { // kirby died
				Debug.Log(this + "collision kirby");
				Application.LoadLevel ("Vegetable Valley 1");
			}
		}
	}

	private void Reset() {
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		transform.position = originalPosition;
	}

	void EnemyAttack() {
	}
}
