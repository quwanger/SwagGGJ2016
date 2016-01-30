using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;
	public List<PlayerController> activePlayers = new List<PlayerController>();

	// Use this for initialization
	void Start () {
		playerManager = this.gameObject.GetComponent<PlayerManager>();
		spawnManager = this.gameObject.GetComponent<SpawnManager>();
		Initiate ();
	}

	public void Initiate()
	{
		spawnManager.Initiate ();

		//activePlayers = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
		foreach (PlayerController activePlayer in activePlayers) {
			activePlayer.StartRound();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
