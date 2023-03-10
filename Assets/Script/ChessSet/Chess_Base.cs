using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SelectEventHandler();
public delegate void EatenEventHandler(GameObject chess);
public enum ChessColor
{
    ��,
    ��
}
public enum ChessState
{
    Free,       // ���ɣ��ɱ�ѡ����ƶ�
    Selected,   // ��ѡ��
    // Moving,     // �ƶ���
    Died        // ���Ե��ӣ�������
}
public enum ChessCharacter
{
    ��, ˧ = 0,
    �� = 1,
    ��, �� = 2,
    �� = 3,
    �� = 4,
    ʿ = 5,
    ��, �� = 6
}

public abstract class Chess_Base : MonoBehaviour, IPointerClickHandler
{
    public static event SelectEventHandler SelectedEvent;   // ѡ��ĳ����ʱ��֪ͨ��������ȡ��ѡ��
    public static event EatenEventHandler EatenEvent;

    public static GameObject selectedChess;     // �洢��ǰѡ�е�����

    [SerializeField]
    public ChessColor chessColor;
    [SerializeField]
    public ChessState chessState;
    [SerializeField]
    public ChessCharacter chessCharacter;

    public Vector2 currentPos;  // ��InitChessGame��ʼ�����ƶ�ʱ��Ҫ�޸�
    protected Vector2[] relativeStep;
    public List<Vector2> accessiblePoints;

    /// <summary>
    /// ʵ���������¼�
    /// </summary>
    public virtual void Awake()
    {
        chessState = ChessState.Free;
        ChessManager.SpawnEvent += SubscribeEvents;
        ChessManager.RespawnEvent += SubscribeEvents;
        ChessManager.DiedEvent += CancelSubscribeEvents;

        accessiblePoints = new List<Vector2>();
    }

    public void Update()
    {
        // ����Ҽ�ȡ��ѡ��
        if (Input.GetMouseButtonDown(1) && selectedChess == this.gameObject)
        {
            CancelSelect();
            selectedChess = null;
        }
    }

    /// <summary>
    /// ѡ�����ӣ�����Ȧѡ��ʾ���ŵ������ʾ
    /// </summary>
    public void BeSelected()
    {
        Debug.Log(this.gameObject.name + " ��ѡ��");
        // ���ŵ����
        AccessiblePoints();
        GridManager.PointTurnGreen(accessiblePoints);
        // ״̬Ϊ ��ѡ��
        chessState = ChessState.Selected;

        selectedChess = this.gameObject;
    }

    /// <summary>
    /// �����ӱ����ʱ
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        // ��ǰ����״̬ΪFree��Ϊ���巽��ɫʱ
        if (chessState == ChessState.Free && (int)GameController.moveSide == (int)chessColor)
        {
            BeSelected();
            SelectedEvent();    // ֪ͨ��������
        }
        else
        {
            CancelSelect();
        }
    }

    /// <summary>
    /// �����ˣ�����ChessManager����
    /// </summary>
    /// <param name="chess"></param>
    public void BeEaten(GameObject chess)
    {
        if (chess != this.gameObject)
            return;
        Debug.Log(this.gameObject.name + "������");
        chessState = ChessState.Died;
        ChessManager.Died(chess);
    }

    public void CancelSelect()
    {
        if (chessState == ChessState.Selected && selectedChess!=this.gameObject)
        {
            Debug.Log("ȡ��ѡ��" + this.gameObject.name);
            chessState = ChessState.Free;
        }
    }

    /// <summary>
    /// �����¼�:ѡ���������ӡ����������ӳ�
    /// </summary>
    /// <param name="chess"></param>
    public void SubscribeEvents(GameObject chess)
    {
        if (chess == this.gameObject)
        {
            SelectedEvent += new SelectEventHandler(CancelSelect);  // �յ��������ӱ�ѡ���¼�ʱ������ȡ����ѡ��
            EatenEvent += new EatenEventHandler(BeEaten);
        }
    }

    /// <summary>
    /// ���ӱ��Ժ�ȡ�������¼�
    /// </summary>
    /// <param name="chess"></param>
    public void CancelSubscribeEvents(GameObject chess)
    {
        if (chess == this.gameObject)
        {
            SelectedEvent -= CancelSelect;
            EatenEvent -= BeEaten;
        }
    }

    /// <summary>
    /// ��PointClick�ĵ���¼����ã��ƶ���Ŀ��λ��
    /// </summary>
    /// <param name="targetPos"></param>
    public void Move(Vector2 targetPos)
    {
        if (chessState != ChessState.Selected || accessiblePoints == null)
            return;
        if (accessiblePoints.Contains(targetPos))
        {
            // Ŀ���ŵ��жԷ�����(���жϹ����ѷ�����)
            if (GridManager.grid2chess.ContainsKey(targetPos))
            {
                EatenEvent(GridManager.grid2chess[targetPos]);
            }
            currentPos = targetPos;
            GridManager.Instance.PlaceChess(this.gameObject, targetPos);
            chessState = ChessState.Free;
            selectedChess = null;
            GridManager.PointTurnGreen(null);
            GameController.Step();
        }
    }

    /// <summary>
    /// �����ж�:�Ƿ������̷�Χ���Ƿ����������ӡ��Ƿ���ѷ�����
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    protected bool BasicJudge(Vector2 point)
    {
        if (point.x < 10 && point.x >= 0 && point.y < 9 && point.y >= 0)
        {
            if (!GridManager.grid2chess.ContainsKey(point))
                return true;
            if (GridManager.grid2chess[point].GetComponent<Chess_Base>().chessColor != this.chessColor)
                return true;
        }
        return false;
    }

    /// <summary>
    /// ���²����ؿ��ŵ��������ͬ�������ش˺���
    /// </summary>
    /// <returns></returns>
    public abstract List<Vector2> AccessiblePoints();

}
