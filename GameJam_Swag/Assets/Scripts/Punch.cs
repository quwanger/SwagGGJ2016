using UnityEngine;
using System.Collections;

public class Punch : MonoBehaviour {

	public float punchStartTime;
	private float punchDuration = 0.3f;
	public float punchRecharge = 0.5f;
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
			else if(other.gameObject.GetComponent<MapleLeaf>().captor != null)
			{
				PlayerController tempPlayer = other.gameObject.GetComponent<MapleLeaf>().captor;
				tempPlayer.currentGemIndex--;
				tempPlayer.activeColor = tempPlayer.myColors[tempPlayer.currentGemIndex];
				tempPlayer.playerShadow.color = tempPlayer.activeColor;
				transform.parent.parent.GetComponent<PlayerController>().gameManager.bases[tempPlayer.PlayerId-1].gems[tempPlayer.currentGemIndex+1].GetComponent<Gem>().StolenGem();
				transform.parent.parent.GetComponent<PlayerController>().gameManager.bases[tempPlayer.PlayerId-1].gems[tempPlayer.currentGemIndex].GetComponent<Gem>().ReactivateGem();
				transform.parent.parent.GetComponent<PlayerController>().gameManager.bases[tempPlayer.PlayerId-1].vulnerableGem = null;
				Debug.Log ("Who's base? " + tempPlayer.character.ToString() + " , What base? " + transform.parent.parent.GetComponent<PlayerController>().gameManager.bases[tempPlayer.PlayerId].playerId);
				GameObject.Find("GameManager").GetComponent<SpawnManager>().leavesOnMap.Add(other.gameObject);
				other.gameObject.GetComponent<MapleLeaf>().captor = null;
			}
			other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			other.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
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
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().punchResetTime = (Time.time + punchRecharge);
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().pState = PlayerController.playerState.Idle;
		} else {
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().punchResetTime = (Time.time + punchRechargeCarry);
			this.transform.parent.transform.parent.GetComponent<PlayerController> ().pState = PlayerController.playerState.Carrying;
		}

		this.gameObject.transform.parent.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + this.transform.parent.parent.GetComponent<PlayerController>().character.ToString () + "_Body") as Sprite;
		this.gameObject.SetActive(false);
	}
}
