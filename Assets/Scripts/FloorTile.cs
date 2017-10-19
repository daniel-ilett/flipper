using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
	[SerializeField]
	private Material redMat;
	[SerializeField]
	private Material blueMat;
	[SerializeField]
	private Material yellowMat;
	[SerializeField]
	private Material greenMat;

	private int playerID = 0;

	[SerializeField]
	private new Renderer renderer;

	public void SetColour(int playerID)
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