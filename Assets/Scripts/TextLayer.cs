using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayer : MonoBehaviour {

	public string sortingLayer = "Default";
	public int sortingOrder = 0;
	private MeshRenderer renderer;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<MeshRenderer> ();
		renderer.sortingLayerName = sortingLayer;
		renderer.sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
