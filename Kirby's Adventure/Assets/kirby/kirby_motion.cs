using UnityEngine;
using System.Collections;

public class kirby_motion : MonoBehaviour {

	private Animator animator;
	
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		var horizontal = Input.GetAxis ("Horizontal");
		var vertical = Input.GetAxis ("Vertical");

		// horizontal move
		if (horizontal > 0) {
			transform.localScale = new Vector3(1f,1f,1f);
			animator.SetBool("Direction", true);
			//move kirby right
			transform.position += transform.right * Time.deltaTime* 0.8f;
		} else if (horizontal < 0) {
			transform.localScale = new Vector3(-1f,1f,-1f);
			animator.SetBool("Direction", true);
			//move kirby left
			transform.position -= transform.right * Time.deltaTime * 0.8f;
		} else{
			animator.SetBool("Direction", false);
		}

		// vertical move
		if (vertical > 0 ) {
			transform.rigidbody2D.mass = 0.3f;
			animator.SetBool("withAir", true);
			transform.position += transform.up * Time.deltaTime * 2.5f;
		} 

		// exhale or inhale air
		if (Input.GetKeyDown (KeyCode.X)) {
			if (animator.GetBool("withAir")) {
				animator.SetBool("withAir", false);
			} else {
				animator.SetBool("inhale", true);
			}
		} else if (Input.GetKeyUp (KeyCode.X)) {
			animator.SetBool("inhale", false);
		}

		// jump
		if (Input.GetKey (KeyCode.Z)) {
			if (transform.position.y < 2.5f) {
				transform.position += transform.up * Time.deltaTime * 2.5f;
			}
		} else if (Input.GetKeyUp (KeyCode.Z)) {
			animator.SetBool("jump", true);
		} else {
			animator.SetBool("jump", false);
		}
	}
}
