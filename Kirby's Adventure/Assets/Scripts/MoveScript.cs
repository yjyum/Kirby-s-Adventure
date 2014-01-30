using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public float		speed = -2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vel = rigidbody2D.velocity;
		vel.x = speed;
		rigidbody2D.velocity = vel;

	}

}
