using UnityEngine;
using System.Collections;

public class throwAtKirby : MonoBehaviour {

	void Start () {
		Vector3 dir = GameObject.FindWithTag("Player").transform.position;
		Vector2 force = 
			new Vector2 (dir.x - transform.position.x, dir.y - transform.position.y);
		rigidbody2D.AddForce (force / force.magnitude * 400f);
	}

	void Update() {		
		if (rigidbody2D.velocity.magnitude <= 0.01f
		    || transform.position.y < 1f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag.Equals("Player")) {		
			SingletonScript.Instance.kirby_life --;
			SingletonScript.Instance.kirby_animator.Play("kirby_revive");
			SingletonScript.Instance.kirby_animator.SetBool ("withAir", false);
			SingletonScript.Instance.kirby_animator.SetBool ("withEnemy", false);
			SingletonScript.Instance.kirby_animator.SetBool ("executing", false);
			SingletonScript.Instance.kirby_animator.SetFloat ("powerType", 0f);

			GameObject script = GameObject.FindWithTag("script");
			SavedVariables sv = (SavedVariables) script.GetComponent(typeof(SavedVariables));
			sv.callRevive();
			
			if (SingletonScript.Instance.kirby_life % 6 == 0) {
				Application.LoadLevel (Application.loadedLevel);
			}
			Destroy(gameObject);
		} 
	}
}
