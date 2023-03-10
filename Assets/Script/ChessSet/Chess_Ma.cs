using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Ma : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
        relativeStep = new Vector2[] {
            new Vector2(1, 2), new Vector2(2, 1),
            new Vector2(2, -1), new Vector2(1, -2),
            new Vector2(-1, -2), new Vector2(-2, -1),
            new Vector2(-2, 1), new Vector2(-1, 2) };
    }
    public override List<Vector2> AccessiblePoints()
    {
        accessiblePoints.Clear();
        foreach (Vector2 step in relativeStep)
        {
            Vector2 nextPos = step + currentPos;
            if (!BasicJudge(nextPos))
                continue;
            accessiblePoints.Add(nextPos);
        }
        return accessiblePoints;
    }

}
