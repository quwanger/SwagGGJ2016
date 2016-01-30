using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public GameObject playerObject;
	private bool[] activePlayers = new bool[4];
	private GameObject[] playerGameObjects = new GameObject[4];

	public float stunDuration = 2.0f;

	public int playerCount = 0;

	public enum PlayerCharacter{
		Loon,
		Beaver,
		Bear,
		Moose
	};

	// Use this for initialization
	void Start () {
		for(int i=0; i<activePlayers.Length; i++)
		{
			activePlayers[i] = false;
		}
	}

	public void Initiate()
	{

	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<activePlayers.Length; i++) {
			if (Input.GetButtonDown (("Start" + (i+1)).ToString()) && !activePlayers [i]) {
				activePlayers [i] = true;
				Debug.Log ("START PLAYER " + (i+1));
				GameObject newPlayer = Instantiate (playerObject, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				newPlayer.GetComponent<PlayerController> ().PlayerId = (i+1);
				switch(i){
				case 0:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Bear;
					break;
				case 1:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Beaver;
					break;
				case 2:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Loon;
					break;
				case 3:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Moose;
					break;
				default:
					newPlayer.GetComponent<PlayerController>().character = PlayerCharacter.Bear;
					break;
				}
				newPlayer.GetComponent<PlayerController>().playerManager = this;
				playerGameObjects [i] = newPlayer;
				playerCount++;
				//this.gameObject.GetComponent<SpawnManager>().CheckStart();
			} else if (Input.GetButtonDown (("Select" + (i+1)).ToString()) && activePlayers [i]) {
				activePlayers [i] = false;
				Debug.Log ("KILL PLAYER " + (i+1));
				Destroy (playerGameObjects [i]);
				playerCount--;
			}
		}
	}
}
