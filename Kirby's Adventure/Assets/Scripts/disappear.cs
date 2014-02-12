using UnityEngine;
using System.Collections;

public class disappear : MonoBehaviour {

	private float disappearTime = 1.5f;

	void Update () {
		disappearTime -= Time.deltaTime;
		if (disappearTime <= 0) {
			Destroy(gameObject);
		}
	}
}
