using UnityEngine;
using System.Collections;

public class Inhale : MonoBehaviour {

	public float 	direction;
	public float 	inhaleWidth = 1f;
	public float 	inhaleHeight = 0.7f;
	public Vector3 	kirbyPos;
	public Vector3	startPos;
	public float 	updatePicTime = 0.1f; 
	public GameObject	character;
	public AudioClip 		scoreSound;

	void Start() {
		character = GameObject.FindWithTag ("Player");
		startPos = new Vector3(kirbyPos.x + direction * inhaleWidth, 
		                       kirbyPos.y + Random.Range(-1,2) * inhaleHeight,
		                       kirbyPos.z);
		transform.position = startPos;
	}
	
	void Update() {
		updatePicTime -= Time.deltaTime;
		if (updatePicTime <= 0) {
			transform.position += (kirbyPos - transform.position) * 0.4f;
			updatePicTime = 0.1f;
		}
		if ((transform.position.x - kirbyPos.x) * direction < 0.1f
		    || !character.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("kirby_executePower")) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag.Equals("Enemy")) {
			Debug.Log("inhaleParticle on collision with" + col.gameObject);
			EnemyScript es = (EnemyScript) col.gameObject.GetComponent(typeof(EnemyScript));
			if (es.hasSpawn) {
				es.Reset();
				SingletonScript.Instance.score += 300;

				PlaySoundEffect(scoreSound, false, false, 0.4f);

				character.GetComponent<Animator>().SetBool("withEnemy", true);
				character.GetComponent<Animator>().SetBool("executing", false);
				character.GetComponent<Animator>().SetFloat("powerType", es.powerType); //different power TODO
			}
		}
		//Destroy (gameObject);
	}
	
	public void SetDirection(float dir) {
		direction = dir;
	}

	public void SetPos(Vector3 pos) {
		kirbyPos = pos;
	}

	public void SetRange(float w, float h) {
		inhaleWidth = w;
		inhaleHeight = h;
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
