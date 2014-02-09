using UnityEngine;
using System.Collections;

public class MoveScoreScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Camera.main.transform.position;
		pos.y -= 4f;
		pos.x -= 1.4f;
		pos.z = 0f;
		transform.position = pos;
	}
}
