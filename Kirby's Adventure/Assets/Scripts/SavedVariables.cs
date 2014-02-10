
using UnityEngine;
using System.Collections;

public class SavedVariables : MonoBehaviour {

	public int life = 24;
	public int score = 0;

	void Awake() {
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		life = SingletonScript.Instance.kirby_life;
		score = SingletonScript.Instance.score;
	}
}
