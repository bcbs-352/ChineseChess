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
            // ����λ�������ӣ�ֱ��Խ��Ѱ·
            if (GridManager.grid2chess.ContainsKey(currentPos + direction))
            {
                StrideChess(currentPos + direction, direction);
                continue;
            }

            Vector2 nextPos = currentPos;
            while (CheBasicJudge(nextPos + direction))
            {
                nextPos += direction;
                // ��������ʱ�����ܳԣ����������÷�����������
                if (GridManager.grid2chess.ContainsKey(nextPos))
                {
                    Debug.Log("�ڼܣ�" + nextPos);
                    StrideChess(nextPos, direction);
                    break;
                }
                accessiblePoints.Add(nextPos);
            }
        }
        return accessiblePoints;
    }

    /// <summary>
    /// �����ڼ�Ѱ�ҶԷ�����
    /// </summary>
    /// <param name="�ڼ�����"></param>
    /// <param name="Ѱ·����"></param>
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
    /// ���ж��Ƿ��������ڣ������Է�����Ҳ�迼�� 
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
