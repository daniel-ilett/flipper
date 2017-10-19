using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
	// Tiles.
	[SerializeField]
	private FloorTile tilePrefab;

	[SerializeField]
	private List<FloorTile> tiles;

	// Players.
	[SerializeField]
	private Player[] players;
	private List<Player> deadPlayers;

	// Countdown and UI.
	[SerializeField]
	private GameObject mainHUD;
	[SerializeField]
	private Text countdownText;
	[SerializeField]
	private Text goText;

	private bool hasEnded = false;

	// Score counting.
	[SerializeField]
	private Image playerOneScore;
	[SerializeField]
	private Image playerTwoScore;
	[SerializeField]
	private Image playerThreeScore;
	[SerializeField]
	private Image playerFourScore;

	[SerializeField]
	private GameObject winnerHUD;
	[SerializeField]
	private Text winnerText;

	private void Start()
	{
		deadPlayers = new List<Player>();

		for (float x = -7.5f; x <= 7.5f; x += 1.0f)
		{
			for(float z = -7.5f; z <= 7.5f; z += 1.0f)
			{
				tiles.Add(Instantiate(tilePrefab, new Vector3(x, -0.25f, z), tilePrefab.transform.rotation));
			}
		}

		StartCoroutine(Countdown());
	}

	private IEnumerator Countdown()
	{
		for(int i = 3; i > 0; --i)
		{
			goText.text = i.ToString();
			yield return new WaitForSeconds(1.0f);
		}

		StartCoroutine(GoDisappear("GO!"));
		StartCoroutine(GameCountdown());

		foreach (Player p in players)
		{
			p.SetLevel(this);
			p.ChangeControl(true);
		}	
	}

	private IEnumerator GoDisappear(string message)
	{
		goText.text = message;
		Vector3 pos = goText.rectTransform.position;

		for (float t = 4.0f; t > -1.0f; t -= Time.deltaTime * 2.0f)
		{
			Color col = goText.color;
			col.a = t;
			goText.color = col;

			goText.rectTransform.position = pos + (Vector3)Random.insideUnitCircle * 2.5f;

			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator GameCountdown()
	{
		for(int t = 60; t > 0;--t)
		{
			countdownText.text = t.ToString();

			if(t <= 10)
			{
				countdownText.color = Color.red;
				goText.color = Color.white;
				goText.text = t.ToString();
			}

			yield return new WaitForSeconds(1.0f);
		}

		GameEnd();
	}

	private void GameEnd()
	{
		if(!hasEnded)
		{
			hasEnded = true;

			countdownText.text = "0";

			StopAllCoroutines();
			StartCoroutine(GoDisappear("GAME!"));

			foreach (Player p in players)
				p.ChangeControl(false);

			StartCoroutine(CountScore());
		}
	}

	private IEnumerator CountScore()
	{
		yield return new WaitForSeconds(2.5f);

		mainHUD.SetActive(false);
		winnerHUD.SetActive(true);

		int p1 = 0;
		int p2 = 0;
		int p3 = 0;
		int p4 = 0;

		foreach(FloorTile tile in tiles)
		{
			switch(tile.GetPlayerID())
			{
				case 1:
					++p1; playerOneScore.fillAmount = (float)p1 / tiles.Count; break;
				case 2:
					++p2; playerTwoScore.fillAmount = (float)p2 / tiles.Count; break;
				case 3:
					++p3; playerThreeScore.fillAmount = (float)p3 / tiles.Count; break;
				case 4:
					++p4; playerFourScore.fillAmount = (float)p4 / tiles.Count; break;
				default:
					if (!tile.IsAlive())
						continue;

					break;
			}
			tile.HitWithExplosion(tile.transform.position - new Vector3(0.0f, 1.0f, 0.0f), 10.0f, 1.0f);
			yield return new WaitForSeconds(0.05f);
		}

		int largest = Mathf.Max(Mathf.Max(Mathf.Max(p1, p2), p3), p4);

		List<string> winners = new List<string>();

		if (p1 == largest)
			winners.Add("<color=#ff1f1f>Player One</color>");
		if (p2 == largest)
			winners.Add("<color=#006fff>Player Two</color>");
		if (p3 == largest)
			winners.Add("<color=#ffff2f>Player Three</color>");
		if (p4 == largest)
			winners.Add("<color=#2fff2f>Player Four</color>");

		string txt = "And the winner is...\n" + winners[0];

		if (winners.Count != 1)
		{
			for (int i = 1; i < winners.Count; ++i)
				txt += " and " + winners[i];
		}

		if (winners.Count == 4)
			txt = "And the winner is...\nNobody! It's a draw!";

		winnerText.text = txt + "!";

		Vector3 pos = winnerText.rectTransform.position;

		for (float t = 4.0f; t > -1.0f; t -= Time.deltaTime * 2.0f)
		{
			Color col = winnerText.color;
			col.a = t;
			winnerText.color = col;

			winnerText.rectTransform.position = pos + (Vector3)Random.insideUnitCircle * 2.5f;

			yield return new WaitForEndOfFrame();
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void PlayerDead(Player player)
	{
		deadPlayers.Add(player);

		if(deadPlayers.Count == 4)
		{
			GameEnd();
		}
	}

	private void Update()
	{
		if(Input.GetMouseButton(0))
		{
			int rand = Random.Range(0, tiles.Count);

			tiles[rand].HitWithExplosion(tiles[rand].transform.position - new Vector3(0.0f, 1.0f, 0.0f), 10.0f, 1.0f);
		}
	}
}