using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SpawnEventHandler(GameObject chess);
public delegate void DiedEventHandler(GameObject chess);
public delegate void RespawnEventHandler(GameObject chess);

/// <summary>
/// �����������ڹ����(���������ա����·���)    ����ü��ɵ�Chess_Base��
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
    /// ������-���
    /// </summary>
    /// <param name="chess"></param>
    public static void Spawn(GameObject chess)
    {
        livePool.Add(chess);
        chess.SetActive(true);
        SpawnEvent(chess);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="chess"></param>
    public static void Died(GameObject chess)
    {
        livePool.Remove(chess);
        diedPool.Add(chess);
        chess.SetActive(false);
        DiedEvent(chess);       // ȡ������

    }

    /// <summary>
    /// ��������,�������ʱ�õõ�
    /// </summary>
    /// <param name="chess"></param>
    public static void Respawn(GameObject chess)
    {
        livePool.Add(chess);
        diedPool.Remove(chess);
        chess.SetActive(true);
        RespawnEvent(chess);    // ���¶���
    }

    /// <summary>
    /// ���ӻ��ڵ�Point��Collider,���ԶԷ����ӵ�ColliderҪ�Ƚ���
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
