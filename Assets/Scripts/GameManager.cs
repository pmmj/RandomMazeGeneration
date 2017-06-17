using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

public class GameManager : MonoBehaviour {
	
	public GameObject WallTile;
	public GameObject FloorTile;
	public GameObject TestingFloor;
	public float dimension;
	public float scale;
	private Vector3 origin = new Vector3 (0, 0, 0);
	public MazeGenerator maze;


	void BuildMaze() {
		int[,] g = maze.GetMaze ();
		int r = g.GetLength (1);
		int c = g.GetLength (0);
		for (int j = 0; j < c; j++) {
			for (int i = 0; i < r; i++) {
				GameObject instance;
				int[] cornerPattern = { -1, 1 };
				int[] wallPattern = { -1, 1, 0, 0 };
				//Debug.Log ("i: " + i);
				//Debug.Log ("j: " + j);
				int rOffset = (r - 1) / 2;

				int cOffset = (c - 1) / 2;
				//Debug.Log (rOffset);
				int rNew = (i - rOffset);
				int cNew = (j - cOffset);
				//Debug.Log (rNew);
				//Debug.Log (cNew);
				/*
				instance = Instantiate(FloorTile, origin + 
					new Vector3(cNew  * (dimension/100) * scale, rNew  * (dimension/100) * scale, 0),
						Quaternion.identity) as GameObject;
				instance.transform.SetParent (this.transform);
				*/

				// NOTE FOR SOME SILLY REASON THE COORDINATES ARE CARTESIAN YET NO MENTION OF THAT IS ANYWHERE

				instance = Instantiate(FloorTile, origin + 
					new Vector3(rNew  * (dimension/100) * scale, -cNew  * (dimension/100) * scale, 0),
					Quaternion.identity) as GameObject;
				instance.transform.SetParent (this.transform);
				// Use testingfloor for testing
				//instance.GetComponent<TextMesh>().text = "" + g [i, j] + "\n(" + i + "," + j + ")";

				for (int corner = 0; corner < 4; corner++) {
					int rCornerOffset = cornerPattern[corner % 2];
					int cCornerOffset = cornerPattern[corner / 2];
					instance = Instantiate(WallTile, origin + 
						new Vector3((rNew  * scale + rCornerOffset)  * (dimension/100), (-cNew * scale + cCornerOffset) * (dimension/100), 0),
						Quaternion.identity) as GameObject;
					instance.transform.SetParent (this.transform);
					//instance.GetComponent<GUIText> ();
				}

				for (int wall = 0; wall < 4; wall++) {
					int cWallOffset = -wallPattern [wall];
					int rWallOffset = wallPattern [wallPattern.Length - wall - 1];
					GameObject tileType;
					//Debug.Log ();
					if ((g [i, j] & (int)Mathf.Pow (2, wall)) != 0) {
						tileType = FloorTile;
					}
						
					else {
						tileType = WallTile;
					}
						
					instance = Instantiate(tileType, origin + 
						new Vector3((rNew  * scale + rWallOffset)  * (dimension/100), (-cNew  * scale + cWallOffset) * (dimension/100), 0),
						Quaternion.identity) as GameObject;
					instance.transform.SetParent (this.transform);
				}




			}
		}

	}

	void Awake() {
		maze = GetComponent<MazeGenerator> ();
		InitGame ();
	}
	// Use this for initialization
	void InitGame() {
		maze.CreateMaze ();
		BuildMaze ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
