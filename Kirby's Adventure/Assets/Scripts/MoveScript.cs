using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public float		speed = -1.5f;
	public float 		flySpeed = 4f;
	public float		flyTime = 0.8f;
	public bool 		canFly = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 vel = rigidbody2D.velocity;
		vel.x = speed;
		if (canFly) {
			flyTime -= Time.deltaTime;
			if (flyTime <= 0) {
				flySpeed *= Random.Range (-1, 1);
				flyTime = 0.8f;
			} 
			if (transform.position.y <= 8f) {
				flySpeed = 4f;
				flyTime = 0.8f;
			}
			vel.y = flySpeed;
		}
		rigidbody2D.velocity = vel;
	}

	public void ChangeDirection () {
		Vector3 curr_scale = transform.localScale;
		transform.localScale = new Vector3(curr_scale.x*-1f,curr_scale.y*1f,1f);
		speed = - speed;
		Vector3 curr_pos = transform.position;
		curr_pos.x += 0.1f * speed;
		transform.position = curr_pos;
	}

}
