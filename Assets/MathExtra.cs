using UnityEngine;
using System.Collections;

public static class MathExtra {
	public static float GetV2L(Vector2 v)
	{
		return MathExtra.FastSqrt(Mathf.Pow(v.x,2f)+Mathf.Pow(v.y,2f));
	}
	public static float GetV3L(Vector3 v)
	{
		return MathExtra.FastSqrt(v.x*v.x+v.y*v.y+v.z*v.z);
	}
	public static Vector3 FastNormalize(Vector3 v)
	{
		return v*InverseSqrtFast (v.sqrMagnitude);
	}
	public static Vector2 FastNormalize(Vector2 v)
	{
		return v*InverseSqrtFast (v.sqrMagnitude);
	}
	public static float FastSqrt(float x)
	{
		unsafe  
		{  
			int i;
			float x2, y;
			const float threehalfs = 1.5F;
			x2 = x * 0.5F;
			y  = x;
			i  = * ( int * ) &y;     
			i  = 0x5f375a86 - ( i >> 1 ); 
			y  = * ( float * ) &i;
			y  = y * ( threehalfs - ( x2 * y * y ) ); 
			//y  = y * ( threehalfs - ( x2 * y * y ) );  	
			//y  = y * ( threehalfs - ( x2 * y * y ) ); 
			return x*y;
		}
	}
	public static float InverseSqrtFast(float x)  
	{  
		unsafe  
		{  
			float xhalf = 0.5f * x;  
			int i = *(int*)&x;          
			i = 0x5f375a86 - (i >> 1);     
			x = *(float*)&i;               
			x = x * (1.5f - xhalf * x * x); 
			return x;  
		}  
	}  
	public static bool IsSameSymbol(float x,float y)
	{
		if (x > 0 && y > 0)
			return true;
		if (x < 0 && y < 0)
			return true;
		return false;
	}
	public static float Dot (Vector2 v1,Vector2 v2)
	{
		return v1.x * v2.x + v1.y * v2.y;
	}
	public static float Dot (Vector3 v1,Vector3 v2)
	{
		return v1.x * v2.x + v1.y * v2.y+v1.z*v2.z;
	}
}
