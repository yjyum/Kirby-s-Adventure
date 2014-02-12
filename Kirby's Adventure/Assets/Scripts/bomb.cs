using UnityEngine;
using System.Collections;

public class bomb : MonoBehaviour {

	private float xDelta = 0.02f;
	private float x;
	private float y;

	void Start() {
		x = transform.position.x;
		y = transform.position.y;
	}

	void Update () {
		Vector3 pos = transform.position;
		pos.x += xDelta;
		pos.y = y;
		transform.position = pos;
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag.Equals("Player")) {		
			SingletonScript.Instance.kirby_life --;
			SingletonScript.Instance.kirby_animator.Play("kirby_revive");
			Vector3 pos = col.gameObject.transform.position;
			pos.x -= 3f;
			pos.y += 3f;
			col.gameObject.transform.position = pos;
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

			pos = transform.position;
			pos.x = x;
			pos.y = y;
			transform.position = pos;
			xDelta = 0.02f;
		} 

		if (col.gameObject.layer == 12) {
			xDelta = -xDelta;
		}
	}
}
