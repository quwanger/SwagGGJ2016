using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;
	public List<PlayerController> activePlayers = new List<PlayerController>();
	public List<Base> bases = new List<Base>();

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

		//activePlayers = FindObjectsOfType(typeof(PlayerController)) as PlayerController[];
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
}
