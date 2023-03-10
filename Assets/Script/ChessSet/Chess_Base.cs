using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SelectEventHandler();
public delegate void EatenEventHandler(GameObject chess);
public enum ChessColor
{
    红,
    黑
}
public enum ChessState
{
    Free,       // 自由，可被选择可移动
    Selected,   // 被选择
    // Moving,     // 移动中
    Died        // 被吃的子，不可用
}
public enum ChessCharacter
{
    将, 帅 = 0,
    车 = 1,
    象, 相 = 2,
    马 = 3,
    炮 = 4,
    士 = 5,
    卒, 兵 = 6
}

public abstract class Chess_Base : MonoBehaviour, IPointerClickHandler
{
    public static event SelectEventHandler SelectedEvent;   // 选中某棋子时，通知其他棋子取消选中
    public static event EatenEventHandler EatenEvent;

    public static GameObject selectedChess;     // 存储当前选中的棋子

    [SerializeField]
    public ChessColor chessColor;
    [SerializeField]
    public ChessState chessState;
    [SerializeField]
    public ChessCharacter chessCharacter;

    public Vector2 currentPos;  // 由InitChessGame初始化，移动时需要修改
    protected Vector2[] relativeStep;
    public List<Vector2> accessiblePoints;

    /// <summary>
    /// 实例化后订阅事件
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
        // 点击右键取消选中
        if (Input.GetMouseButtonDown(1) && selectedChess == this.gameObject)
        {
            CancelSelect();
            selectedChess = null;
        }
    }

    /// <summary>
    /// 选中棋子，棋子圈选提示、着点标绿提示
    /// </summary>
    public void BeSelected()
    {
        Debug.Log(this.gameObject.name + " 被选中");
        // 可着点变绿
        AccessiblePoints();
        GridManager.PointTurnGreen(accessiblePoints);
        // 状态为 被选中
        chessState = ChessState.Selected;

        selectedChess = this.gameObject;
    }

    /// <summary>
    /// 该棋子被点击时
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        // 当前棋子状态为Free且为运棋方颜色时
        if (chessState == ChessState.Free && (int)GameController.moveSide == (int)chessColor)
        {
            BeSelected();
            SelectedEvent();    // 通知其他棋子
        }
        else
        {
            CancelSelect();
        }
    }

    /// <summary>
    /// 被吃了，交给ChessManager管理
    /// </summary>
    /// <param name="chess"></param>
    public void BeEaten(GameObject chess)
    {
        if (chess != this.gameObject)
            return;
        Debug.Log(this.gameObject.name + "被吃了");
        chessState = ChessState.Died;
        ChessManager.Died(chess);
    }

    public void CancelSelect()
    {
        if (chessState == ChessState.Selected && selectedChess!=this.gameObject)
        {
            Debug.Log("取消选中" + this.gameObject.name);
            chessState = ChessState.Free;
        }
    }

    /// <summary>
    /// 订阅事件:选择其他棋子、被其他棋子吃
    /// </summary>
    /// <param name="chess"></param>
    public void SubscribeEvents(GameObject chess)
    {
        if (chess == this.gameObject)
        {
            SelectedEvent += new SelectEventHandler(CancelSelect);  // 收到其他棋子被选中事件时，自身取消被选中
            EatenEvent += new EatenEventHandler(BeEaten);
        }
    }

    /// <summary>
    /// 棋子被吃后，取消订阅事件
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
    /// 被PointClick的点击事件调用，移动到目标位置
    /// </summary>
    /// <param name="targetPos"></param>
    public void Move(Vector2 targetPos)
    {
        if (chessState != ChessState.Selected || accessiblePoints == null)
            return;
        if (accessiblePoints.Contains(targetPos))
        {
            // 目标着点有对方棋子(已判断过非友方棋子)
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
    /// 基本判断:是否在棋盘范围、是否有其他棋子、是否非友方棋子
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
    /// 更新并返回可着点坐标表，不同棋种重载此函数
    /// </summary>
    /// <returns></returns>
    public abstract List<Vector2> AccessiblePoints();

}
