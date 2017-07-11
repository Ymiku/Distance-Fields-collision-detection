using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaticObstacle {
	float ClosestDisOnBounds(Vector2 v);
}
