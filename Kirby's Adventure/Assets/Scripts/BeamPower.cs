using UnityEngine;
using System.Collections;

public class BeamPower : MonoBehaviour {

	public string 			aimTag;
	public float 			direction;
	public float 			rotateSpeed = 5f;
	public float 			timeRemaining = 0.5f;
	
	void Start() {
		if (direction < 0) {
			Vector3 face = transform.localScale;
			face.x *=  (-1);
			face.z = -1f;
			transform.localScale = face;
		}
	}

	void Update() {
		timeRemaining -= Time.deltaTime;

		if (aimTag.Equals("Enemy")) {
			GameObject kirby = GameObject.FindWithTag("Player");

			Vector3 p = kirby.rigidbody2D.velocity;
			p.x = 0;
			kirby.rigidbody2D.velocity = p;

			p = kirby.transform.position;
			p.y = kirby.transform.position.y;
			transform.position = p;
		} else {

		}
		transform.Rotate (new Vector3(0, 0, -rotateSpeed * direction));

		if (timeRemaining <= 0f) {
			Destroy (gameObject);
			if (aimTag.Equals("Enemy")) {
				Animator anim = 
					GameObject.FindWithTag("Player").GetComponent<Animator>();
				anim.SetBool("executing", false);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("beam power on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals(aimTag)) {
			if (aimTag.Equals("Enemy")) {
				EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
				es.Reset();
				SingletonScript.Instance.score += 100;
			}

			if (aimTag.Equals("Player")) {
				Debug.Log(SingletonScript.Instance.kirby_life);
				SingletonScript.Instance.kirby_life --;
				EnemyScript es = (EnemyScript) 
					SingletonScript.Instance.current_enemy.GetComponent(typeof(EnemyScript));
				es.Reset();
				if (SingletonScript.Instance.kirby_life % 6 == 0) {
					Application.LoadLevel ("Vegetable Valley 1");
				}
			}
		}
	}
	
	public void SetDirection(float dir) {
		direction = dir;
	}

	public void SetAimTag(string aim) {
		aimTag = aim;
	}
}
