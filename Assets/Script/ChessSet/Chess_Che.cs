using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Che : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
        relativeStep = new Vector2[] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
    }
    public override List<Vector2> AccessiblePoints()
    {
        accessiblePoints.Clear();
        foreach(Vector2 direction in relativeStep)
        {
            Vector2 nextPos = currentPos;
            while (BasicJudge(nextPos+direction))
            {
                // 沿着一个方向走
                nextPos += direction;
                if (GridManager.grid2chess.ContainsKey(nextPos))
                {
                    // 遇到棋子，为对方可吃，为己方则不再前进
                    if(GridManager.grid2chess[nextPos].GetComponent<Chess_Base>().chessColor != this.chessColor)
                    {
                        accessiblePoints.Add(nextPos);
                    }
                    break;
                }
                accessiblePoints.Add(nextPos);
            }
        }
        return accessiblePoints;
    }

}
