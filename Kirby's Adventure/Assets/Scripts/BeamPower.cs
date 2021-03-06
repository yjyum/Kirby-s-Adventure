﻿using UnityEngine;
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

				if (es.hasSpawn) {
					PlaySoundEffect(scoreSound, false, false, 0.4f);
					SingletonScript.Instance.score += 600;
				}

				es.Reset();
			}

			if (aimTag.Equals("Player")) {
				//Debug.Log(SingletonScript.Instance.kirby_life);
				SingletonScript.Instance.kirby_animator.Play("kirby_revive");
				SingletonScript.Instance.kirby_animator.SetBool ("withAir", false);
				SingletonScript.Instance.kirby_animator.SetBool ("withEnemy", false);
				SingletonScript.Instance.kirby_animator.SetBool ("executing", false);
				SingletonScript.Instance.kirby_animator.SetFloat ("powerType", 0f);

				EnemyScript es = (EnemyScript) 
					SingletonScript.Instance.current_enemy.GetComponent(typeof(EnemyScript));
				if (es.hasSpawn) {
					SingletonScript.Instance.kirby_life --;



					GameObject kirby = GameObject.FindWithTag("Player");
					KirbyScript ks = (KirbyScript) kirby.GetComponent(typeof(KirbyScript));

					GameObject script = GameObject.FindWithTag("script");
					SavedVariables sv = (SavedVariables) script.GetComponent(typeof(SavedVariables));
					sv.callRevive();

					if (SingletonScript.Instance.kirby_life % 6 == 0) {
						ks.PlaySoundEffect(loseLifeSound, false, false, 0.4f);
						Application.LoadLevel (Application.loadedLevel);
					}else {
						ks.PlaySoundEffect(loseBloodSound, false, false, 0.4f);
					}
					Destroy(gameObject);
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
		Destroy(audio, clip.length);
	}

}
