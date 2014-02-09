using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform 	poi; // Point of Interest
	public float		u;
	public Vector3		offset = new Vector3(0,0,-5);
	public float screenEdgeLeft = -17.73f; 
	public float screenEdgeRight = 11.5f;
	public float screenEdgeTop = 7.5f; 
	public float screenEdgeBotton = 7f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 poiV3 = poi.position + offset;
		Vector3 currPos = transform.position;
		Vector3 pos = (1-u)*currPos + u*poiV3;
		// limit camera in screen
		if (pos.x < screenEdgeLeft) {
			pos.x = screenEdgeLeft;
		}
		if (pos.x > screenEdgeRight) {
			pos.x = screenEdgeRight;
		}
		if (pos.y > screenEdgeTop) {
			pos.y = screenEdgeTop;
		}
		if (pos.y < screenEdgeBotton) {
			pos.y = screenEdgeBotton;
		}
		pos.y -= 1f;
		transform.position = pos;
	}
}
