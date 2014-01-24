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
			animator.SetBool("Direction", true);
			//move kirby
			transform.position += transform.right * Time.deltaTime* 0.8f;
		} else if (horizontal < 0) {
			transform.localScale = new Vector3(-1f,1f,-1f);
			animator.SetBool("Direction", true);
			//move kirby
			transform.position -= transform.right * Time.deltaTime * 0.8f;
		} else {
			animator.SetBool("Direction", false);
		}
	}
}
