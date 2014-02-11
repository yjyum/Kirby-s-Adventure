using UnityEngine;
using System.Collections;

public class Spark : MonoBehaviour {
	public string 		aimTag;
	public float 		eachTime = 0.3f; 
	public float		totalTime = 1f;

	public AudioClip 		scoreSound;
	public AudioClip 		loseBloodSound;
	public AudioClip		loseLifeSound;

	void Start() {
		//PlaySoundEffect(sparkSound, false, false, 0.4f);
	}
	
	void Update() {
		eachTime -= Time.deltaTime;
		if (eachTime <= 0) {
			Destroy(gameObject);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag.Equals(aimTag)) {
			Debug.Log("Sparky on collision with" + col.gameObject);
			if (aimTag.Equals("Enemy")) {
				EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
				if (es.hasSpawn) {
					es.Reset();
					SingletonScript.Instance.score += 100;
					SingletonScript.Instance.kirby_animator.SetBool("executing", false);
					SingletonScript.Instance.kirby_animator.SetFloat("powerType", 2f); //different power TODO
				}
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
		Destroy (gameObject);
	}

	public void SetAimTag(string aim) {
		aimTag = aim;
	}

	public void SetAudio(AudioClip score, AudioClip loseBlood, AudioClip loseLife) {
		scoreSound = score;
		loseBloodSound = loseBlood;
		loseLifeSound = loseLife;
	}
	
	public void PlaySoundEffect(AudioClip clip, bool loop, bool onAwake, float vol) {
		AudioSource audio = (AudioSource) gameObject.AddComponent(typeof(AudioSource));
		audio.clip = clip;
		audio.loop = loop;
		audio.playOnAwake = onAwake;
		audio.volume = vol;
		audio.Play();
		Destroy(audio, clip.length);
	}
}
