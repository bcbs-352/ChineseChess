using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Bing : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
    }
    public override List<Vector2> AccessiblePoints()
    {
        accessiblePoints.Clear();
        if (this.chessColor == ChessColor.ºÚ)
        {
            relativeStep = new Vector2[] { new Vector2(1,0) };
            if(currentPos.x > 4)
                relativeStep = new Vector2[] { new Vector2(1, 0),new Vector2(0,1),new Vector2(0,-1) };
        }
        else if(this.chessColor == ChessColor.ºì)
        {
            relativeStep = new Vector2[] { new Vector2(-1, 0) };
            if (currentPos.x < 5)
                relativeStep = new Vector2[] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        }
        foreach(Vector2 direction in relativeStep)
        {
            if (BasicJudge(currentPos + direction))
                accessiblePoints.Add(currentPos + direction);
        }
        return accessiblePoints;
    }

}
