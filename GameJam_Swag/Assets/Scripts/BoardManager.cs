using UnityEngine;
using System; // To use Serializable. Can convert variables into text for example
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count(int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	// Dimensions of floor
	public int columns = 4;
	public int rows = 4;

	public GameObject[] floorTiles;
	public GameObject[] outerWallTiles;

	// Hold all out game objects in this transform
	private Transform floorHolder;

	// Track all the possible positions that can be occupied
	private List<Vector3> gridPositions = new List<Vector3>();

	// 
	void InitializeList() {
		// Clear our grid positions List
		gridPositions.Clear ();

		// Add all possible positions on our game board (0,1), (0,0)...etc.
		for (int x = 1; x < columns; x++) {
			for (int y = 1; y < columns; y++) {
				gridPositions.Add (new Vector3(x, y, 0.0f));
			}
		}
	}

	// Setup outer wall and floor
	void FloorSetup() {

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
