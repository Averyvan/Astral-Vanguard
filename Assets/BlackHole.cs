using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
	public float power = 500f;
	public float speed = 2f;
	GameObject physicsBox;
	Rigidbody2D[] rigidbodies;
	float timeStart;
	bool isFlying = true;
    
    void Start()
    {
		physicsBox = transform.parent.gameObject;
		rigidbodies = new Rigidbody2D[50];
		timeStart = Time.time;
    }
	
    void Update()
    {
		if (isFlying)
		{
			transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
			if (Time.time - timeStart > 1f) isFlying = false;
		}
		rigidbodies = physicsBox.GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D i in rigidbodies)
		{
			if (i.tag == "Player" && isFlying || i.tag == "Black Hole")
			{
				continue;
			}
			float force;
			if (Vector2.Distance(transform.position, i.position) > 0.5f)
				force = power / Mathf.Pow(Vector2.Distance(transform.position, i.position), 2);
			else
				force = power;
			if (i.tag == "Player") force /= 4f;
			i.AddForce(((Vector2)transform.position - i.position) * force * Time.deltaTime, ForceMode2D.Force);
			if (Vector2.Distance(transform.position, i.position) < 0.25f) i.SendMessage("TakeDamage", 50);
		}
		if (Time.time - timeStart > 10) Destroy(gameObject);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Enemy" || collision.tag == "Wall")
			isFlying = false;
	}
}
