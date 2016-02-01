﻿using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {

	public bool isActiveGem = false;
	float targetScale = 1.2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveGem) {
			float scale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime*2f);

			if(scale > 1.15f)
			{
				targetScale = 0.7f;
			}
			else if(scale < 0.75f)
			{
				targetScale = 1.2f;
			}

			transform.localScale = new Vector3 (scale, scale, scale);
		}
	}

	public void ActivateGem()
	{
		isActiveGem = true;
	}

	public void DeactivateGem()
	{
		transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/GemBaseOn");
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		isActiveGem = false;
	}

	public void ReactivateGem()
	{
		//Debug.Log ("REACTIVATE GEM");
		transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/GemBaseOff");
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		isActiveGem = true;
	}

	public void StolenGem()
	{
		//Debug.Log ("STOLEN GEM");
		//transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/GemOff");
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		isActiveGem = false;
	}

	public void ResetGem()
	{
		transform.localScale = new Vector3 (0.8f, 0.8f, 0.8f);
		transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/GemBaseOff");
		isActiveGem = false;
	}
}
