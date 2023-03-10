using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess_Pao : Chess_Base
{
    public override void Awake()
    {
        base.Awake();
        relativeStep = new Vector2[] { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
    }
    public override List<Vector2> AccessiblePoints()
    {
        accessiblePoints.Clear();
        foreach (Vector2 direction in relativeStep)
        {
            // 相邻位置有棋子，直接越过寻路
            if (GridManager.grid2chess.ContainsKey(currentPos + direction))
            {
                StrideChess(currentPos + direction, direction);
                continue;
            }

            Vector2 nextPos = currentPos;
            while (CheBasicJudge(nextPos + direction))
            {
                nextPos += direction;
                // 遇到棋子时，不能吃，但需搜索该方向后面的棋子
                if (GridManager.grid2chess.ContainsKey(nextPos))
                {
                    Debug.Log("炮架：" + nextPos);
                    StrideChess(nextPos, direction);
                    break;
                }
                accessiblePoints.Add(nextPos);
            }
        }
        return accessiblePoints;
    }

    /// <summary>
    /// 沿着炮架寻找对方棋子
    /// </summary>
    /// <param name="炮架坐标"></param>
    /// <param name="寻路方向"></param>
    /// <returns></returns>
    private bool StrideChess(Vector2 carrigePos, Vector2 direction)
    {
        Vector2 nextPos = carrigePos;
        while (CheBasicJudge(nextPos + direction))
        {
            nextPos += direction;
            if (GridManager.grid2chess.ContainsKey(nextPos))
            {
                if (GridManager.grid2chess[nextPos].GetComponent<Chess_Base>().chessColor != this.chessColor)
                {
                    accessiblePoints.Add(nextPos);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 仅判断是否在棋盘内，遇到对方棋子也需考虑 
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool CheBasicJudge(Vector2 point)
    {
        if (point.x < 10 && point.x >= 0 && point.y < 9 && point.y >= 0)
            return true;
        return false;
    }
}
