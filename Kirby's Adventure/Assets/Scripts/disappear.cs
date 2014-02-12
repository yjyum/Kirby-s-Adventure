using UnityEngine;
using System.Collections;

public class disappear : MonoBehaviour {

	private float disappearTime = 3f;

	void Update () {
		Debug.Log (transform.position.z);
		Vector3 pos = transform.position;
		pos.z = -0.1f;
		transform.position = pos;
		disappearTime -= Time.deltaTime;
		if (disappearTime <= 0) {
			Destroy(gameObject);
		}
	}
}
