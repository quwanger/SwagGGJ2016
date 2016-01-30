using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;
	public SoundManager soundManager;
	public List<PlayerController> activePlayers = new List<PlayerController>();
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
}
