using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public float		speed = -1.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vel = rigidbody2D.velocity;
		vel.x = speed;
		rigidbody2D.velocity = vel;

	}

	public void ChangeDirection () {
		Debug.Log ("speed = "+speed);
		Vector3 curr_scale = transform.localScale;
		transform.localScale = new Vector3(curr_scale.x*-1f,curr_scale.y*1f,1f);
		speed = - speed;
		Vector3 curr_pos = transform.position;
		curr_pos.x += 0.1f * speed;
		transform.position = curr_pos;
	}

}
