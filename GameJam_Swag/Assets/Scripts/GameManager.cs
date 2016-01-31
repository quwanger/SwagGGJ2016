using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public PlayerManager playerManager;
	public SpawnManager spawnManager;
	public SoundManager soundManager;
	public List<PlayerController> activePlayers = new List<PlayerController>();
	public List<Base> bases = new List<Base>();
	public List<GameObject> maps = new List<GameObject>();
	public Camera mainCamera;

	public const int totalColors = 7;
	public const int minColors = 2;
	public const int maxSequence = 7;
	public const int minSequence = 3;

	private float godModeDuration = 10.0f;
	private float godModeStartTime;
	public bool inGodMode = false;
	private PlayerController currentGod;
	private float godScale = 3.0f;
	private Vector3 originalScale = new Vector3(0.267742f, 0.267742f, 0.267742f);
	private GameObject currentMap;

	private GameObject gameEndMessage = null;

	public int colorCount;
	public int sequenceCount;
	
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
		// Resets gem requirements
		// Check if there's more than 1 player in the game
		// Check if the game is currently set to started
		// Mark the game as started
		// Add Leaves to map

		if (currentMap != null) {
			Destroy(currentMap);
		}

		currentMap = Instantiate<GameObject> (maps[Random.Range(0, maps.Count)]);

		InitializeGemConfiguration ();

		// Activate the game
		foreach (PlayerController activePlayer in activePlayers) {
			activePlayer.StartRound();
		}

		spawnManager.Initiate ();
	}

	// Update is called once per frame
	void Update () {
		if (inGodMode) {
			Camera.main.GetComponent<CameraShake>().Shake();

			float tempScale = Mathf.Lerp(currentGod.transform.localScale.x, godScale, Time.deltaTime);
			currentGod.transform.localScale = new Vector3 (tempScale, tempScale, 1f);

			if(Time.time > (godModeStartTime + godModeDuration))
			{
				currentGod.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + currentGod.character.ToString() + "_Body");
				currentGod.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + currentGod.character.ToString() + "_Head");
				currentGod.gameObject.transform.localScale = new Vector3(0.267742f, 0.267742f, 1f);
				inGodMode = false;
				currentGod = null;
				ResetRound();
			}
		}
	}

	public void InitializeGemConfiguration () {
		spawnManager.possibleColors [0] = new Color (255f/255f, 44f/255f, 0f/255f);
		spawnManager.possibleColors [1] = new Color (0f/255f, 246f/255f, 255f/255f);
		spawnManager.possibleColors [2] = new Color (255f/255f, 252f/255f, 0f/255f);
		spawnManager.possibleColors [3] = new Color (0f/255f, 252f/255f, 0f/255f);
		spawnManager.possibleColors [4] = new Color (196f/255f, 0f/255f, 252f/255f);
		spawnManager.possibleColors [5] = new Color (255f/255f, 175f/255f, 218f/255f);
		spawnManager.possibleColors [6] = new Color (255f/255f, 146f/255f, 0f/255f);
		
		colorCount = Random.Range (minColors, totalColors);
		sequenceCount = Random.Range (minSequence, maxSequence);
	}

	public void DeclareWinner(PlayerController winner)
	{
		currentGod = winner;
		Debug.Log (winner.character.ToString() + " has won!");
		gameEndMessage = Instantiate<GameObject> (Resources.Load<GameObject> ("Prefabs/GameStart"));
		gameEndMessage.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/victory_" + currentGod.character.ToString ()) as Sprite;
		godModeStartTime = Time.time;
		inGodMode = true;
		StartGodMode (currentGod);
	}

	public void StartGodMode(PlayerController god){
		god.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + god.character.ToString() + "_God_Body");
		god.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + god.character.ToString() + "_God_Head");

		BoxCollider2D[] collisionObjects = FindObjectsOfType<BoxCollider2D> ();
		foreach(BoxCollider2D bc in collisionObjects)
		{
			Rigidbody2D rb = bc.gameObject.AddComponent<Rigidbody2D>();
			rb.gravityScale = 0;
		}
	}

	public void ResetRound() {

		if (gameEndMessage != null) {
			Destroy(gameEndMessage);
		}

		// Reset bases
		foreach(Base playerBase in bases) {
			playerBase.resetBase();
		}

		MapleLeaf[] leaves = GameObject.FindObjectsOfType<MapleLeaf>();
		foreach (MapleLeaf go in leaves) {
			go.captor = null;
			go.carrier = null;
			Destroy(go.gameObject);
		}

		// Set the game to finished
		spawnManager.gameHasStarted = false;

		// Remove all maple leafs
		foreach (GameObject leaf in spawnManager.leavesOnMap) {
			Destroy(leaf);
		}

		// Set leaves on map to empty
		spawnManager.leavesOnMap = new List<GameObject> ();

		// Respawn players at home
		foreach (PlayerController pc in activePlayers) {
			pc.transform.position = pc.GetComponent<PlayerController> ().myBase.transform.position;
			if(pc.pState == PlayerController.playerState.Throwing)
				pc.CancelThrow();
			pc.pState = PlayerController.playerState.Idle;
			pc.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
		}

		Camera.main.transform.position = new Vector3(0, 0, -10f);
		Camera.main.transform.eulerAngles = new Vector3(0, 0, 0);

		Initiate();
	}
}
