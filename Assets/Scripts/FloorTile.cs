using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	// Player colour materials.
	[SerializeField]
	private Material redMat;
	[SerializeField]
	private Material blueMat;
	[SerializeField]
	private Material yellowMat;
	[SerializeField]
	private Material greenMat;
	[SerializeField]
	private Material deadMat;

	private int playerID = 0;

	// References to components.
	[SerializeField]
	private new Renderer renderer;
	private new Rigidbody rigidbody;

	private bool isAlive = true;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	public void SetColour(int playerID)
	{
		if(isAlive)
		{
			this.playerID = playerID;

			switch (playerID)
			{
				case 1:
					renderer.material = redMat;
					break;
				case 2:
					renderer.material = blueMat;
					break;
				case 3:
					renderer.material = yellowMat;
					break;
				default:
					renderer.material = greenMat;
					break;
			}
		}
	}

	// Kill the tile and let it fly away.
	public void HitWithExplosion(Vector3 origin, float force, float size)
	{
		if(isAlive)
		{
			rigidbody.isKinematic = false;

			renderer.material = deadMat;
			playerID = 0;
			isAlive = false;

			rigidbody.AddExplosionForce(force, origin, size, 0, ForceMode.Impulse);
			rigidbody.AddExplosionForce(force, origin + Random.onUnitSphere, size, 0, ForceMode.Impulse);
		}
	}

	public int GetPlayerID()
	{
		return playerID;
	}

	public bool IsAlive()
	{
		return isAlive;
	}
}