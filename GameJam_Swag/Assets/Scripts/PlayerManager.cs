using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour {

	public GameObject playerObject;
	private bool[] activePlayers = new bool[4];
	private GameObject[] playerGameObjects = new GameObject[4];

	private GameManager gameManager;

	public float stunDuration = 1.0f;
	
	public enum PlayerCharacter{
		Loon,
		Beaver,
		Bear,
		Moose
	};

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();

		for(int i=0; i<activePlayers.Length; i++)
		{
			activePlayers[i] = false;
		}
	}

	public void Initiate() {

	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<activePlayers.Length; i++) {
			//if (Input.GetButtonDown (("Start_" + (i+1)).ToString()) && !activePlayers [i]) {
			if (GamePad.GetState(WindowsCheckController(i+1)).Buttons.Start == ButtonState.Pressed && !activePlayers [i]) {
				activePlayers [i] = true;
				//Debug.Log ("START PLAYER " + (i+1));
				GameObject newPlayer = Instantiate (playerObject, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				newPlayer.GetComponent<PlayerController> ().PlayerId = (i+1);
				foreach(Base b in gameManager.bases)
				{
					if(b.playerId == newPlayer.GetComponent<PlayerController> ().PlayerId)
					{
						newPlayer.GetComponent<PlayerController> ().myBase = b;
						break;
					}
				}
				
				newPlayer.transform.position = newPlayer.GetComponent<PlayerController> ().myBase.transform.position;
				newPlayer.GetComponent<PlayerController> ().myBase.transform.GetChild (0).gameObject.SetActive (false);
				switch(i){
				case 0:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Bear;
					break;
				case 1:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Moose;
					break;
				case 2:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Beaver;
					break;
				case 3:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Loon;
					break;
				default:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Bear;
					break;
				}
				newPlayer.GetComponent<PlayerController>().playerManager = this;
				playerGameObjects [i] = newPlayer;
				//this.gameObject.GetComponent<SpawnManager>().CheckStart();
			//} else if (Input.GetButtonDown (("Back_" + (i+1)).ToString()) && activePlayers [i]) {
			} else if (GamePad.GetState(WindowsCheckController(i+1)).Buttons.Back == ButtonState.Pressed && activePlayers [i]) {
				activePlayers [i] = false;

				foreach (PlayerController pc in gameManager.activePlayers) {
					// If the type of the deleted character is in the list of active characters, remove it from active characters
					if(pc.character == playerGameObjects [i].GetComponent<PlayerController>().character) {
						pc.myBase.transform.GetChild (0).gameObject.SetActive (true);
						pc.myBase.resetBase();
						gameManager.activePlayers.Remove(pc);
						break;
					}
				}

				//Debug.Log ("KILL PLAYER " + (i+1));
				Destroy (playerGameObjects [i]);

			}
		}
	}

	// To switch between our player indexes to assign vibration
	public XInputDotNetPure.PlayerIndex WindowsCheckController(int i) {
		switch (i) {
		case(1):
			return PlayerIndex.One;
		case(2):
			return PlayerIndex.Two;
		case(3):
			return PlayerIndex.Three;
		case(4):
			return PlayerIndex.Four;
		default:
			return PlayerIndex.One;
		}
	}
}
