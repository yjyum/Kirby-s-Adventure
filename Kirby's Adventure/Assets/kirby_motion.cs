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
			animator.SetInteger("Direction", 1);
			transform.position += transform.right * Time.deltaTime* 0.6f;
		} else if (horizontal < 0) {
			animator.SetInteger("Direction", 2);
			transform.position -= transform.right * Time.deltaTime * 0.6f;
		} else {
			animator.SetInteger("Direction", 0);
		}
	}
}
