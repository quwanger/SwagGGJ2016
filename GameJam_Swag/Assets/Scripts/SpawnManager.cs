using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public GameManager gameManager;

	public GameObject mapleLeaf;

	public bool gameHasStarted = false;

	public Color[] possibleColors = new Color[7];

	public List<GameObject> leavesOnMap = new List<GameObject>();

	// Use this for initialization
	void Start () {
		gameManager = this.gameObject.transform.GetComponent<GameManager> ();
	}

	// Initialize possible colors array
	public void Initiate()
	{
		// Start game if there's more than 1 player 
		CheckStart ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Start game if there's more than 1 player
	public void CheckStart()
	{
		if (gameManager.activePlayers.Count >= 1 && !gameHasStarted) {
			StartGame ();
		}
	}

	private void StartGame()
	{
		gameHasStarted = true;
		Debug.Log ("GAME HAS STARTED");

		// Create leaves and spawn them on the map with a color
		for (int i = 0; i < gameManager.activePlayers.Count + 1; i++){
			SpawnLeaf();
		}
	}

	// Creates an instance of leaf with a spawn position
	public void SpawnLeaf()
	{
		List<Color> activeColors = new List<Color>();

		foreach (PlayerController pc in gameManager.activePlayers) {
			activeColors.Add(pc.activeColor);
		}

		// Spawn a leaf at a random RespawnPoint
		List<GameObject> leafSpawnPoints = new List<GameObject> ();
		leafSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("Respawn"));
		List<GameObject> validSpawnPoints = new List<GameObject> ();

		// Prevent a spawn on same point
		if (leavesOnMap.Count > 0) {
			foreach (GameObject tempSpawnPoint in leafSpawnPoints) {
				// go through 
				bool validSpawnPoint = true;
				foreach (GameObject tempLeaf in leavesOnMap) {
					if(Vector3.Distance(tempLeaf.transform.position, tempSpawnPoint.transform.position) < 1.0f)
					{
						validSpawnPoint = false;
						break;
					}
				}

				if(validSpawnPoint) {
					validSpawnPoints.Add (tempSpawnPoint);
				}
			}
		} else {
			validSpawnPoints = leafSpawnPoints;
		}

		if(validSpawnPoints.Count < 1) {
			Debug.Log("All Spawn points are currently crowded");
			return;
		}

		// Get a random position from the list of possible spawn points
		GameObject spawnPoint = validSpawnPoints [Random.Range (0, validSpawnPoints.Count)];
		Vector3 position = spawnPoint.transform.position;

		GameObject leaf = Instantiate (mapleLeaf, position, Quaternion.identity) as GameObject;

		leaf.GetComponent<MapleLeaf>().leafColor = activeColors[Random.Range(0, activeColors.Count)];

		leavesOnMap.Add(leaf);
	}
}
