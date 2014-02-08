using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public bool hasSpawn = false;
	public bool hasEnter = false;
	private MoveScript moveScripte;
	private GameObject kirby;
	private Vector3 originalPosition;
	private float enemyScale = 4f;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	void Start () {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		kirby = GameObject.FindWithTag("Player");
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if ((kirby.transform.position - transform.position).magnitude > cameraRange) {
			hasEnter = false;
		}

		if (hasSpawn == true) {
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
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		transform.position = originalPosition;
	}

	void EnemyAttack() {
	}
}
