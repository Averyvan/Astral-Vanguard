using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject physicsBox;
	public GameObject enemy;
	public TextMesh livesCounter;
	public TextMesh scoreCounter;
	public TextMesh highScoreCounter;
	public AudioClip startSound;
	public int startingLives = 3;
	public Canvas mainMenu;
	AudioSource source;
	Player player;
	Sweeper sweeper;
	int lives;
	Vector2 center = Vector2.zero;
	Vector2 offScreen = new Vector2(100, 0);
	bool isGameActive = false;
	float roundTime;
	float timeOfDeath;
	int score;
	int highScore;
	int enemiesSpawned;
	Vector2[] spawnPositions;
	float[] spawnRotations;
    
    void Awake()
    {
		source = GetComponent<AudioSource>();
		player = FindObjectOfType<Player>();
		sweeper = FindObjectOfType<Sweeper>();
		lives = startingLives;
		roundTime = 0;
		enemiesSpawned = 0;
		spawnPositions = new Vector2[4];
		spawnPositions[0] = new Vector2(-5.15f, 2.8f);
		spawnPositions[1] = new Vector2(5.15f, 2.8f);
		spawnPositions[2] = new Vector2(-5f, -2.51f);
		spawnPositions[3] = new Vector2(5f, -2.51f);

		spawnRotations = new float[4];
		spawnRotations[0] = -135;
		spawnRotations[1] = 135;
		spawnRotations[2] = -45;
		spawnRotations[3] = 45;
	}

	public void StartGame()
	{
		ResetScore();
		mainMenu.enabled = false;
		player.StartGame();
		source.PlayOneShot(startSound);
		isGameActive = true;
	}

	public void ResetGame()
	{
		mainMenu.enabled = true;
		player.transform.position = center;
		player.ResetGame();
		lives = startingLives;
		livesCounter.text = "Lives: " + lives;
		roundTime = 0;
		enemiesSpawned = 0;
	}

	public void PlayerDeath()
	{
		source.Play();
		isGameActive = false;
		lives--;
		livesCounter.text = "Lives: " + lives;
		player.transform.position = offScreen;
		sweeper.Sweep();
		timeOfDeath = Time.time;
	}

	public void AddScore(int additionalScore)
	{
		score += additionalScore;
		scoreCounter.text = "Score: " + score;
		if (score > highScore)
		{
			highScore = score;
			highScoreCounter.text = "Highscore: " + highScore;
		}
	}

	void ResetScore()
	{
		score = 0;
		scoreCounter.text = "Score: ";
	}
	
	void Update()
    {
		if(isGameActive)
		{
			roundTime += Time.deltaTime;
			if (Mathf.Pow(roundTime, 1.5f)/(5*Mathf.Log(roundTime+1)) > enemiesSpawned)
			{
				int corner = (int)(Random.value*100 % 4);
				GameObject newEnemy = Instantiate(enemy, physicsBox.transform, true);
				newEnemy.transform.position = spawnPositions[corner];
				newEnemy.transform.Rotate(newEnemy.transform.up, spawnRotations[corner]);
				++enemiesSpawned;
			}
		}
		else if (!isGameActive && !mainMenu.enabled && lives > 0 && Time.time - timeOfDeath > 3)
		{
			isGameActive = true;
			player.transform.position = center;
			player.Revive();
		}
		else if (!isGameActive && !mainMenu.enabled && lives <= 0 && Time.time - timeOfDeath > 4)
		{
			ResetGame();
		}
    }
}
