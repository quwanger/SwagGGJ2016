﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	public int playerId;
	public List<GameObject> gems = new List<GameObject>();

	public GameObject vulnerableGem;

	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();
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
				if(player.pState == PlayerController.playerState.Carrying || player.pState == PlayerController.playerState.Throwing)
				{
					if(player.leafInArms.GetComponent<MapleLeaf>().leafColor == player.activeColor)
					{
						gameManager.soundManager.PlaySound (GameManager.SoundType.leafYes);
						GameObject crackPart = Instantiate (Resources.Load<GameObject> ("Prefabs/crackParticle"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),Quaternion.identity) as GameObject;
						crackPart.GetComponent<ParticleSystem>().startColor = player.activeColor;
						crackPart.transform.parent = this.transform;
						Destroy (crackPart, 3);

						//score!
						Debug.Log ("Score!");
						// Set the big leaf to the active color
						transform.GetChild(0).GetComponent<SpriteRenderer>().color = player.activeColor;
						transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GemBaseOn");

						if(player.pState == PlayerController.playerState.Throwing)
						{
							player.CancelThrow();
						}

						// Disable effect in the gem history
						gems[player.currentGemIndex].GetComponent<Gem>().DeactivateGem();
						player.currentGemIndex++;

						// Remove the leaf from the players hand when they score
						if(vulnerableGem != null)
						{
							Destroy(vulnerableGem);
						}
						vulnerableGem = player.leafInArms;
						player.gameManager.spawnManager.leavesOnMap.Remove(player.leafInArms);
						player.leafInArms.transform.parent = this.transform;
						player.leafInArms.GetComponent<CircleCollider2D>().isTrigger = false;
						player.leafInArms.GetComponent<MapleLeaf>().captor = player;
						player.leafInArms.GetComponent<MapleLeaf>().carrier = null;
						player.leafInArms = null;

						//Destroy(player.leafInArms.gameObject);

						// Change player state
						player.pState = PlayerController.playerState.Idle;

						// If player won
						if(player.currentGemIndex >= gameManager.sequenceCount) 
						{
							player.gameManager.DeclareWinner(player);

							// Restart game
							gameManager.ResetRound();
						} 

						// If player didn't win
						else 
						{
							// Active next gem
							gems[player.currentGemIndex].GetComponent<Gem>().ActivateGem();

							// Update the players active color (which color is needed next)
							player.activeColor = player.myColors[player.currentGemIndex];

							player.playerShadow.color = player.activeColor;

							// Spawn a new leaf
							player.gameManager.spawnManager.SpawnLeaf();
						}
					}
					else
					{
						//don't score!
						Debug.Log ("WRONG BASE NOOB!");
					}
				}
			}
		}
	}

	private void ScorePoint()
	{

	}

	public void resetBase() {
		// Reset gems to default 
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GemBaseOff");
		transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255,255,255);
		// To be safe, lets deactivate all gems
		for(int i = 0; i < gems.Count; i++) {
			gems[i].GetComponent<Gem> ().ResetGem ();
			gems[i].SetActive(false);
		}
	}
}
