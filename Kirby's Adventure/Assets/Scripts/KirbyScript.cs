using UnityEngine;
using System.Collections;

public class KirbyScript : MonoBehaviour {

	public float		speed = 6;
	public float		jumpSpeed = 5;
	public float		jumpAcc = 1;
	
	public bool			grounded = true;

	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		
		Vector2 vel = rigidbody2D.velocity;
		vel.x = h*speed;
		
		if (Input.GetKeyDown(KeyCode.X) ) {
			if (grounded) vel.y = jumpSpeed;
		}
		if (Input.GetKey(KeyCode.X) ) {
			if (!grounded) vel.y += jumpAcc * Time.deltaTime;
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
			Debug.Log("collision kirby");
			Application.LoadLevel ("Vegetable Valley 1");
		}
	}
}
