using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ����Trigger����¼�
public class PointClick : MonoBehaviour,IPointerClickHandler
{
    public Vector2 gridPos;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("����ŵ�" + gridPos.ToString());
        if (Chess_Base.selectedChess == null)
        {
            Debug.Log("δѡ������");
            return;
        }
        if ((int)Chess_Base.selectedChess.GetComponent<Chess_Base>().chessColor != (int)GameController.moveSide)
        {
            Debug.Log("δ�ֵ��㷽");
            return;
        }
        if (!Chess_Base.selectedChess.GetComponent<Chess_Base>().accessiblePoints.Contains(gridPos))
        {
            Debug.Log("��������");
            return;
        }
            Chess_Base.selectedChess.GetComponent<Chess_Base>().Move(gridPos);

    }

}
