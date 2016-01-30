using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	private float defaultSize;
	private bool followTarget = false;
	private bool zoomIn = false;
	public GameObject character;
	public Vector3 originalCameraPosition;
	private float targetSize = 3f;

	void Start () {
		defaultSize = Camera.main.orthographicSize;
		originalCameraPosition = Camera.main.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (followTarget) {
			this.gameObject.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, originalCameraPosition.z);
			Time.timeScale = 0.25f;

			if(Camera.main.orthographicSize > (targetSize+0.2f)){
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.deltaTime*10f);
			}
			else
			{
				Camera.main.orthographicSize = defaultSize;
				followTarget = false;
				this.gameObject.transform.position = originalCameraPosition;
				Time.timeScale = 1.0f;
			}
		}
	}

	public void ZoomIn (GameObject target) {

		followTarget = true;
		zoomIn = true;
		character = target;
		this.gameObject.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, originalCameraPosition.z);
		/*iTween.ValueTo (this.gameObject, iTween.Hash ("from", defaultSize,
		                                                     "to", 2f,
		                                                     "time", 0.2f,
		                                                     "easetype", iTween.EaseType.linear,
		                                                     "onupdate", "UpdateOrthographicCameraSize",
		                                                     "onupdatetarget", this.gameObject,
															 "oncomplete", "ZoomOut"));*/


	}
	
	void UpdateOrthographicCameraSize (float size) {
		Camera.main.orthographicSize = size; 
	}

	void moveToOrigin () {
		StartCoroutine(Wait(0.5f));
	}

	void ZoomOut () {
		StartCoroutine(Wait(0.5f));
	}

	IEnumerator Wait(float timeInSeconds) {
		yield return new WaitForSeconds(timeInSeconds);

		this.transform.position = originalCameraPosition;

		iTween.ValueTo (this.gameObject, iTween.Hash ("from", Camera.main.orthographicSize,
		                                              "to", defaultSize,
		                                              "time", 0.4f,
		                                              "easetype", iTween.EaseType.easeOutExpo,
		                                              "onupdate", "UpdateOrthographicCameraSize",                                           
		                                              "onupdatetarget", this.gameObject));

		followTarget = false;
		character = null;
	}
	
}
