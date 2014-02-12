using UnityEngine;
using System.Collections;

public class StarPower : MonoBehaviour {
	
	public float 	direction;
	public float 	force = 500f;
	public AudioClip 		scoreSound;
	public AudioClip 		winSound;

	void Start() {
		rigidbody2D.AddForce (new Vector2 (force * direction, 0));
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("star power on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals("Enemy")) {
			EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));

			if (es.hasSpawn) {
				SingletonScript.Instance.score += 600;
				es.Reset();

				GameObject kirby = GameObject.FindWithTag("Player");
				KirbyScript ks = (KirbyScript) kirby.GetComponent(typeof(KirbyScript));
				ks.PlaySoundEffect(scoreSound, false, false, 0.4f);
			}

		}

		if (col.gameObject.tag.Equals("boss")) {
			SingletonScript.Instance.score += 100000;
			Destroy(col.gameObject);

			GameObject kirby = GameObject.FindWithTag("Player");
			KirbyScript ks = (KirbyScript) kirby.GetComponent(typeof(KirbyScript));
			ks.PlaySoundEffect(winSound, false, false, 0.4f);

			//Application.LoadLevel("Vegetable Valley 1");
		}

		Destroy (gameObject);
	}

	public void SetDirection(float dir) {
		direction = dir;
	}

	public void SetAudio(AudioClip score, AudioClip win) {
		scoreSound = score;
		winSound = win;
	}
}
