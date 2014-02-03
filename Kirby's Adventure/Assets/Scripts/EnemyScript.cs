﻿using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private bool hasSpawn;
	private MoveScript moveScripte;
	private GameObject kirby;

	//public Transform kirby;
	public int cameraRange = 6;
	public static float enemyAttackDis = 2.5f;

	void Start () {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
		transform.localScale = new Vector3 (0, 0, 0);
		kirby = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (hasSpawn == false) {
			if ((kirby.transform.position - transform.position).magnitude <= cameraRange) {
				Spwan ();
			}
		} else {
			if ((kirby.transform.position - transform.position).magnitude <= KirbyScript.kirbyAttackDis) {
				Debug.Log(this + "current enemy change");
				SingletonScript.Instance.current_enemy = gameObject;
			}
			/*
			if ((kirby.transform.position - transform.position).magnitude <= KirbyScript.attackDis &&
			    SingletonScript.Instance.kirby_animator.GetBool("inhale") == true) {
				Debug.Log(this + "inhale close");
				Destroy(gameObject);
			}
			*/
			if ((kirby.transform.position - transform.position).magnitude <= enemyAttackDis) {
				Debug.Log(this + "attack kirby");
				SingletonScript.Instance.current_enemy = gameObject;
				EnemyAttack();
			}
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
			Destroy(gameObject);
		}
	}

	void EnemyAttack() {
	}
}