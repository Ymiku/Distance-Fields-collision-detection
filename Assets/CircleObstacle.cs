using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObstacle : MonoBehaviour,IObstacle {
	// Use this for initialization
	public bool isStatic = true;
	public float sqrRange;
	private float _range;
	private Vector2 _upRightVertex;
	void Awake()
	{
		_range = transform.localScale.x*0.5f;
		sqrRange = _range * _range;
	}
	void OnEnable()
	{
		if (isStatic) {
			DistanceFields.Instance.staticObstacleList.Add (this);
		} else {
			DistanceFields.Instance.dynamicObstacleList.Add (this);
		}
	}
	void OnDisable()
	{
		if (isStatic) {
			if(DistanceFields.Instance!=null)
			DistanceFields.Instance.staticObstacleList.Remove (this);
		} else {
			if(DistanceFields.Instance!=null)
			DistanceFields.Instance.dynamicObstacleList.Remove (this);
		}
	}
	/// <summary>
	/// 在预计算中获取点到碰撞体的最近距离
	/// </summary>
	public float ClosestDisOnBounds(Vector2 point,bool debug = false)
	{
		if (DistanceFields.Instance.selfObs == this)
			return 100f;
		return Mathf.Max(MathExtra.GetV2L (point-(Vector2)transform.position)-_range,0f);
	}
	public float GetSqrRange ()
	{
		return sqrRange;
	}
	public Vector2 GetOri ()
	{
		return (Vector2)transform.position;
	}
}
