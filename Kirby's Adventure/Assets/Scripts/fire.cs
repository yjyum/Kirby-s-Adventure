using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {

	public float 	direction;
	public float 	force = 200f;
	public float 	timeRemaining = 0.3f;
	public string 	aimTag;
	
	void Start() {
		rigidbody2D.AddForce (new Vector2 (force * direction, Random.Range(-0.1f, 0.1f)));
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
		Debug.Log("fire power on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals(aimTag)) {
			if (aimTag.Equals("Enemy")) {
				EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
				es.Reset();
				SingletonScript.Instance.score += 100;
			}
			
			if (aimTag.Equals("Player")) {
				Debug.Log(SingletonScript.Instance.kirby_life);
				SingletonScript.Instance.kirby_life --;
				SingletonScript.Instance.kirby_animator.Play("kirby_revive");
				SingletonScript.Instance.kirby_animator.SetBool ("withAir", false);
				SingletonScript.Instance.kirby_animator.SetBool ("withEnemy", false);
				SingletonScript.Instance.kirby_animator.SetBool ("executing", false);
				SingletonScript.Instance.kirby_animator.SetFloat ("powerType", 0f);
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
