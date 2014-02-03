using UnityEngine;
using System.Collections;

public class changeScene : MonoBehaviour {

	private Animator 	animator;

	public string 		nextScene;
	public static bool 	atDoor = false;

	void Start () {
		animator = this.GetComponent<Animator>();
	}

	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals("Door")) {
			atDoor = true;
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag.Equals("Door")) {
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("kirby_stand")
			    && Input.GetKeyDown(KeyCode.UpArrow)) {
				Debug.Log("change scene");
				atDoor = false;
				Application.LoadLevel (nextScene); 
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag.Equals("Door")) {
			atDoor = false;
		}
	}

}
