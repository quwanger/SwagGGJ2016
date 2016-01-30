using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 movementVector;

	public PlayerManager playerManager;
	public GameManager gameManager;

    public Camera mainCamera;
	private int playerId = 1;
	public PlayerManager.PlayerCharacter character;

	public float stunStartTime;
	public float punchResetTime = 0f;

	public GameObject leafInArms = null;

	public List<Color> myColors = new List<Color>();
	public Color activeColor;

	public enum playerState
	{
		Idle,
		Punching,
		Carrying,
		Throwing,
		Stunned
	};

	public playerState pState = playerState.Idle;

	public int PlayerId {
		get {
			return playerId;
		}
		set {
			playerId = value;
		}
	}

	public float movementSpeed = 4f;
	public float dragConstant = 0.09f;

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();

		rb = this.gameObject.GetComponent<Rigidbody2D> ();
				 
		this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Body");
		this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Head");

		mainCamera = FindObjectOfType<Camera> ();

		StartRound ();
	}

	public void StartRound()
	{
		gameManager.activePlayers.Add (this);

		myColors.Clear ();
		SpawnManager sm = playerManager.gameObject.GetComponent<SpawnManager> ();
		for (int i=0; i<sm.sequenceCount; i++){
			myColors.Add(sm.possibleColors[Random.Range(0, sm.colorCount)]);
		}
		activeColor = myColors [0];

		FindObjectOfType<SpawnManager> ().CheckStart ();
	}
	
	// Update is called once per frame
	void Update () {

//----- HANDLES PLAYER MOVEMENT
		if (pState != playerState.Stunned) {
			movementVector.x = Input.GetAxis (("LeftJoystickX" + playerId).ToString ()) * movementSpeed;
			movementVector.y = Input.GetAxis (("LeftJoystickY" + playerId).ToString ()) * movementSpeed * -1;

			//Debug.Log (playerId + " " + Input.GetAxis (("RightTrigger" + playerId).ToString ()));

			this.gameObject.transform.GetChild (0).gameObject.transform.localEulerAngles = new Vector3 (0, 0, Mathf.Rad2Deg * (Mathf.Atan2 (movementVector.y, movementVector.x)) - 270f);

			movementVector.x = Mathf.Lerp (rb.velocity.x, movementVector.x, dragConstant);
			movementVector.y = Mathf.Lerp (rb.velocity.y, movementVector.y, dragConstant);

			rb.velocity = movementVector;
			this.gameObject.transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
		} else {
			if(Time.time > (stunStartTime + playerManager.stunDuration))
			{
				pState = playerState.Idle;
			}
		}

//----- HANDLES PLAYER ACTION
		if(Input.GetAxis (("RightTrigger" + playerId).ToString()) > 0f)
		{
			if(Time.time > punchResetTime)
			{
				switch(pState)
				{
				case playerState.Idle:
					//punch
					Punch();
					break;
				case playerState.Carrying:
					// drop/throw leaf
					leafInArms.transform.parent = transform.parent;
					leafInArms.GetComponent<Collider2D>().isTrigger = false;
					leafInArms.GetComponent<MapleLeaf>().carrier = null;
					leafInArms = null;
					pState = playerState.Idle;
					punchResetTime = (Time.time + this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Punch> ().punchRecharge);
					break;
				case playerState.Punching:
					//nothing
					break;
				case playerState.Stunned:
					//nothing
					break;
				case playerState.Throwing:
					//nothing
					break;
				}
			}
		}
	}

	private void Punch()
	{
		//mainCamera.GetComponent<CameraShake> ().Shake ();
		//mainCamera.GetComponent<CameraZoom> ().ZoomIn (this.gameObject);
		pState = playerState.Punching;
		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.SetActive (true);
		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Punch> ().punchStartTime = Time.time;
		Debug.Log ("Punch!");
	}

	public void Stun()
	{
		Debug.Log ("Stunned!");
		pState = playerState.Stunned;
		stunStartTime = Time.time;
		leafInArms = null;
	}
}
