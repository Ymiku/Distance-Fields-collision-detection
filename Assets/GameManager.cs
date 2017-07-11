using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager> {
	public List<AI> aiList = new List<AI> ();
	public float time;
	public float deltaTime = 0f;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		
	}
	public void Creat(int count)
	{
		for (int i = 0; i < count; i++) {
			GameObject.Instantiate (prefab,new Vector3(20f,16f,0f),Quaternion.Euler(Vector3.zero));
		}
	}
	// Update is called once per frame
	void Update () {
		time = Time.realtimeSinceStartup;
		for (int i = 0; i < aiList.Count; i++) {
			aiList [i].Execute ();
		}
		deltaTime = (Time.realtimeSinceStartup -time)*1000f;
	}
	void OnGUI()
	{
		GUI.TextArea (new Rect(0f,0f,180f,40f),deltaTime.ToString()+"ms");
	}
}
