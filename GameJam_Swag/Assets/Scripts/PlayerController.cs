using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 movementVector;

	public PlayerManager playerManager;

	private int playerId = 1;
	public PlayerManager.PlayerCharacter character;

	public float stunStartTime;
	public float punchResetTime = 0f;

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
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
				 
		this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Body");
		this.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + character.ToString() + "_Head");
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
			switch(pState)
			{
			case playerState.Idle:
				//punch
				if(Time.time > punchResetTime)
				{
					Punch();
				}
				break;
			case playerState.Carrying:
				// drop/throw leaf
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

	private void Punch()
	{
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
	}
}
