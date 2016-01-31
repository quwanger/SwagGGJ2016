using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public GameManager gameManager;

	public GameObject mapleLeaf;

	public bool gameHasStarted = false;

	public Color[] possibleColors = new Color[7];

	public List<GameObject> leavesOnMap = new List<GameObject>();

	private float countdownDuration = 3.0f;
	public float countdownStartTime;
	public bool countdownStart = false;

	private GameObject gameStartMessage = null;
	private GameObject gameCountdown01 = null;
	private GameObject gameCountdown02 = null;
	private GameObject gameCountdown03 = null;

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
		if (!gameHasStarted) {
			if (countdownStart) {
				//Debug.Log((countdownStartTime + countdownDuration) - Time.time);

				if ((countdownStartTime + countdownDuration) - Time.time > 2.0f) {
					Destroy (gameCountdown03);
					gameCountdown03 = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Countdown03"));
				} else if ((countdownStartTime + countdownDuration) - Time.time > 1.0f) {
					Destroy (gameCountdown03);
					Destroy (gameCountdown02);
					gameCountdown03 = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Countdown02"));
				} else if ((countdownStartTime + countdownDuration) - Time.time > 0.0f) {
					Destroy (gameCountdown03);
					Destroy (gameCountdown02);
					Destroy (gameCountdown01);
					gameCountdown03 = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/Countdown01"));
				}

				if (Time.time > (countdownStartTime + countdownDuration)) {
					countdownStart = false;
					Destroy(gameStartMessage);
					Destroy (gameCountdown03);
					Destroy (gameCountdown02);
					Destroy (gameCountdown01);

					GameObject starPart = Instantiate (Resources.Load<GameObject> ("Prefabs/StarParticleBig"), new Vector3(0, 0, 0),Quaternion.identity) as GameObject;
					Camera.main.GetComponent<CameraShake>().Shake();
					Destroy (starPart, 3);

					GameObject go = Instantiate (Resources.Load<GameObject> ("Prefabs/Go"), new Vector3(0, 0, -18f),Quaternion.identity) as GameObject;
					go.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Sprites/text_go" + Random.Range(1,3).ToString()) as Sprite;
					Destroy (go, 0.75f);

					gameManager.soundManager.PlaySound (GameManager.SoundType.intro);

					StartGame ();
				}
			}
		}

		if (gameManager.activePlayers.Count < 2 && countdownStart) {
			CancelCountdown();	
		}
	}

	// Start game if there's more than 1 player
	public void CheckStart()
	{
		if (gameManager.activePlayers.Count > 1 && !countdownStart) {
			StartCountdown();
		}
	}

	//cancel countdown if someone leaves before countdown ends
	public void CancelCountdown()
	{
		countdownStart = false;
		Destroy(gameStartMessage);
		Destroy (gameCountdown03);
		Destroy (gameCountdown02);
		Destroy (gameCountdown01);
	}

	public void StartCountdown()
	{
		Destroy (gameStartMessage);

		if (!gameHasStarted && !countdownStart) {
			gameStartMessage = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/GameStart"));
		}

		//gameStartMessage = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/GameStart"));
		if (!gameHasStarted) {
			Debug.Log ("PLAYING SOUND");
			gameManager.soundManager.PlaySound (GameManager.SoundType.countDown);
		}

		countdownStart = true;
		countdownStartTime = Time.time;
	}

	private void StartGame()
	{
		gameHasStarted = true;

		Debug.Log ("GAME HAS STARTED");

		foreach (PlayerController player in gameManager.activePlayers) {
			player.StartRound();
		}

		// Create leaves and spawn them on the map with a color

		for (int i = 0; i < gameManager.activePlayers.Count + 2; i++){
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
