using UnityEngine;
using System.Collections;

public class BaseManager : MonoBehaviour {
	public GameObject order;

	private PlayerController playerPosition;
	public CircleCollider2D playerCollider;
	public SpriteRenderer render;
	
	// Use this for initialization
	void Start () {
		render = order.GetComponent<SpriteRenderer> ();
		//
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.GetComponent<PlayerController> ()) {
			render.color = new Color (0.5f, 0.5f, 0.5f);
		}
	}
}
