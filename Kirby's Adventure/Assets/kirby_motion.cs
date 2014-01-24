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

		if (horizontal > 0) {
			transform.localScale = new Vector3(1f,1f,1f);
			animator.SetInteger("Direction", 2);
			//move kirby
			transform.position += transform.right * Time.deltaTime* 0.6f;
		} else if (horizontal < 0) {
			transform.localScale = new Vector3(-1f,1f,-1f);
			animator.SetInteger("Direction", 3);
			//move kirby
			transform.position -= transform.right * Time.deltaTime * 0.6f;
		} else {
			//change state
			var current_direction = animator.GetInteger ("Direction");
			if (current_direction == 2) {
				animator.SetInteger("Direction", 0);
				transform.localScale = new Vector3(1f,1f,1f);
			}
			if (current_direction == 3) {
				animator.SetInteger("Direction", 1);
				transform.localScale = new Vector3(-1f,1f,-1f);
			}
		}
	}
}
