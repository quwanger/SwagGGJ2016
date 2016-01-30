using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	public GameObject mapleLeaf;

	// Use this for initialization
	void Start () {
		Vector3 position = new Vector3 (Random.Range (-10.0F, 10.0F), Random.Range (-10F, 10.0F), 0);
		Instantiate (mapleLeaf, position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
