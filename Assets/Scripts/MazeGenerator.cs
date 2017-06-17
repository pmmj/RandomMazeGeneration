using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Completed

{

	public class MazeGenerator : MonoBehaviour {

		public int rows;
		public int columns;

		private int[] DIRECTIONS = {1, 2, 4, 8}; // N S E W
		private int[] OPPOSITE_DIRECTIONS = {2, 1, 8, 4};
		private int[] DX = {0, 0, 1, -1};
		private int[] DY = { -1, 1, 0, 0 };

		private int[,] grid;



		void CarveMaze( int x, int y, int[,] g) {
			int[] order = newOrder(DIRECTIONS);

			for (int i = 0; i < order.Length; i++) {
				int newx = x + DX [order[i]];
				int newy = y + DY [order[i]];
				//Debug.Log ("Carving!");
				if ((newy >= 0 && newy < rows) && (newx >= 0 && newx < columns) && g [newx, newy] == 0) {
					g [x, y] |= DIRECTIONS [order [i]];
					//Debug.Log ("x: " + x + ", y: " + y);
					g [newx, newy] |= OPPOSITE_DIRECTIONS [order [i]];
					CarveMaze (newx, newy, g);
				}
			}
		}

		int[] newOrder(int[] s) {
			int[] a = new int[s.Length];
			string d = "";
			for (int i = 0; i < s.Length; i++) {
				a [i] = i;
			}
			//int[] b = new int[a.Length]; 
			for (int i = a.Length - 1; i > 0; i--) {
				int j = Random.Range (0, i + 1);
				int temp = a [j];
				a [j] = a [i];
				a [i] = temp;
			}
			for (int i = 0; i < a.Length; i++) {
				d += a [i] + " ";
			}
			Debug.Log ("newOrder is " + d);
			return a;

		}

		public int[,] GetMaze() {
			return grid;
		}

		public void CreateMaze(){
			grid = new int[rows, columns];
			CarveMaze(0, 0, grid);
			string g = "";
			//Debug.Log ("gL 0 " + grid.GetLength (0));
			//Debug.Log ("gL 1 " + grid.GetLength (1));
			for (int i = 0; i < grid.GetLength(0); i++) {
				for (int j = 0; j < grid.GetLength(1); j++) {
					g += grid[j,i] + " ";
				}
				g += "\n";
				
			}
			Debug.Log (g);
		}
	}
}
