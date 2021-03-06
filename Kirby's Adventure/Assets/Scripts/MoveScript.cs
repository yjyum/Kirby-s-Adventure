﻿using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public float		speed = -1.5f;
	public float 		flySpeed = 4f;
	public float		flyTime = 0.6f;
	public bool 		canFly = false;
	public bool			withUmbrella = false;

	private bool 		grounded = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vel = rigidbody2D.velocity;

		if (withUmbrella) {
			if (grounded) {
				this.GetComponent<Animator>().Play("waddleDoo");
				vel.x = speed;
				this.rigidbody2D.mass = 1f;
				this.rigidbody2D.gravityScale = 1f;
			}
		} else {
			vel.x = speed;
			if (canFly) {
				flyTime -= Time.deltaTime;
				if (flyTime <= 0) {
					flySpeed *= Random.Range (-1, 1);
					flyTime = 0.8f;
				} 
				if (transform.position.y <= 8.8f) {
					flySpeed = 4f;
					flyTime = 0.5f;
				}
				vel.y = flySpeed;
			}
		}

		rigidbody2D.velocity = vel;
	}

	public void ChangeDirection () {
		Vector3 curr_scale = transform.localScale;
		transform.localScale = new Vector3(curr_scale.x*-1f,curr_scale.y*1f,1f);
		speed = - speed;
		Vector3 curr_pos = transform.position;
		curr_pos.x += 0.1f * speed;
		transform.position = curr_pos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}
}
