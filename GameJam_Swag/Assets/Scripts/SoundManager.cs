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

	public AudioClip gameStart;

	public AudioClip countdown;

	public GameManager gameManager;

	void Start() {
		gameManager = this.GetComponent<GameManager> ();
	}

	public void PlaySound(GameManager.SoundType s) {
		switch (s) {
		case GameManager.SoundType.bear:
			gameManager.GetPlayerById(1).GetComponent<AudioSource>().PlayOneShot(bearPunches[Random.Range(0, bearPunches.Length)]);
			break;
		case GameManager.SoundType.bearGod:
			gameManager.GetPlayerById(1).GetComponent<AudioSource>().PlayOneShot(bearGodMode);
			break;
		case GameManager.SoundType.moose:
			gameManager.GetPlayerById(4).GetComponent<AudioSource>().PlayOneShot(moosePunches[Random.Range(0, moosePunches.Length)]);
			break;
		case GameManager.SoundType.mooseGod:
			gameManager.GetPlayerById(4).GetComponent<AudioSource>().PlayOneShot(mooseGodMode);
			break;
		case GameManager.SoundType.loon:
			gameManager.GetPlayerById(2).GetComponent<AudioSource>().PlayOneShot(loonPunches[Random.Range(0, loonPunches.Length)]);
			break;
		case GameManager.SoundType.loonGod:
			gameManager.GetPlayerById(2).GetComponent<AudioSource>().PlayOneShot(loonGodMode);
			break;
		case GameManager.SoundType.beaver:
			gameManager.GetPlayerById(3).GetComponent<AudioSource>().PlayOneShot(beaverPunches[Random.Range(0, beaverPunches.Length)]);
			break;
		case GameManager.SoundType.beaverGod:
			gameManager.GetPlayerById(3).GetComponent<AudioSource>().PlayOneShot(beaverGodMode);
			break;
		case GameManager.SoundType.grab:
			this.GetComponent<AudioSource>().PlayOneShot(grabNoise);
			break;
		case GameManager.SoundType.leafYes:
			this.GetComponent<AudioSource>().PlayOneShot(leafSuccess);
			break;
		case GameManager.SoundType.leafNo:
			this.GetComponent<AudioSource>().PlayOneShot(leafFail);
			break;
		case GameManager.SoundType.countDown:
			this.GetComponent<AudioSource>().PlayOneShot(countdown);
			break;
		case GameManager.SoundType.intro:
			this.GetComponent<AudioSource>().PlayOneShot(gameStart);
			break;
		}
	}
}
