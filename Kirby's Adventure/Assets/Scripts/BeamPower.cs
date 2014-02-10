using UnityEngine;
using System.Collections;

public class BeamPower : MonoBehaviour {

	public string 			aimTag;
	public float 			direction;
	public float 			rotateSpeed = 5f;
	public float 			timeRemaining = 0.5f;

	public AudioClip 		scoreSound;
	public AudioClip 		loseBloodSound;
	public AudioClip		loseLifeSound;

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

				PlaySoundEffect(scoreSound, false, false, 0.4f);
			}

			if (aimTag.Equals("Player")) {
				//Debug.Log(SingletonScript.Instance.kirby_life);

				EnemyScript es = (EnemyScript) 
					SingletonScript.Instance.current_enemy.GetComponent(typeof(EnemyScript));
				if (es.hasSpawn) {
					SingletonScript.Instance.kirby_life --;
					es.Reset();
				}

				PlaySoundEffect(loseBloodSound, false, false, 0.4f);

				if (SingletonScript.Instance.kirby_life % 6 == 0) {
					PlaySoundEffect(loseLifeSound, false, false, 0.4f);
					Application.LoadLevel (Application.loadedLevel);
				}
			}

			SingletonScript.Instance.score += 600;
		}
	}
	
	public void SetDirection(float dir) {
		direction = dir;
	}

	public void SetAimTag(string aim) {
		aimTag = aim;
	}

	public void SetAudio(AudioClip score, AudioClip loseBlood, AudioClip loseLife) {
		scoreSound = score;
		loseBloodSound = loseBlood;
		loseLifeSound = loseLife;
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
