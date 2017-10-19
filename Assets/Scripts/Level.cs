using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField]
	private FloorTile tilePrefab;

	[SerializeField]
	private List<FloorTile> tiles;

	private void Start()
	{
		for (float x = -7.5f; x <= 7.5f; x += 1.0f)
		{
			for(float z = -7.5f; z <= 7.5f; z += 1.0f)
			{
				tiles.Add(Instantiate(tilePrefab, new Vector3(x, -0.25f, z), tilePrefab.transform.rotation));
			}
		}
	}
}