using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{

	bool isSweeping = false;
	readonly float startX = -5.5f;
	readonly float endX = 5.5f;
	readonly float sweepTime = 3;
    
	public void Sweep()
	{
		isSweeping = true;
	}

    void Update()
    {
        if (isSweeping)
		{
			if (transform.position.x < endX)
			{
				transform.Translate(Time.deltaTime / sweepTime * (Mathf.Abs(startX) + Mathf.Abs(endX)), 0, 0);
			}
			else
			{
				transform.position = Vector2.right * startX;
				isSweeping = false;
			}
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(collision.gameObject);
	}
}
