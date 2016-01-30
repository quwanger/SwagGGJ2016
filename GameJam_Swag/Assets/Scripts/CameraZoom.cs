using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	private float originPosition;

	void Start () {
		originPosition = Camera.main.orthographicSize;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space"))
			ZoomIn ();
	}

	void ZoomIn () {

		iTween.ValueTo (Camera.main.gameObject, iTween.Hash ("from", originPosition,
		                                                     "to", 2f,
		                                                     "time", 0.5f,
		                                                     "easetype", iTween.EaseType.easeInExpo,
		                                                     "onupdate", "UpdateOrthographicCameraSize",
		                                                     "onupdatetarget", Camera.main.gameObject,
															 "oncomplete", "ZoomOut"));	
	}
	
	void UpdateOrthographicCameraSize (float size) {
		Camera.main.orthographicSize = size;
	}

	void ZoomOut () {
		iTween.ValueTo (Camera.main.gameObject, iTween.Hash ("from", Camera.main.orthographicSize,
		                                                     "to", originPosition,
		                                                     "time", 0.7f,
		                                                     "easetype", iTween.EaseType.easeOutExpo,
		                                                     "onupdate", "UpdateOrthographicCameraSize",
		                                                     "onupdatetarget", Camera.main.gameObject));
	}
}
