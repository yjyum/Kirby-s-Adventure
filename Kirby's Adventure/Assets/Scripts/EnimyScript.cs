using UnityEngine;
using System.Collections;

public class EnimyScript : MonoBehaviour {

	private bool hasSpawn;
	private MoveScript moveScripte;
	public Transform kirby;
	public int cameraRange = 6;

	void Start () {
		moveScripte = GetComponent<MoveScript> ();
		moveScripte.enabled = false;
		hasSpawn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasSpawn == false) {
			if ((kirby.position - transform.position).magnitude <= cameraRange) {
				Spwan ();	
			}
		}
	}

	private void Spwan () {
		hasSpawn = true;
		collider2D.enabled = true;
		moveScripte.enabled = true;
	}

	void OnCollisionEnter(Collision col) {
		Debug.Log("collision");
		if (col.gameObject.name.Equals("kirby")) {
			Debug.Log("collision kirby");
		}
	}
}
