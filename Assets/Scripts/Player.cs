using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	// Player details.
	[SerializeField]
	private int playerID;

	// Button/Axis mapping variables.
	private string xAxis;
	private string zAxis;

	private string leftCycle;
	private string rightCycle;
	
	private string use;
	private string jump;

	// Player properties.
	private float startSpeed = 5.0f;
	private float moveSpeed = 5.0f;

	// Raycasting variables.
	[SerializeField]
	private LayerMask mask;

	// References.
	private new Rigidbody rigidbody;

	private void Start()
	{
		switch (playerID)
		{
			// Right (Red) Joy-Con user.
			case 1:
				xAxis = "J1Horizontal";
				zAxis = "J1Vertical";
				leftCycle = "J1LCycle";
				rightCycle = "J1RCycle";
				use = "J1Use";
				jump = "J1Jump";
				break;
			// Left (Blue) Joy-Con user.
			case 2:
				xAxis = "J2Horizontal";
				zAxis = "J2Vertical";
				leftCycle = "J2LCycle";
				rightCycle = "J2RCycle";
				use = "J2Use";
				jump = "J2Jump";
				break;
			// Pro Controller user.
			case 3:
				xAxis = "J3Horizontal";
				zAxis = "J3Vertical";
				leftCycle = "J3LCycle";
				rightCycle = "J3RCycle";
				use = "J3Use";
				jump = "J3Jump";
				break;
			// Keyboard user.
			default:
				xAxis = "J4Horizontal";
				zAxis = "J4Vertical";
				leftCycle = "J4LCycle";
				rightCycle = "J4RCycle";
				use = "J4Use";
				jump = "J4Jump";
				break;
		}

		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update ()
	{
		Vector3 movement = new Vector3(Input.GetAxis(xAxis), 0.0f, 
			Input.GetAxis(zAxis)) * Time.deltaTime * moveSpeed;
		
		transform.Translate(movement);

		if (Input.GetButtonDown(jump))
			rigidbody.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);

		RaycastHit[] hits;
		hits = Physics.BoxCastAll(transform.position, new Vector3(0.2f, 0.2f, 0.2f), -Vector3.up, Quaternion.identity, 0.4f, mask);

		foreach(RaycastHit hit in hits)
		{
			FloorTile tile = hit.transform.GetComponent<FloorTile>();

			if(tile != null)
				tile.SetColour(playerID);
		}
	}
}
