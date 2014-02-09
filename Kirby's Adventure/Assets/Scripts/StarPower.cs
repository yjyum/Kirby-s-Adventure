using UnityEngine;
using System.Collections;

public class StarPower : MonoBehaviour {
	
	public float 	direction;
	public float 	force = 500f;
	public AudioClip 		scoreSound;

	void Start() {
		rigidbody2D.AddForce (new Vector2 (force * direction, 0));
	}

	void OnCollisionEnter2D(Collision2D col) {
		Debug.Log("star power on collision with" + col.gameObject);
		if (col.gameObject.tag.Equals("Enemy")) {
			EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
			es.Reset();
			SingletonScript.Instance.score += 600;

			PlaySoundEffect(scoreSound, false, false, 0.4f);
		}
		Destroy (gameObject);
	}

	public void SetDirection(float dir) {
		direction = dir;
	}

	public void SetAudio(AudioClip score) {
		scoreSound = score;
	}
	
	void PlaySoundEffect(AudioClip clip, bool loop, bool onAwake, float vol) {
		AudioSource audio = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
		audio.clip = clip;
		audio.loop = loop;
		audio.playOnAwake = onAwake;
		audio.volume = vol;
		audio.Play();
	}
}
