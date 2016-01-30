using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {

	public float punchStartTime;
	private float punchDuration = 0.3f;
	private float punchRecharge = 2.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > (punchStartTime + punchDuration)){
			EndPunch(true);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerController> ()){
			//you punched a player
			PlayerController victim = other.gameObject.GetComponent<PlayerController> ();
			if(victim.pState == PlayerController.playerState.Idle || victim.pState == PlayerController.playerState.Punching)
			{
				victim.Stun();
				EndPunch(true);
			}
			else if(victim.pState == PlayerController.playerState.Carrying || victim.pState == PlayerController.playerState.Throwing)
			{
			}
		}
		//else if(other.gameObject.GetComponent<MapleLeaf> ())
		//{
			//pick up maple leaf
			//change state to carrying
		//}
	}

	void EndPunch(bool toIdle)
	{
		if (toIdle) {
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().pState = PlayerController.playerState.Idle;
		} else {

		}
		this.transform.parent.transform.parent.GetComponent<PlayerController> ().punchResetTime = (Time.time + punchRecharge);
		this.gameObject.SetActive(false);
	}
}
