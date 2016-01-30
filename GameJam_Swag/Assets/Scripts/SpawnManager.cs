using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	public const int totalColors = 7;
	public const int minColors = 2;
	public const int maxSequence = 7;
	public const int minSequence = 3;

	public GameObject mapleLeaf;

	public int colorCount;
	public int sequenceCount;

	public bool gameHasStarted = false;

	public Color[] possibleColors = new Color[7];

	public List<GameObject> leavesOnMap = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}

	public void Initiate()
	{
		possibleColors [0] = new Color (1f, 0, 0);
		possibleColors [1] = new Color (1f, 0.5f, 0);
		possibleColors [2] = new Color (1f, 1f, 0);
		possibleColors [3] = new Color (0, 1f, 0);
		possibleColors [4] = new Color (0, 1f, 1f);
		possibleColors [5] = new Color (0, 0, 1f);
		possibleColors [6] = new Color (0.5f, 0, 1f);
		
		BeginRound ();
		CheckStart ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CheckStart()
	{
		if (this.gameObject.GetComponent<PlayerManager> ().playerCount > 1) {
			StartGame ();
		}
	}

	private void BeginRound()
	{
		colorCount = Random.Range (minColors, totalColors);
		sequenceCount = Random.Range (minSequence, maxSequence);
	}

	private void StartGame()
	{
		gameHasStarted = true;
		Debug.Log ("GAME HAS STARTED");

		for (int i=0; i<this.gameObject.GetComponent<PlayerManager>().playerCount+1; i++){
			GameObject leaf = SpawnLeaf();
			leaf.GetComponent<MapleLeaf>().leafColor = possibleColors[Random.Range(0, colorCount)];
			leavesOnMap.Add(leaf);
		}
	}

	private GameObject SpawnLeaf()
	{
		Vector3 position = new Vector3 (Random.Range (-4F, 4F), Random.Range (-4F, 4F), 0);
		return Instantiate (mapleLeaf, position, Quaternion.identity) as GameObject;
	}
}
