using UnityEngine;
using System.Collections;

public class StarParticle : MonoBehaviour {

	private ParticleSystem starParticles;
	
	// Use this for initialization
	void Awake () {
		starParticles = GetComponent<ParticleSystem> ();
	}

	void Update() {
		if (Input.GetMouseButton (1)) {
			PlayParticles();
		}
	}

	void PlayParticles () {
		starParticles.Play ();
	}
}
