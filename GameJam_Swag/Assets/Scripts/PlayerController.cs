using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector2 movementVector;

	public float movementSpeed = 4f;
	public float dragConstant = 0.09f;

	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		movementVector.x = Input.GetAxis("LeftJoystickX") * movementSpeed;
		movementVector.y = Input.GetAxis("LeftJoystickY") * movementSpeed * -1;

		this.gameObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * (Mathf.Atan2 (movementVector.y, movementVector.x)) - 90f);

		movementVector.x = Mathf.Lerp (rb.velocity.x, movementVector.x, dragConstant);
		movementVector.y = Mathf.Lerp (rb.velocity.y, movementVector.y, dragConstant);

		rb.velocity = movementVector;
		//rb.AddForce (movementVector * movementSpeed);
	}
}
