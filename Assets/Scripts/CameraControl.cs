using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public IEnumerator Shake()
	{
		Vector3 pos = transform.position;
		for (float t = 0.0f; t < 0.2f; t += Time.deltaTime)
		{
			transform.position = pos + Random.insideUnitSphere / 2.5f;
			yield return new WaitForEndOfFrame();
		}

		transform.position = pos;
	}
}