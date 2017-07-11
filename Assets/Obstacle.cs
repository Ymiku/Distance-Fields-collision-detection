using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacle {
	float ClosestDisOnBounds(Vector2 v,bool debug = false);
	float GetSqrRange ();
	Vector2 GetOri ();
}
