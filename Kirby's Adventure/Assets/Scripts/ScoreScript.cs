using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject go = GameObject.Find("life");
		if (SingletonScript.Instance.kirby_life <= 18) {
			//Debug.Log ("text mesh!!!");
			TextMesh tm = (TextMesh) go.GetComponent(typeof(TextMesh));
			tm.text = "03";
		}
		if (SingletonScript.Instance.kirby_life <= 12) {
			TextMesh tm = (TextMesh) go.GetComponent(typeof(TextMesh));
			tm.text = "02";
		}
		if (SingletonScript.Instance.kirby_life <= 6) {
			TextMesh tm = (TextMesh) go.GetComponent(typeof(TextMesh));
			tm.text = "02";
		}
		if (SingletonScript.Instance.kirby_life <= 0) {
			TextMesh tm = (TextMesh) go.GetComponent(typeof(TextMesh));
			tm.text = "00";
		}

		if (SingletonScript.Instance.kirby_life % 6 == 5) {
			Destroy (GameObject.Find("blood_0"));
		}
		if (SingletonScript.Instance.kirby_life % 6 == 4) {
			Destroy (GameObject.Find("blood_1"));
		}
		if (SingletonScript.Instance.kirby_life % 6 == 3) {
			Destroy (GameObject.Find("blood_2"));
		}
		if (SingletonScript.Instance.kirby_life % 6 == 2) {
			Destroy (GameObject.Find("blood_3"));
		}
		if (SingletonScript.Instance.kirby_life % 6 == 1) {
			Destroy (GameObject.Find("blood_4"));
		}

		go = GameObject.Find("score");
		TextMesh tmm = (TextMesh) go.GetComponent(typeof(TextMesh));
		tmm.text = SingletonScript.Instance.score.ToString();
	}
}
