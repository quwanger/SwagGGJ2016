using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;

	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = FindObjectOfType<Camera> ();
		playerManager = this.gameObject.GetComponent<PlayerManager>();
		spawnManager = this.gameObject.GetComponent<SpawnManager>();
		Initiate ();
	}

	public void Initiate()
	{
		spawnManager.Initiate ();

		PlayerController[] activePlayers = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
		foreach (PlayerController activePlayer in activePlayers) {
			activePlayer.StartRound();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
