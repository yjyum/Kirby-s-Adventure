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
		var vertical = Input.GetAxis ("Vertical");
		var horizontal = Input.GetAxis ("Horizontal");

		if (horizontal > 0) {
			animator.SetBool("Direction", true);
			transform.position += transform.right * Time.deltaTime* 0.6f;
		} else if (horizontal < 0) {
			animator.SetBool("Direction", false);
			transform.position -= transform.right * Time.deltaTime * 0.6f;
		} else {
			animator.SetBool("Direction", false);
		}
	}
}
