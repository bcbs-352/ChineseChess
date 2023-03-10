using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Shi : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
        relativeStep = new Vector2[] { new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(1, 1) };
    }
    public override List<Vector2> AccessiblePoints()
    {
        accessiblePoints.Clear();
        foreach(Vector2 step in relativeStep)
        {
            Vector2 nextPos = step + currentPos;
            if (!BasicJudge(nextPos))
                continue;
            if (this.chessColor == ChessColor.ºÚ)
            {
                if (nextPos.y >= 3 && nextPos.y <= 5 && nextPos.x >= 0 && nextPos.x <= 2)
                    accessiblePoints.Add(nextPos);
            }
            else
            {
                if (nextPos.y >= 3 && nextPos.y <= 5 && nextPos.x >= 7 && nextPos.x <= 9)
                {
                    accessiblePoints.Add(nextPos);
                }
            }
        }
        return accessiblePoints;
    }

}
