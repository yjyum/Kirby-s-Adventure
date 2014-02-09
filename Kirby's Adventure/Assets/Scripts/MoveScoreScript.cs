using UnityEngine;
using System.Collections;

public class MoveScoreScript : MonoBehaviour {

	private bool hasOrigin = false;
	private Vector3 origin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasOrigin) {
			hasOrigin = true;
			origin = Camera.main.transform.position;
		}

		Vector3 pos = Camera.main.transform.position;
		pos.y = origin.y - 3f;
		pos.z = 0f;
		transform.position = pos;
	}
}
