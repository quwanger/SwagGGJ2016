using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;
	public SoundManager soundManager;
	public List<PlayerController> activePlayers = new List<PlayerController>();
	public List<Base> bases = new List<Base>();
	public Camera mainCamera;

	// For sound
	public enum SoundType {
		bear,
		bearGod,
		moose,
		mooseGod,
		loon,
		loonGod,
		beaver,
		beaverGod,
		grab,
		throwStart,
		throwEnd,
		powerPickup,
		powerRelease,
		leafNo,
		leafYes
	};

	// Use this for initialization
	void Start () {
		// Setup audio listener
		AudioListener.volume = 1.0f;

		//Initalize our game objects
		mainCamera = FindObjectOfType<Camera> ();
		playerManager = this.gameObject.GetComponent<PlayerManager>();
		spawnManager = this.gameObject.GetComponent<SpawnManager>();
		soundManager = this.GetComponent<SoundManager>();

		Initiate ();
	}

	public void Initiate() {
		// Spawn in our players
		spawnManager.Initiate ();

		// Activate the game
		foreach (PlayerController activePlayer in activePlayers) {
			activePlayer.StartRound();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void DeclareWinner(PlayerController winner)
	{
		Debug.Log (winner.character.ToString() + " has won!");
	}

	public void ResetRound() {
		foreach(Base playerBase in bases) {
			playerBase.resetBase();
		}

		// Remove all maple leafs
		foreach (GameObject leaf in spawnManager.leavesOnMap) {
			Destroy(leaf);
		}

		spawnManager.leavesOnMap = new List<GameObject> ();

		// Respawn players at home
		foreach (PlayerController pc in activePlayers) {
			pc.transform.position = pc.GetComponent<PlayerController> ().myBase.transform.position;
		}


		
		//spawnManager.gameHasStarted = false;

		Initiate();
	}
}
