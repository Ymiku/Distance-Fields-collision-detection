using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMove : MonoBehaviour {
	public float time = 0f;
	void Start()
	{
		Invoke ("ChangeActive",1f);
	}
	void ChangeActive()
	{
		gameObject.SetActive (!gameObject.activeSelf);
		Invoke ("ChangeActive",1f);
	}
}
