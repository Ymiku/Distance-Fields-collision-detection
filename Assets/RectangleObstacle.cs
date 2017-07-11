using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleObstacle : MonoBehaviour,IObstacle {
	// Use this for initialization
	public bool isStatic = true;
	public float width;
	public float height;
	public float sqrRange;
	private float _range;
	private Vector2 _upRightVertex;
	void Awake()
	{
		width = transform.localScale.x;
		height = transform.localScale.y;
		_upRightVertex = new Vector2(width*0.5f,height*0.5f);
		sqrRange = new Vector2 (width*0.5f,height*0.5f).sqrMagnitude;
		_range = MathExtra.FastSqrt (sqrRange);
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
		if ((point - (Vector2)transform.position).sqrMagnitude > Mathf.Pow(_range+DistanceFields.Instance.radius,2f))
			return 100f;
		Vector2 _point = (Vector2)transform.InverseTransformPoint(new Vector3(point.x,point.y,0f));
		_point = new Vector2(_point.x*transform.localScale.x,_point.y*transform.localScale.y);
		_point = new Vector2 (Mathf.Abs(_point.x),Mathf.Abs(_point.y));
		Vector2 v = _point;
		Vector2 h = _upRightVertex;
		Vector2 u = v - h;
		u = new Vector2 (Mathf.Max(u.x,0f),Mathf.Max(u.y,0f));
		return MathExtra.GetV2L (u);
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
