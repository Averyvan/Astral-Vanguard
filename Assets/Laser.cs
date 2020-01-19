using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public float speed = 5f;
	public float damage = 20f;
	float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
		spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
		if (Time.time - spawnTime > 5) Destroy(gameObject);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
