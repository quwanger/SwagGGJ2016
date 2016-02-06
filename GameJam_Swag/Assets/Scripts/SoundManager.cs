using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    // Load in audio clips
	public AudioClip[] bearPunches;
	public AudioClip[] moosePunches;
	public AudioClip[] loonPunches;
	public AudioClip[] beaverPunches;
	public AudioClip bearGodMode;
	public AudioClip mooseGodMode;
	public AudioClip loonGodMode;
	public AudioClip beaverGodMode;
	public AudioClip bearFireMode;
	public AudioClip mooseFireMode;
	public AudioClip loonFireMode;
	public AudioClip beaverFireMode;
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

    // Sound effects enum for easy reference
    public enum SoundType {
        bear,
        bearGod,
        bearFire,
        moose,
        mooseGod,
        mooseFire,
        loon,
        loonGod,
        loonFire,
        beaver,
        beaverGod,
        beaverFire,
        grab,
        throwStart,
        throwEnd,
        powerPickup,
        powerRelease,
        leafNo,
        leafYes,
        intro,
        countDown
    };

    void Start() {
        // Initialize our gameManager variable to GameManager. We need this to get the player ID and add audio sources to them invididually
        gameManager = GetComponent<GameManager> ();
	}

	public void PlaySound(SoundType s) {
		switch (s) {
		case SoundType.bear:
			gameManager.GetPlayerById(1).GetComponent<AudioSource>().PlayOneShot(bearPunches[Random.Range(0, bearPunches.Length)]);
			break;
		case SoundType.bearGod:
			gameManager.GetPlayerById(1).GetComponent<AudioSource>().PlayOneShot(bearGodMode);
			break;
		case SoundType.bearFire:
			gameManager.GetPlayerById(1).GetComponent<AudioSource>().PlayOneShot(bearFireMode);
			break;
		case SoundType.moose:
			gameManager.GetPlayerById(2).GetComponent<AudioSource>().PlayOneShot(moosePunches[Random.Range(0, moosePunches.Length)]);
			break;
		case SoundType.mooseGod:
			gameManager.GetPlayerById(2).GetComponent<AudioSource>().PlayOneShot(mooseGodMode);
			break;
		case SoundType.mooseFire:
			gameManager.GetPlayerById(2).GetComponent<AudioSource>().PlayOneShot(mooseFireMode);
			break;
		case SoundType.loon:
			gameManager.GetPlayerById(4).GetComponent<AudioSource>().PlayOneShot(loonPunches[Random.Range(0, loonPunches.Length)]);
			break;
		case SoundType.loonGod:
			gameManager.GetPlayerById(4).GetComponent<AudioSource>().PlayOneShot(loonGodMode);
			break;
		case SoundType.loonFire:
			gameManager.GetPlayerById(4).GetComponent<AudioSource>().PlayOneShot(loonFireMode);
			break;
		case SoundType.beaver:
			gameManager.GetPlayerById(3).GetComponent<AudioSource>().PlayOneShot(beaverPunches[Random.Range(0, beaverPunches.Length)]);
			break;
		case SoundType.beaverGod:
			gameManager.GetPlayerById(3).GetComponent<AudioSource>().PlayOneShot(beaverGodMode);
			break;
		case SoundType.beaverFire:
			gameManager.GetPlayerById(3).GetComponent<AudioSource>().PlayOneShot(beaverFireMode);
			break;
		case SoundType.grab:
			this.GetComponent<AudioSource>().PlayOneShot(grabNoise);
			break;
		case SoundType.leafYes:
			this.GetComponent<AudioSource>().PlayOneShot(leafSuccess);
			break;
		case SoundType.leafNo:
			this.GetComponent<AudioSource>().PlayOneShot(leafFail);
			break;
		case SoundType.countDown:
			this.GetComponent<AudioSource>().PlayOneShot(countdown);
			break;
		case SoundType.intro:
			this.GetComponent<AudioSource>().PlayOneShot(gameStart);
			break;
		}
	}
}
