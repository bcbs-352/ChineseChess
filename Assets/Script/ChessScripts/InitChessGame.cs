using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 棋局初始化：棋子预制体实例化，棋盘着点grid初始化
// 由GameController调用，棋局初始化后交给ChessManager 和 GridManager管理
public class InitChessGame : MonoBehaviour
{
    #region 预制体
    public GameObject 红兵;
    public GameObject 红士;
    public GameObject 红帅;
    public GameObject 红炮;
    public GameObject 红相;
    public GameObject 红车;
    public GameObject 红马;
    public GameObject 黑卒;
    public GameObject 黑士;
    public GameObject 黑将;
    public GameObject 黑炮;
    public GameObject 黑象;
    public GameObject 黑车;
    public GameObject 黑马;
    #endregion
    public static GameObject[] AllChess;
    public static Vector2[] OriginalPos;
    void Awake()
    {
        AllChess = new GameObject[] {
            Instantiate(黑车),Instantiate(黑马),Instantiate(黑象),Instantiate(黑士),Instantiate(黑将),Instantiate(黑士),Instantiate(黑象),Instantiate(黑马),Instantiate(黑车),
            Instantiate(黑炮),Instantiate(黑炮),Instantiate(黑卒),Instantiate(黑卒),Instantiate(黑卒),Instantiate(黑卒),Instantiate(黑卒),
            Instantiate(红兵),Instantiate(红兵),Instantiate(红兵),Instantiate(红兵),Instantiate(红兵),Instantiate(红炮),Instantiate(红炮),
            Instantiate(红车),Instantiate(红马),Instantiate(红相),Instantiate(红士),Instantiate(红帅),Instantiate(红士),Instantiate(红相),Instantiate(红马),Instantiate(红车)
        };
        OriginalPos = new Vector2[]
        {
            new Vector2(0,0), new Vector2(0,1), new Vector2(0,2), new Vector2(0,3), new Vector2(0,4), new Vector2(0,5), new Vector2(0,6), new Vector2(0,7), new Vector2(0,8),
            new Vector2(2,1), new Vector2(2,7), new Vector2(3,0), new Vector2(3,2), new Vector2(3,4), new Vector2(3,6), new Vector2(3,8),
            new Vector2(6,0), new Vector2(6,2), new Vector2(6,4), new Vector2(6,6), new Vector2(6,8), new Vector2(7,1), new Vector2(7,7),
            new Vector2(9,0), new Vector2(9,1), new Vector2(9,2), new Vector2(9,3), new Vector2(9,4), new Vector2(9,5), new Vector2(9,6), new Vector2(9,7), new Vector2(9,8)
        };
        
    }

    public void BtnDown()
    {
        InitAllChess();
        GameController.Init();
    }

    /// <summary>
    /// 初始化/复位棋子位置
    /// </summary>
    public void InitAllChess()
    {
        ChessManager.livePool.Clear();
        ChessManager.diedPool.Clear();
        
        for (int i = 0; i < AllChess.Length; ++i)
        {
            GridManager.Instance.PlaceChess(AllChess[i], OriginalPos[i]);           // 放置棋子
            Chess_Base tmpClass = AllChess[i].GetComponent<Chess_Base>();

            // 如果棋子死亡,需要Respawn重置，但因为是初始化，所以死亡的棋子要手动取消订阅事件,再统一Spawn
            // 此处可能出Bug
            if (tmpClass.chessState == ChessState.Died)
                tmpClass.CancelSubscribeEvents(AllChess[i]);                        
            ChessManager.Spawn(AllChess[i]);        // 生成棋子事件

            tmpClass.currentPos = OriginalPos[i];   // 设置坐标
            tmpClass.chessState=ChessState.Free;    // 设置状态
        }
        GridManager.Instance.UpdateGridData();  // 有点乱，之后统一静态类
    }
}
