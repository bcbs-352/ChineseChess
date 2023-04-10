using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 处理Trigger点击事件
public class PointClick : MonoBehaviour,IPointerClickHandler
{
    public Vector2 gridPos;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("点击着点" + gridPos.ToString());
        if (Chess_Base.selectedChess == null)
        {
            Debug.Log("未选择棋子");
            return;
        }
        if ((int)Chess_Base.selectedChess.GetComponent<Chess_Base>().chessColor != (int)GameController.moveSide)
        {
            Debug.Log("未轮到你方");
            return;
        }
        if (!Chess_Base.selectedChess.GetComponent<Chess_Base>().accessiblePoints.Contains(gridPos))
        {
            Debug.Log("不可落子");
            return;
        }
            Chess_Base.selectedChess.GetComponent<Chess_Base>().Move(gridPos);

    }

}
