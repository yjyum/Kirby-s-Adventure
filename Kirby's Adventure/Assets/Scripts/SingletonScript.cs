using UnityEngine;
using System.Collections;

public class SingletonScript : MonoBehaviour {

	private static SingletonScript instance = null;
	public static SingletonScript Instance { get {return instance;} }

	public int kirby_life = 24;
	public int score = 0;
	public Animator kirby_animator;
	public GameObject current_enemy;

	void Awake () {
		instance = this;
		GameObject go = GameObject.Find("Scripts");
		SavedVariables sv = (SavedVariables) go.GetComponent (typeof (SavedVariables));
		if (sv.life != 24 || sv.score != 0) {
			kirby_life = sv.life;
			score = sv.score;
		}

	}

	void Update () {

	}

}
