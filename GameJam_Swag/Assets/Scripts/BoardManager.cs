using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	// Dimensions of floor
	public int columns = 4;
	public int rows = 4;

	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;

	// Hold all out game objects in this transform
	private Transform boardHolder;

	// Track all the possible positions that can be occupied
	private List<Vector3> gridPositions = new List<Vector3>();

	// 
	void InitializeList() {
		// Clear our grid positions List
		gridPositions.Clear ();

		// Add all possible positions on our game board (0,1), (0,0)...etc.
		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3(x, y, 0.0f));
			}
		}
	}

	void Start() {
		//Debug.Log ("DFDFDF");
	}

	// Setup outer wall and floor
	void BoardSetup() {
		boardHolder = new GameObject ("Board").transform;

		// Lay out the tiles
		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				// Storing an index that will correspond to a random tile 
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				// If a border tile instantiate an outer wall instead
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
				}

				// Actually creates our floor tiles are random positions with no rotation
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);
			}
		}
	}

	public void SetupScene() {
		BoardSetup ();
		InitializeList ();
	}
}
