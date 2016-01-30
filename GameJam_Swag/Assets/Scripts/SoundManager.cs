using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public AudioClip[] bearPunches;
	public AudioClip[] moosePunches;
	public AudioClip[] loonPunches;
	public AudioClip[] beaverPunches;

	public AudioClip bearGodMode;
	public AudioClip mooseGodMode;
	public AudioClip loonGodMode;
	public AudioClip beaverGodMode;

	public AudioClip grabNoise;
	public AudioClip throwPull;
	public AudioClip throwRelease;
	public AudioClip powerupPickup;
	public AudioClip powerupRelease;
	public AudioClip leafFail;
	public AudioClip leafSuccess;

	public void PlaySound(GameManager.SoundType s) {
		switch (s) {
		case GameManager.SoundType.bear:
			this.GetComponent<AudioSource>().PlayOneShot(bearPunches[Random.Range(0, bearPunches.Length)]);
			break;
		case GameManager.SoundType.moose:
			this.GetComponent<AudioSource>().PlayOneShot(moosePunches[Random.Range(0, moosePunches.Length)]);
			break;
		case GameManager.SoundType.loon:
			this.GetComponent<AudioSource>().PlayOneShot(loonPunches[Random.Range(0, loonPunches.Length)]);
			break;
		case GameManager.SoundType.beaver:
			this.GetComponent<AudioSource>().PlayOneShot(beaverPunches[Random.Range(0, beaverPunches.Length)]);
			break;
		case GameManager.SoundType.grab:
			this.GetComponent<AudioSource>().PlayOneShot(grabNoise);
			break;
		case GameManager.SoundType.leafYes:
			this.GetComponent<AudioSource>().PlayOneShot(leafSuccess);
			break;
		}
	}
}
