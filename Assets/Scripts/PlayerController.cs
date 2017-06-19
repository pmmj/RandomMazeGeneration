using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb2d;

	void Start() {
		rb2d = GetComponent<Rigidbody2D> ();
	}
	// note: physics goes here
	void FixedUpdate() {
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		rb2d.AddForce (new Vector2 (h, v));
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log (other.gameObject.tag);
		if (other.gameObject.CompareTag("Pickups"))	{
			other.GetComponent<PickupController>().GetManager().ReloadMaze ();
			Destroy (other.gameObject);

		}
	}

}
