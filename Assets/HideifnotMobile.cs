﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideifnotMobile : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		if (Application.platform != RuntimePlatform.Android) gameObject.SetActive(false);
	}
}
