using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour {
	// Update is called once per frame
	public float radius = 0.5f;
	public float time =0f;
	float h = 1f;
	float v = 1f;
	private IObstacle _collider;
	void OnEnable()
	{
		GameManager.Instance.aiList.Add (this);
	}
	void Start()
	{
		_collider = GetComponent<CircleObstacle> ();
		h = Random.Range (-1f,1f);
		v = Random.Range (-1f,1f);
	}
	public void Execute () {
		time += Time.deltaTime;
		if (time >= 2f) {
			h = Random.Range (-1f,1f);
			v = Random.Range (-1f,1f);
			time = 0f;
		}

		Move((Vector3.right * h*4f + Vector3.up * v*4f) * Time.deltaTime);
	}
	public void Move(Vector2 dir)
	{
		DistanceFields.Instance.selfObs = _collider;
		transform.position = DistanceFields.Instance.Move (transform.position,radius,dir);
		DistanceFields.Instance.selfObs = null;
	}
}
