using UnityEngine;
using System.Collections;

public class MapleLeaf : MonoBehaviour {

	public Color leafColor;
	public PlayerController carrier;
	public bool isBeingThrown = false;
	public PlayerController captor;
	public PlayerController thrower;

	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;

		gameManager = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isBeingThrown) {
			float velX = Mathf.Lerp(this.gameObject.GetComponent<Rigidbody2D> ().velocity.x, 0, Time.deltaTime);
			float velY = Mathf.Lerp(this.gameObject.GetComponent<Rigidbody2D> ().velocity.y, 0, Time.deltaTime);

			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(velX, velY);

			float tempMag = this.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;

			if(tempMag < 1f && tempMag != 0)
			{
				this.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
				isBeingThrown = false;
				this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, 0);
				thrower = null;
			}
		}

		//Debug.Log ("(" + this.gameObject.GetComponent<Rigidbody2D> ().velocity.x + ", " + this.gameObject.GetComponent<Rigidbody2D> ().velocity.y + ")");
	}

	void OnCollisionEnter2D(Collision2D coll){
		gameManager.soundManager.PlaySound (GameManager.SoundType.grab);

		GameObject hitPart = Instantiate (Resources.Load<GameObject> ("Prefabs/hitParticle"), new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),Quaternion.identity) as GameObject;
		hitPart.transform.parent = this.transform;
		Destroy (hitPart, 1);

		if (isBeingThrown) {
			if (coll.gameObject.GetComponent<PlayerController> ()) {
				//gameManager.soundManager.PlaySound (GameManager.SoundType.grab);

				PlayerController tempPlayer = coll.gameObject.GetComponent<PlayerController> ();
				if (tempPlayer.pState != PlayerController.playerState.Stunned) {
					if (tempPlayer.pState == PlayerController.playerState.Throwing || tempPlayer.pState == PlayerController.playerState.Carrying)
					{
						tempPlayer.ThrowLeaf(default(Vector3));
					}
					tempPlayer.Stun ();
				}
				ChangeLeafColor(thrower.activeColor);
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
					ChangeLeafColor(thrower.activeColor);
				}
			}
		}
	}

	public void ChangeLeafColorRandom()
	{
		leafColor = GameObject.Find ("GameManager").GetComponent<SpawnManager> ().possibleColors [Random.Range (0, GameObject.Find ("GameManager").GetComponent<GameManager> ().colorCount)];
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;
	}

	public void ChangeLeafColor(Color color)
	{
		leafColor = color;
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;
	}
}
