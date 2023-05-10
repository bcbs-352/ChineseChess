using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SpawnEventHandler(GameObject chess);
public delegate void DiedEventHandler(GameObject chess);
public delegate void RespawnEventHandler(GameObject chess);

/// <summary>
/// 棋子生命周期管理池(创建、回收、重新放置)    或许该集成到Chess_Base里
/// </summary>
public class ChessManager : MonoBehaviour
{
    public static List<GameObject> livePool;
    public static List<GameObject> diedPool;

    public static event SpawnEventHandler SpawnEvent;
    public static event DiedEventHandler DiedEvent;
    public static event RespawnEventHandler RespawnEvent;

    void Awake()
    {
        livePool = new List<GameObject>();
        diedPool = new List<GameObject>();
    }

    /// <summary>
    /// 绑定棋子-棋局
    /// </summary>
    /// <param name="chess"></param>
    public static void Spawn(GameObject chess)
    {
        livePool.Add(chess);
        chess.SetActive(true);
        SpawnEvent(chess);
    }

    /// <summary>
    /// 棋子死亡
    /// </summary>
    /// <param name="chess"></param>
    public static void Died(GameObject chess)
    {
        livePool.Remove(chess);
        diedPool.Add(chess);
        chess.SetActive(false);
        DiedEvent(chess);       // 取消订阅

    }

    /// <summary>
    /// 棋子重生,或许悔棋时用得到
    /// </summary>
    /// <param name="chess"></param>
    public static void Respawn(GameObject chess)
    {
        livePool.Add(chess);
        diedPool.Remove(chess);
        chess.SetActive(true);
        RespawnEvent(chess);    // 重新订阅
    }

    /// <summary>
    /// 棋子会遮挡Point的Collider,所以对方棋子的Collider要先禁用
    /// </summary>
    /// <param name="moveSide"></param>
    public static void UpdateChessCollider(MoveSide moveSide)
    {
        for (int i = 0; i < livePool.Count; ++i)
        {
            livePool[i].GetComponent<Collider>().enabled = ((int)livePool[i].GetComponent<Chess_Base>().chessColor == (int)moveSide);
        }
    }
}
