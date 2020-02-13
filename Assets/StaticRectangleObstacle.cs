using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRectangleObstacle : MonoBehaviour,IStaticObstacle {
	// Use this for initialization
	public float width;
	public float height;
	private Vector2 _upRightVertex;
	private Vector2 _center;
	private Vector3 _oldScale;
	void Awake()
	{
		DistanceFields.Instance.staticObstacleList.Add (this);
		_oldScale = transform.localScale;
		width = _oldScale.x;
		height = _oldScale.y;
		transform.localScale = Vector3.one;
		_upRightVertex = new Vector2(width*0.5f,height*0.5f);
		_center = transform.TransformPoint (Vector3.zero);
		transform.localScale = _oldScale;
	}
	/// <summary>
	/// 在预计算中获取点到碰撞体的最近距离
	/// </summary>
	public float ClosestDisOnBounds(Vector2 point,bool debug = false)
	{
		transform.localScale = Vector3.one;
		Vector2 _point = (Vector2)transform.InverseTransformPoint(new Vector3(point.x,point.y,0f));
		transform.localScale = _oldScale;
		_point = new Vector2 (Mathf.Abs(_point.x),Mathf.Abs(_point.y));
		Vector2 v = _point - _center;
		Vector2 h = _upRightVertex - _center;
		Vector2 u = v - h;
		u = new Vector2 (Mathf.Max(u.x,0f),Mathf.Max(u.y,0f));
		return MathExtra.GetV2L (u);
	}
    public float GetSqrRange()
    {
        return Mathf.Sqrt(width*width+height*height);
    }
    public Vector2 GetOri()
    {
        return (Vector2)transform.position;
    }
}
