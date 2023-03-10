using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Xiang : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
        relativeStep = new Vector2[] { new Vector2(2, 2), new Vector2(2, -2), new Vector2(-2, -2), new Vector2(-2, 2) };
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
                if (nextPos.x <= 4)
                    accessiblePoints.Add(nextPos);
            }
            else if (this.chessColor == ChessColor.ºì)
            {
                if (nextPos.x >= 5)
                {
                    accessiblePoints.Add(nextPos);
                }
            }
        }
        return accessiblePoints;
    }

}
