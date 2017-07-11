using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFields : UnitySingleton<DistanceFields> {
	public Vector2 originPoint;
	public float radius;
	public int samplesPerUnit = 20;
	public int fieldLength;
	public bool isVisual = true;
	public SpriteRenderer visual;
	public float[,] fieldsMap;
	public List<IObstacle> staticObstacleList = new List<IObstacle> ();
	public List<IObstacle> dynamicObstacleList = new List<IObstacle> ();
	public List<IObstacle> activeObstacleList = new List<IObstacle> ();
	public IObstacle selfObs=null;
	private float delta;
	// Use this for initialization
	void Start () {
		PreCompute ();
	}
	public void PreCompute()
	{
		int mapLength = fieldLength * samplesPerUnit;
		delta = 1f / samplesPerUnit;
		fieldsMap = new float[mapLength,mapLength];
		Texture2D tex2D = new Texture2D (mapLength*samplesPerUnit,mapLength*samplesPerUnit,TextureFormat.ARGB32,false);
		float color;
		if (isVisual) {
			for (int col = 0; col < mapLength; col++) {
				for (int cul = 0; cul < mapLength; cul++) {
					color = fieldsMap [cul, col] = CalculateDis (new Vector2 (originPoint.x + cul * delta, originPoint.y + col * delta));
					tex2D.SetPixel (cul, col, new Color (color, color, color));
				}
			}
			tex2D.Apply ();
			Sprite sprite = Sprite.Create (tex2D, new Rect (0, 0, mapLength, mapLength), Vector2.one * 0.5f);
			visual.sprite = sprite;
		} else {
			for (int col = 0; col < mapLength; col++) {
				for (int cul = 0; cul < mapLength; cul++) {
					fieldsMap [cul, col] = CalculateDis (new Vector2 (originPoint.x + cul * delta, originPoint.y + col * delta));
				}
			}
		}
	}
	public float QueryWorld(Vector2 worldPos)
	{
		return (1f-GetPixelBilinear (worldPos))*radius;
	}
	public Vector2 QueryNormal(Vector2 worldPos)
	{
		float left = GetPixelBilinear (worldPos+new Vector2(-0.1f,0f));
		float right = GetPixelBilinear (worldPos+new Vector2(0.1f,0f));
		float top = GetPixelBilinear (worldPos+new Vector2(0f,0.1f));
		float bottom = GetPixelBilinear (worldPos+new Vector2(0f,-0.1f));
		float x = (left - right) * 0.5f;
		float y = (bottom - top) * 0.5f;
		return MathExtra.FastNormalize (new Vector2(x,y));
	}
	public float GetPixelBilinear(Vector2 pos)
	{
		float fx = (pos.x - originPoint.x) * samplesPerUnit;
		float fy = (pos.y - originPoint.y) * samplesPerUnit;
		int x = (int)((pos.x - originPoint.x) * samplesPerUnit);
		int y = (int)((pos.y - originPoint.y) * samplesPerUnit);
		int x2 = x + 1;
		int y2 = y + 1;
		x = Mathf.Clamp (x,0,fieldLength*samplesPerUnit-1);
		y= Mathf.Clamp (y,0,fieldLength*samplesPerUnit-1);
		x2 = Mathf.Clamp (x2,0,fieldLength*samplesPerUnit-1);
		y2= Mathf.Clamp (y2,0,fieldLength*samplesPerUnit-1);
		float lerpx1 = fieldsMap [x, y]*(x2-fx) + fieldsMap [x + 1, y]*(fx-x);
		float lerpx2 = fieldsMap [x, y+1]*(x2-fx) + fieldsMap [x + 1, y+1]*(fx-x);
		float staticFinal = lerpx1*(y2-fy)+lerpx2*(fy-y);

		return Mathf.Max(staticFinal,GetDynamic(pos));
	}
	public float GetDynamic(Vector2 pos)
	{
		float max = 0f;
		float temp;
		for (int i = 0; i < activeObstacleList.Count; i++) {
			temp = 1f - Mathf.Min (activeObstacleList [i].ClosestDisOnBounds (pos,true), radius) / radius;
			max = Mathf.Max (temp,max);
		}
		return max;
	}
	public Vector2 Move(Vector2 ori,float rad,Vector2 dir)
	{
		activeObstacleList.Clear ();
		for (int i = 0; i < dynamicObstacleList.Count; i++) {
			if ((dynamicObstacleList [i].GetOri () - ori).sqrMagnitude <= dynamicObstacleList [i].GetSqrRange () + rad * rad + dir.sqrMagnitude+1f) {
				activeObstacleList.Add (dynamicObstacleList [i]);
			}
		}
		Vector2 newPos = ori + dir;
		float dist = 0f;
		for (int i = 0; i < 3; i++) {
			dist = QueryWorld(newPos);
			if (dist >= rad)
				break;
			newPos = ori + MathExtra.FastNormalize(newPos+QueryNormal (newPos)*(rad-dist)-ori)*4f*Time.deltaTime;
		}
		if(Vector2.Dot(newPos-ori,dir)<0f)
			return ori;
		return newPos;
	}
	private float CalculateDis(Vector2 v)
	{
		float max = 0f;
		for (int i = 0; i < staticObstacleList.Count; i++) {
			max = Mathf.Max(max,1f-Mathf.Min(staticObstacleList [i].ClosestDisOnBounds (v),radius)/radius);
		}
		return max;
	}
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (new Vector3(fieldLength/2f,fieldLength/2f,0f),Vector3.one*fieldLength);
	}
}
