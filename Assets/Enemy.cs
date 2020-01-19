using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float speed = 2f;
	public GameObject laser;
	bool isActive = true;
	float health;
	AudioSource source;
	Transform playerTransform;
	Rigidbody2D playerRigidBody;
	Rigidbody2D rb;
	SpriteRenderer sr;
	float timeOfShot;
	readonly int pointValue = 100;

	void Awake()
    {
		health = 100f;
		source = GetComponent<AudioSource>();
		playerTransform = FindObjectOfType<Player>().transform;
		playerRigidBody = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponentInChildren<SpriteRenderer>();
		timeOfShot = Time.time;
    }

	public void TakeDamage(int damage)
	{
		if (isActive)
		{
			health -= damage;
			if (health <= 0)
			{
				gameObject.transform.root.GetComponent<GameMaster>().AddScore(pointValue);
				Destroy(gameObject);
			}
			else
			{
				sr.color = Color.Lerp(Color.red, Color.white, 0.9f);
			}
		}
	}
	
    void Update()
    {
		if (isActive)
		{
			Vector2 targetAngle;
			if (Vector2.Distance(transform.position, playerTransform.position) > playerRigidBody.velocity.magnitude) //if farther than velocity, lead target
				targetAngle = ((Vector2)playerTransform.position + playerRigidBody.velocity - (Vector2)transform.position);
			else
				targetAngle = (playerTransform.position - transform.position);
			transform.up = (Vector2)Vector3.RotateTowards(transform.up, targetAngle, 2*Mathf.PI * Time.deltaTime, 0);

			if (Vector2.Distance(transform.up.normalized, targetAngle.normalized) < .3f) //if facing enemy, boost forward
				rb.AddForce(transform.up * speed * Time.deltaTime * 50);

			if (Time.time - timeOfShot > 2 && Vector2.Distance(transform.up.normalized, targetAngle.normalized) < .3f && Vector2.Distance(transform.position, playerTransform.position) < 10)
			{
				Instantiate(laser, transform.position + (transform.up * 0.1f), transform.rotation).transform.SetParent(gameObject.transform.parent);
				source.Play();
				timeOfShot = Time.time;
			}

			sr.color = Color.Lerp(sr.color, Color.red, Time.deltaTime * 5f);
		}
	}

	//flight model brainstorming:
	//-max turn speed
	//-tracking delay, check position every x seconds rather than every frame
}
