using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	public int playerId;
	public List<GameObject> gems = new List<GameObject>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.GetComponent<PlayerController> ()) {
			//is a player
			PlayerController player = collider.gameObject.GetComponent<PlayerController> ();
			if(player.PlayerId == this.playerId)
			{
				if(player.pState == PlayerController.playerState.Carrying)
				{
					if(player.leafInArms.GetComponent<MapleLeaf>().leafColor == player.activeColor)
					{
						//score!
					}
					else
					{
						//don't score!
					}
				}
			}
		}
	}
}
