using UnityEngine;
using System.Collections;

public class PuffAttack : MonoBehaviour {

	public float 	direction;
	public float 	force = 300f;
	public float 	timeRemaining = 0.2f;

	void Start() {
		rigidbody2D.AddForce (new Vector2 (force * direction, 0));
		if (direction < 0) {
			Vector3 face = transform.localScale;
			face.x *=  (-1);
			face.z = -1f;
			transform.localScale = face;
		}
	}

	void Update() {
		timeRemaining -= Time.deltaTime;
		if (timeRemaining <= 0f) {
				Destroy (gameObject);
		}
	}

	
	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("puff attack on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals("Enemy")) {
			Destroy (col.gameObject);
		}
		Destroy (gameObject);
	}

	public void SetDirection(float dir) {
		direction = dir;
	}
}
