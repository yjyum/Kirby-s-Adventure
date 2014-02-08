using UnityEngine;
using System.Collections;

public class SingletonScript : MonoBehaviour {

	private static SingletonScript instance = null;
	public static SingletonScript Instance { get {return instance;} }

	public Animator kirby_animator;
	public GameObject current_enemy;

	void Awake () {
		instance = this;
	}

}
