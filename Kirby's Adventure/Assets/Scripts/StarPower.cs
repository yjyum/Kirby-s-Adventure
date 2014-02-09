using UnityEngine;
using System.Collections;

public class StarPower : MonoBehaviour {
	
	public float 	direction;
	public float 	force = 500f;

	void Start() {
		rigidbody2D.AddForce (new Vector2 (force * direction, 0));
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("star power on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals("Enemy")) {
			EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
			es.Reset();
			SingletonScript.Instance.score += 100;
		}
		Destroy (gameObject);
	}

	public void SetDirection(float dir) {
		direction = dir;
	}
}
