using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float startingHealth = 100;
	public float speed = 2f;
	public GameObject laser;
	public GameObject blackHole;
	public SpriteRenderer plume;
	public AudioClip damagedSound;
	public bool isActive = false;
	AudioSource source;
	Rigidbody2D rb;
	SpriteRenderer sr;
	GameMaster gm;
	float health;
	float timeOfShot;
	float timeOfBlackHole;

	public bool isMobileBoosting = false;
	public bool isMobileFiring = false;
	public bool isMobileFiringBH = false;

    void Awake()
    {
		health = startingHealth;
		source = GetComponent<AudioSource>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponentInChildren<SpriteRenderer>();
		gm = FindObjectOfType<GameMaster>();
		timeOfShot = 0;
		timeOfBlackHole = -20;
    }

	public void StartGame()
	{
		isActive = true;
	}

	public void ResetGame()
	{
		plume.enabled = false;
		transform.up = Vector2.up;
	}

	public void TakeDamage(int damage)
	{
		if(isActive)
		{
			health -= damage;
			source.PlayOneShot(damagedSound);
			if (health <= 0)
			{
				rb.velocity = Vector2.zero;
				rb.angularVelocity = 0;
				isActive = false;
				gm.PlayerDeath();
			}
			else
			{
				sr.color = Color.Lerp(Color.white, Color.red, 0.5f);
			}
		}
	}

	public void Revive()
	{
		health = startingHealth;
		isActive = true;
	}

	void Update()
	{
		if (isActive)
		{
			Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.up = (mouseWorldPos - (Vector2)transform.position).normalized;
			if (Input.GetButton("Jump") || isMobileBoosting)
			{
				rb.AddForce(transform.up * speed * Time.deltaTime * 50);
				plume.enabled = true;
				//consider removing rigidbodies and coding acceleration manually
			}
			else plume.enabled = false;
			if ((Input.GetButton("Fire1") || isMobileFiring) && Time.time - timeOfShot > 0.2f)
			{
				Instantiate(laser, transform.position + (transform.up * 0.1f), transform.rotation).transform.SetParent(gameObject.transform.parent);
				source.Play();
				timeOfShot = Time.time;
			}
			if ((Input.GetButton("Fire2") || isMobileFiringBH) && Time.time - timeOfBlackHole > 20)
			{
				Instantiate(blackHole, transform.position, transform.rotation).transform.SetParent(gameObject.transform.parent);
				timeOfBlackHole = Time.time;
			}
			sr.color = Color.Lerp(sr.color, Color.white, Time.deltaTime * 5f);
		}
	}

	public void SetMobileBoosting(bool state)
	{
		isMobileBoosting = state;
	}

	public void SetMobileFiring(bool state)
	{
		isMobileFiring = state;
	}

	public void SetMobileFiringBH(bool state)
	{
		isMobileFiringBH = state;
	}

}
