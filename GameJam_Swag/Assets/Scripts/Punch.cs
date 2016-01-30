﻿using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {

	public float punchStartTime;
	private float punchDuration = 0.3f;
	public float punchRecharge = 2.0f;
	public float punchRechargeCarry = 0.3f;

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
				victim.leafInArms.transform.parent = this.transform.parent;
				victim.leafInArms.transform.position = this.transform.position;
				victim.leafInArms.GetComponent<MapleLeaf>().carrier = this.transform.parent.transform.parent.GetComponent<PlayerController> ();
				this.transform.parent.transform.parent.GetComponent<PlayerController> ().leafInArms = victim.leafInArms;
				victim.Stun();
				EndPunch(false);
			}
		}
		else if(other.gameObject.GetComponent<MapleLeaf> ())
		{
			//pick up maple leaf
			if(other.gameObject.GetComponent<MapleLeaf>().carrier != null)
			{
				other.gameObject.GetComponent<MapleLeaf>().carrier.Stun();
			}
			other.gameObject.GetComponent<Collider2D>().isTrigger = true;
			other.gameObject.transform.parent = this.transform.parent;
			other.gameObject.transform.position = this.transform.position;
			other.gameObject.GetComponent<MapleLeaf>().carrier = this.transform.parent.transform.parent.GetComponent<PlayerController> ();
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().leafInArms = other.gameObject;
			EndPunch(false);
		}
	}

	private void EndPunch(bool toIdle)
	{
		if (toIdle) {
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().pState = PlayerController.playerState.Idle;
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().punchResetTime = (Time.time + punchRecharge);
		} else {
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().pState = PlayerController.playerState.Carrying;
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().punchResetTime = (Time.time + punchRechargeCarry);
		}

		this.gameObject.SetActive(false);
	}
}
