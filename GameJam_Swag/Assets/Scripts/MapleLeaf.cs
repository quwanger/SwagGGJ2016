using UnityEngine;
using System.Collections;

public class MapleLeaf : MonoBehaviour {

	public Color leafColor;
	public PlayerController carrier;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().color = leafColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
