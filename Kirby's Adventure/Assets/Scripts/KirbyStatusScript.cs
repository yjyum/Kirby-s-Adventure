using UnityEngine;
using System.Collections;

public class KirbyStatusScript : MonoBehaviour {

	private Animator 	animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("status " + (int)SingletonScript.Instance.kirby_animator.GetFloat ("powerType"));
		animator.SetInteger ("status", (int)SingletonScript.Instance.kirby_animator.GetFloat ("powerType"));
	}
}
