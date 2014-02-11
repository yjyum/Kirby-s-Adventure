using UnityEngine;
using System.Collections;

public class gun : MonoBehaviour {

	public GameObject bulletPrefab;
	public float attackDis = 10f;

	private float shootInterval = 0f;
	private GameObject kirby;

	void Start() {
		kirby = GameObject.FindWithTag("Player");
	}

	void Update () {
		if (Mathf.Abs(kirby.transform.position.x - transform.position.x) < attackDis) {
			shootInterval -= Time.deltaTime;
			if (shootInterval <= 0) {
				GameObject bullet = 
					Instantiate (bulletPrefab, transform.position, transform.rotation)
					as GameObject;
				shootInterval = 3f;
			}
		}

		Vector3 scale = transform.localScale;
		if (transform.position.x < kirby.transform.position.x) {
			transform.localScale = new Vector3(-Mathf.Abs(scale.x), Mathf.Abs(scale.y), 1f);
		} else {
			transform.localScale = new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), 1f);
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
			
			if (SingletonScript.Instance.kirby_life % 6 == 0) {
				Application.LoadLevel (Application.loadedLevel);
			}

			Vector3 pos = col.gameObject.transform.position;
			pos.x += Mathf.Sign(col.gameObject.transform.position.x - transform.position.x) 
				* renderer.bounds.size.x / 2;
			col.gameObject.transform.position = pos;
		}
		if (col.gameObject.layer == 12) {
			Destroy (gameObject);
		}
	}
}
