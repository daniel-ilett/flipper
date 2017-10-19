using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

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

	private bool canControl = false;

	// Game references.
	private Level level;

	// Raycasting variables.
	[SerializeField]
	private LayerMask mask;

	// Bomb properties.
	[SerializeField]
	private GameObject bomb;

	[SerializeField]
	private Text readyText;
	[SerializeField]
	private Image readyImage;

	private bool canBomb = true;

	// Blob shadow references.
	[SerializeField]
	private GameObject blobShadow;

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

	public void SetLevel(Level level)
	{
		this.level = level;
	}

	public void ChangeControl(bool canControl)
	{
		this.canControl = canControl;
	}

	private void Update ()
	{
		if(canControl)
		{
			Vector3 movement = new Vector3(Input.GetAxis(xAxis), 0.0f,
			Input.GetAxis(zAxis)) * Time.deltaTime * moveSpeed;

			transform.Translate(movement);

			if (Input.GetButtonDown(jump))
				rigidbody.AddForce(Vector3.up * 15.0f, ForceMode.Impulse);

			if (Input.GetButtonDown(use))
			{
				if(canBomb)
				{
					Instantiate(bomb, transform.position + new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);

					canBomb = false;
					StartCoroutine(BombCooldown());
				}
			}
		}

		RaycastHit[] hits;
		hits = Physics.BoxCastAll(transform.position, new Vector3(0.2f, 0.2f, 0.2f), -Vector3.up, Quaternion.identity, 0.4f, mask);

		foreach(RaycastHit hit in hits)
		{
			FloorTile tile = hit.transform.GetComponent<FloorTile>();

			if(tile != null)
				tile.SetColour(playerID);
		}
	}

	private IEnumerator BombCooldown()
	{
		readyImage.fillAmount = 0.0f;
		readyText.text = "Recharging...";
		for (float t = 0; t < 2.5f; t += Time.deltaTime)
		{
			readyImage.fillAmount = t / 2.5f;
			yield return new WaitForEndOfFrame();
		}

		readyImage.fillAmount = 1.0f;
		readyText.text = "Ready!";

		canBomb = true;
	}

	public void HitWithExplosion(Vector3 origin, float force, float size)
	{
		blobShadow.SetActive(false);

		gameObject.layer = 12;

		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.AddExplosionForce(force * 5.0f, origin, size, 1.0f, ForceMode.Impulse);

		if(canControl)
			level.PlayerDead(this);

		canControl = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Destructor")
		{
			canControl = false;
			level.PlayerDead(this);
		}
	}

	public int GetPlayerID()
	{
		return playerID;
	}
}
