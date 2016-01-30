using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	private float defaultSize;
	private float defaultPosition;

	public GameObject character;

	void Start () {
		defaultSize = Camera.main.orthographicSize;
		defaultPosition = this.transform.position.x;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			ZoomIn ();
		}
	}

	void ZoomIn () {
		iTween.ValueTo (this.gameObject, iTween.Hash ("from", defaultSize,
		                                                     "to", 2f,
		                                                     "time", 0.5f,
		                                                     "easetype", iTween.EaseType.easeInExpo,
		                                                     "onupdate", "UpdateOrthographicCameraSize",
		                                                     "onupdatetarget", this.gameObject,
															 "oncomplete", "ZoomOut"));

		iTween.ValueTo (this.gameObject, iTween.Hash ("from", defaultPosition,
		                                                     "to", character.transform.position.x,
		                                                     "time", 0.5f,
		                                                     "easetype", iTween.EaseType.easeInExpo,
		                                                     "onupdate", "UpdateCameraPosition",
		                                                     "onupdatetarget", this.gameObject,
		                                                     "oncomplete", "moveToOrigin"));
	}
	
	void UpdateOrthographicCameraSize (float size) {
		Camera.main.orthographicSize = size; 
	}

	void UpdateCameraPosition (float position) {
		Debug.Log (position);
		this.transform.position = new Vector3(position, this.transform.position.y, this.transform.position.z);
	}

	void moveToOrigin () {
		StartCoroutine(Wait(1));
	}

	void ZoomOut () {
		StartCoroutine(Wait(1));
	}

	IEnumerator Wait(int timeInSeconds) {
		yield return new WaitForSeconds(timeInSeconds);

		iTween.ValueTo (this.gameObject, iTween.Hash ("from", Camera.main.orthographicSize,
		                                              "to", defaultSize,
		                                              "time", 0.7f,
		                                              "easetype", iTween.EaseType.easeOutExpo,
		                                              "onupdate", "UpdateOrthographicCameraSize",                                           
		                                              "onupdatetarget", this.gameObject));

		iTween.ValueTo (this.gameObject, iTween.Hash ("from", this.transform.position.x,
		                                              "to", defaultPosition,
		                                              "time", 0.7f,
		                                              "easetype", iTween.EaseType.easeOutExpo,
		                                              "onupdate", "UpdateCameraPosition",
		                                              "onupdatetarget", this.gameObject));
	}
	
}
