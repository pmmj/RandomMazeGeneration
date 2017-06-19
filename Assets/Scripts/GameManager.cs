using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Completed;

public class GameManager : MonoBehaviour {
	
	public GameObject WallTile;
	public GameObject FloorTile;
	public GameObject TestingFloor;
	public GameObject Player;
	public GameObject Pickup;

	public Sprite[] FloorTileSprites;

	public float dimension;
	public float scale;
	public int startingSize;
	public int score;
	private int size;
	private Vector3 origin = new Vector3 (0, 0, 0);
	public MazeGenerator maze;


	void BuildMaze() {
		int[,] g = maze.GetMaze ();
		int r = g.GetLength (0);
		int c = g.GetLength (1);
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
				// for lack of a better rewrite, yes x coords are in row changes, and y coords are in column changes. sorry
				// any position is scaled by r, -c because the grid increases in x to the right and y down, which differs from the Cartesian plane.

				instance = Instantiate(FloorTile, origin + 
					new Vector3(rNew  * (dimension/100) * scale, -cNew  * (dimension/100) * scale, 0),
					Quaternion.identity) as GameObject;
				instance.transform.SetParent (this.transform);
				instance.GetComponent<SpriteRenderer> ().sprite = FloorTileSprites [g [i, j]];
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
					if (tileType == FloorTile) {
						int mask = 3 & (g [i, j] & (int)Mathf.Pow (2, wall));
						if (mask != 0)
							mask = 3;
						else
							mask = 12;
						instance.GetComponent<SpriteRenderer> ().sprite = FloorTileSprites [mask];
					}
				}




			}
		}

	}

	public void ReloadMaze() {
		//int size = startingSize;

		List<GameObject> children = new List<GameObject>();
		foreach (Transform child in transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));
		maze.rows = size;
		maze.columns = size;
		int x = Random.Range(0, size);
		int y = Random.Range(0, size);
		maze.CreateMaze (x, y);
		BuildMaze ();
		List<int[]> ends = maze.GetDeadEnds ();

		int pickupIndex = Random.Range (0, ends.Count);
		int playerIndex = Random.Range (0, ends.Count);
		if (playerIndex == pickupIndex) {
			playerIndex = (playerIndex + 1) % ends.Count;
		}
		int[] pickupEnd = ends [pickupIndex];
		int[] playerEnd = ends [playerIndex];
		int rOffset = (maze.rows - 1) / 2;
		int cOffset = (maze.columns - 1) / 2;
		//Debug.Log (rOffset);
		int rNew = (pickupEnd[0] - rOffset);
		int cNew = (pickupEnd[1] - cOffset);
		GameObject instance = Instantiate(Pickup, origin + 
			new Vector3(rNew  * (dimension/100) * scale, -cNew  * (dimension/100) * scale, 0),
			Quaternion.identity) as GameObject;
		instance.transform.SetParent (this.transform);
		instance.GetComponent<PickupController>().SetManager(this);
		rNew = (playerEnd[0] - rOffset);
		cNew = (playerEnd[1] - cOffset);
		Player.transform.position = new Vector3(rNew  * (dimension/100) * scale, -cNew  * (dimension/100) * scale, 0);


		size += 2;

	}

	void Awake() {
		maze = GetComponent<MazeGenerator> ();
		InitGame ();
	}
	// Use this for initialization
	void InitGame() {
		size = startingSize;
		ReloadMaze ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
