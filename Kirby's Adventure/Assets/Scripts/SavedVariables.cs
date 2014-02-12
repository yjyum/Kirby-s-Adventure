
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

	public void callRevive() {
		StartCoroutine(Revive());
	}
	
	public IEnumerator Revive() {
		
		Debug.Log("no collision");
		Physics2D.IgnoreLayerCollision (8, 9, true);
		Physics2D.IgnoreLayerCollision (8, 11, true);
		yield return new WaitForSeconds (3);
		Debug.Log("collision");
		Physics2D.IgnoreLayerCollision (8, 9, false);
		Physics2D.IgnoreLayerCollision (8, 11, false);
		yield break;
		
	}
}
