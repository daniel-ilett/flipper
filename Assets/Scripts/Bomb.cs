using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField]
	private GameObject explosion;

	[SerializeField]
	private int playerID;

	[SerializeField]
	private LayerMask mask;

	public void Start()
	{
		StartCoroutine(Countdown());
	}

	private IEnumerator Countdown()
	{
		yield return new WaitForSeconds(1.0f);
		float scale = 0.0f;

		// Sine wave scaling.
		for(float t = 0.0f; t < 2.0f; t += Time.deltaTime)
		{
			scale = Mathf.Sin(t * 10.0f) * 0.05f + 0.45f;
			transform.localScale = new Vector3(scale, scale, scale);
			yield return new WaitForEndOfFrame();
		}

		// Get larger until exploding.
		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime)
		{
			scale += Time.deltaTime / 5.0f;
			transform.localScale = new Vector3(scale, scale, scale);
			yield return new WaitForEndOfFrame();
		}

		Explode(null);
	}

	private void Explode(Bomb b)
	{
		if(b == this)
			return;

		StopAllCoroutines();

		Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, mask);

		foreach (Collider col in colliders)
		{
			Debug.Log(col);

			FloorTile tile = null;

			if (col.transform.parent != null)
				 tile = col.transform.parent.GetComponent<FloorTile>();

			if (tile != null)
				tile.HitWithExplosion(transform.position, 10.0f, 2.5f);

			Player player = col.GetComponent<Player>();

			if (player != null && player.GetPlayerID() != playerID)
				player.HitWithExplosion(transform.position, 10.0f, 2.5f);

			//Bomb bomb = col.GetComponent<Bomb>();

			//if (bomb != null)
				//bomb.Explode(this);
		}

		Camera.main.GetComponent<CameraControl>().StartCoroutine(Camera.main.GetComponent<CameraControl>().Shake());
		Destroy(gameObject);
	}
}