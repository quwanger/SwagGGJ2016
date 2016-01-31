using UnityEngine;
using System.Collections;

public class MapleLeaf : MonoBehaviour {

	public Color leafColor;
	public PlayerController carrier;
	public bool isBeingThrown = false;
	public PlayerController captor;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (isBeingThrown) {
			float velX = Mathf.Lerp(this.gameObject.GetComponent<Rigidbody2D> ().velocity.x, 0, Time.deltaTime);
			float velY = Mathf.Lerp(this.gameObject.GetComponent<Rigidbody2D> ().velocity.y, 0, Time.deltaTime);

			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(velX, velY);

			if(this.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude < 1f && this.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude != 0f)
			{
				this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
				isBeingThrown = false;
				this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, 0);
			}
		}

		//Debug.Log ("(" + this.gameObject.GetComponent<Rigidbody2D> ().velocity.x + ", " + this.gameObject.GetComponent<Rigidbody2D> ().velocity.y + ")");
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (isBeingThrown) {
			if (coll.gameObject.GetComponent<PlayerController> ()) {
				PlayerController tempPlayer = coll.gameObject.GetComponent<PlayerController> ();
				if (tempPlayer.pState != PlayerController.playerState.Stunned) {
					if (tempPlayer.pState == PlayerController.playerState.Throwing || tempPlayer.pState == PlayerController.playerState.Carrying)
					{
						tempPlayer.ThrowLeaf(default(Vector3));
					}
					tempPlayer.Stun ();
				}
			}else if (coll.gameObject.GetComponent<MapleLeaf> ()) {
				if(coll.gameObject.GetComponent<MapleLeaf>().carrier != null)
				{
					PlayerController tempPlayer = coll.gameObject.GetComponent<MapleLeaf> ().carrier.GetComponent<PlayerController>();
					if (tempPlayer.pState != PlayerController.playerState.Stunned) {
						if (tempPlayer.pState == PlayerController.playerState.Throwing || tempPlayer.pState == PlayerController.playerState.Carrying)
						{
							tempPlayer.ThrowLeaf(default(Vector3));
						}
						tempPlayer.Stun ();
					}
				}
			}
		}
	}

	public void ChangeLeafColorRandom()
	{
		leafColor = GameObject.Find ("GameManager").GetComponent<SpawnManager> ().possibleColors [Random.Range (0, GameObject.Find ("GameManager").GetComponent<GameManager> ().colorCount)];
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;
	}
}
