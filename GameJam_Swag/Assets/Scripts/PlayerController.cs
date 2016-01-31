using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour {

	private const float zoomConstant = 2.5f;

	private Rigidbody2D rb;
	private Vector2 movementVector;

	public PlayerManager playerManager;
	public GameManager gameManager;

    public Camera mainCamera;
	private int playerId = 1;
	public PlayerManager.PlayerCharacter character;

	public SpriteRenderer playerShadow;
	public Transform throwLine;
	public Transform target;

	public float stunStartTime;
	private float throwStartTime;
	private float timeBeforeThrowBegins = 0.2f;
	public float punchResetTime = 0f;

	public GameObject leafInArms = null;

	public List<Color> myColors = new List<Color>();
	public Color activeColor;

	public Base myBase;
	public int currentGemIndex;

	private bool startThrow = false;

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

	//For vibration
	public float vibrationIntensity = 0.0f;

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();

		rb = this.gameObject.GetComponent<Rigidbody2D> ();

		playerShadow = transform.FindChild ("Silhouette").GetComponent<SpriteRenderer> ();
		throwLine = transform.FindChild ("Body").transform.FindChild ("Aim").transform;
		target = transform.FindChild ("Body").transform.FindChild ("Target").transform;

		this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Body");
		this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Head");

		mainCamera = FindObjectOfType<Camera> ();

		gameManager.activePlayers.Add (this);

		gameManager.spawnManager.CheckStart ();

		if (gameManager.spawnManager.gameHasStarted) {
			StartRound ();
			if(gameManager.spawnManager.leavesOnMap.Count < (gameManager.activePlayers.Count + 1))
			{
				gameManager.spawnManager.SpawnLeaf();
			}
		}
	}

	public void StartRound()
	{
		myColors.Clear ();
		SpawnManager sm = playerManager.gameObject.GetComponent<SpawnManager> ();
		for (int i=0; i<gameManager.sequenceCount; i++){
			Color tempColor = sm.possibleColors[Random.Range(0, gameManager.colorCount)];
			myColors.Add(tempColor);
			myBase.gems[i].SetActive(true);
			myBase.gems[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = tempColor;
			currentGemIndex = 0;
			if(i==0)
			{
				myBase.gems[i].GetComponent<Gem>().ActivateGem();
			}
		}
		activeColor = myColors [0];
		playerShadow.color = activeColor;
		FindObjectOfType<SpawnManager> ().CheckStart ();
	}
	
	// Update is called once per frame
	void Update () {
		// Set vibration intensity to decrease by half until it reaches 0
		if (vibrationIntensity > 1){
			vibrationIntensity /= 2;
		} else if(vibrationIntensity < 1){
			vibrationIntensity = 0;
		}

		if (gameManager.spawnManager.gameHasStarted) {
//----- HANDLES PLAYER MOVEMENT

			float tempSpeed = movementSpeed;

			if (pState == playerState.Idle || pState == playerState.Carrying) {
				GamePad.SetVibration (WindowsCheckControllerToVibrate (), 0, 0);
			}

			if (pState == playerState.Carrying || pState == playerState.Throwing) {
				tempSpeed *= 0.9f;
			}

			if (pState != playerState.Stunned) {
				movementVector.x = Input.GetAxis (("LeftJoystickX" + playerId).ToString ()) * tempSpeed;
				movementVector.y = Input.GetAxis (("LeftJoystickY" + playerId).ToString ()) * tempSpeed * -1;

				//Debug.Log (playerId + " " + Input.GetAxis (("RightTrigger" + playerId).ToString ()));

				this.gameObject.transform.GetChild (0).gameObject.transform.localEulerAngles = new Vector3 (0, 0, Mathf.Rad2Deg * (Mathf.Atan2 (movementVector.y, movementVector.x)) - 270f);

				movementVector.x = Mathf.Lerp (rb.velocity.x, movementVector.x, dragConstant);
				movementVector.y = Mathf.Lerp (rb.velocity.y, movementVector.y, dragConstant);

				rb.velocity = movementVector;
				this.gameObject.transform.localEulerAngles = new Vector3 (0f, 0f, 0f);
			} else {
				vibrationIntensity = 100.0f;

				GamePad.SetVibration (WindowsCheckControllerToVibrate (), vibrationIntensity, vibrationIntensity);

				if (Time.time > (stunStartTime + playerManager.stunDuration)) {
					pState = playerState.Idle;

					GamePad.SetVibration (WindowsCheckControllerToVibrate (), 0, 0);
				}
			}

//----- HANDLES PLAYER ACTION
			if(!gameManager.inGodMode)
			{
				if (Input.GetAxis (("RightTrigger" + playerId).ToString ()) > 0f) {
					// Actual vibration
					GamePad.SetVibration (WindowsCheckControllerToVibrate (), vibrationIntensity, vibrationIntensity);

					if (Time.time > punchResetTime) {
						switch (pState) {
						case playerState.Idle:
						//punch
							Punch ();
							break;
						case playerState.Carrying:
							pState = playerState.Throwing;
							startThrow = true;
							throwStartTime = Time.time;
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
				} else if (startThrow) {
					if (target.gameObject.activeSelf) {
						ThrowLeaf (target.position);
					} else {
						ThrowLeaf ();
					}
				}

				if (startThrow && (Time.time > (throwStartTime + timeBeforeThrowBegins))) {
					throwLine.gameObject.SetActive (true);
					target.gameObject.SetActive (true);
				
					throwLine.localScale = new Vector3 (throwLine.localScale.x, throwLine.localScale.y + 0.1f, throwLine.localScale.z);
					target.localPosition = new Vector3 (0f, -3.4f * throwLine.localScale.y, -10f);
				}
			}
		}
	}

	public void ThrowLeaf(Vector3 targetPosition=default(Vector3))
	{
		startThrow = false;

		Debug.Log (transform.parent);
		leafInArms.transform.parent = null;

		leafInArms.GetComponent<Collider2D> ().isTrigger = false;
		leafInArms.GetComponent<MapleLeaf> ().carrier = null;
		if (targetPosition != default(Vector3)) {
			//leafInArms.transform.position = targetPosition;
			leafInArms.GetComponent<Rigidbody2D> ().isKinematic = false;
			leafInArms.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (targetPosition.x - transform.position.x, targetPosition.y - transform.position.y).normalized * (200f * throwLine.localScale.y));
			leafInArms.GetComponent<MapleLeaf> ().isBeingThrown = true;
			leafInArms.GetComponent<MapleLeaf>().thrower = this;
		}
		leafInArms = null;
		pState = playerState.Idle;
		punchResetTime = (Time.time + this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Punch> ().punchRecharge);
	
		throwLine.localScale = new Vector3 (throwLine.localScale.x, 0f, throwLine.localScale.z);
		target.localPosition = new Vector3 (0f, 0f, -10f);
		throwLine.gameObject.SetActive (false);
		target.gameObject.SetActive (false);
	}

	public void CancelThrow()
	{
		startThrow = false;

		leafInArms = null;

		throwLine.localScale = new Vector3(throwLine.localScale.x, 0f, throwLine.localScale.z);
		target.localPosition = new Vector3(0f, 0f, -10f);
		throwLine.gameObject.SetActive(false);
		target.gameObject.SetActive(false);
	}
	
	private void Punch()
	{
		Debug.Log ("Punch!");

		// Set state to punching
		pState = playerState.Punching;

		bool zoom = false;

		foreach (PlayerController player in gameManager.activePlayers) {
			if(player != this)
			{
				if(Vector3.Distance(this.gameObject.transform.position, player.gameObject.transform.position) < zoomConstant)
				{
					if(player.pState == playerState.Carrying || player.pState == playerState.Throwing)
					{
						Camera.main.GetComponent<CameraZoom>().ZoomIn(this.gameObject);
						zoom = true;
						break;
					}
				}
			}
		}

		if (!zoom) {
			Camera.main.GetComponent<CameraShake>().Shake();
		}

		// Set vibration intensity to a 
		vibrationIntensity = 200.0f;

		// Play correct sound for correct player
		switch (playerId) {
		case 1:
			gameManager.soundManager.PlaySound (GameManager.SoundType.bear);
			break;
		case 2:
			gameManager.soundManager.PlaySound (GameManager.SoundType.moose);
			break;
		case 3:
			gameManager.soundManager.PlaySound (GameManager.SoundType.beaver);
			break;
		case 4:
			gameManager.soundManager.PlaySound (GameManager.SoundType.loon);
			break;
		}

		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.SetActive (true);
		this.gameObject.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + character.ToString () + "_Body_Punch") as Sprite;
		this.gameObject.transform.GetChild (0).transform.GetChild (0).gameObject.GetComponent<Punch> ().punchStartTime = Time.time;
	}

	public void Stun()
	{
		Debug.Log ("Stunned!");
	
		GameObject starPart = Instantiate (Resources.Load<GameObject> ("Prefabs/StarParticle"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),Quaternion.identity) as GameObject;
		starPart.transform.parent = this.transform;
		Destroy (starPart, 3);

		// Set state to punching
		pState = playerState.Stunned;

		stunStartTime = Time.time;
		leafInArms = null;
	}

	// To switch between our player indexes to assign vibration
	public XInputDotNetPure.PlayerIndex WindowsCheckControllerToVibrate() {
		switch (playerId) {
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
