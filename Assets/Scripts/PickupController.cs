using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {
	private GameManager manager;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	}

	public GameManager GetManager() {
		return manager;
	}

	public void SetManager(GameManager m) {
		manager = m;
	}
}
