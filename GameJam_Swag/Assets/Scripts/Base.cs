using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {

	public int playerId;
	public List<GameObject> gems = new List<GameObject>();

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
				if(player.pState == PlayerController.playerState.Carrying)
				{
					Debug.Log("CARRY");

					if(player.leafInArms.GetComponent<MapleLeaf>().leafColor == player.activeColor)
					{
						gameManager.soundManager.PlaySound (GameManager.SoundType.leafYes);
						GameObject crackPart = Instantiate (Resources.Load<GameObject> ("Prefabs/crackParticle"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),Quaternion.identity) as GameObject;
						crackPart.GetComponent<ParticleSystem>().startColor = player.activeColor;
						crackPart.transform.parent = this.transform;
						Destroy (crackPart, 3);

						//score!
						Debug.Log ("Score!");
						transform.GetChild(0).GetComponent<SpriteRenderer>().color = player.activeColor;
						transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/GemBaseOn");
						gems[player.currentGemIndex].GetComponent<Gem>().DeactivateGem();
						player.currentGemIndex++;
						player.gameManager.spawnManager.leavesOnMap.Remove(player.leafInArms);

						//player.leafInArms.transform.parent = this.transform;

						Destroy(player.leafInArms.gameObject);
						player.pState = PlayerController.playerState.Idle;
						
						if(player.currentGemIndex >= player.gameManager.spawnManager.sequenceCount)
						{
							player.gameManager.DeclareWinner(player);
						}else
						{
							gems[player.currentGemIndex].GetComponent<Gem>().ActivateGem();
							player.activeColor = player.myColors[player.currentGemIndex];
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
}
