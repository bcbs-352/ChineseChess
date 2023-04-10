using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 挂载到Grid对象下，管理棋子对象与棋盘着点，可着点变绿
// 由上至下，由左至右,上黑下红 共10行9列，grid下标表示为i(0~9) j(0~8)
public class GridManager : MonoBehaviour
{
    public static Dictionary<Vector2, Transform> grid2point;    // 棋盘Grid二维坐标-着点Point对象
    public static Dictionary<Vector2, GameObject> grid2chess;   // 棋盘Grid二维坐标-棋子Chess对象
    public static Dictionary<GameObject, Vector2> chess2grid;   // 棋子Chess对象-棋盘Grid二维坐标

    public static List<Vector2> accessiblePoints;

    [SerializeField]
    public float offset = 0.41f;

    public static GridManager instance;
    public static GridManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
            instance = new GridManager();
        grid2chess = new Dictionary<Vector2, GameObject>();
        grid2point = new Dictionary<Vector2, Transform>();
        chess2grid = new Dictionary<GameObject, Vector2>();
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                // 获取Point的Transform    不显示绿色(Sphere),赋予坐标
                Transform point = transform.GetChild(9 * i + j);
                grid2point[new Vector2(i, j)] = point;
                point.GetChild(0).gameObject.SetActive(false);
                point.GetComponent<PointClick>().gridPos = new Vector2(i, j);
            }
        }
    }

    /// <summary>
    ///  根据棋子池，更新棋子与网格数据
    ///  GameController每一步调用
    /// </summary>
    public void UpdateGridData()
    {
        chess2grid.Clear();
        grid2chess.Clear();

        for (int i = 0; i < ChessManager.livePool.Count; ++i)
        {
            GameObject chess = ChessManager.livePool[i];
            chess2grid.Add(chess, chess.GetComponent<Chess_Base>().currentPos);
            grid2chess.Add(chess.GetComponent<Chess_Base>().currentPos, chess);
        }
    }

    /// <summary>
    /// 传入可着点列表，先全部重置后将可着点设置绿色
    /// </summary>
    /// <param name="points"></param>
    public static void PointTurnGreen(List<Vector2> points)
    {
        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                grid2point[new Vector2(i, j)].GetChild(0).gameObject.SetActive(false);
            }
        }
        if (points == null)
            return;
        accessiblePoints = points;
        
        for (int i = 0; i < points.Count; ++i)
        {
            grid2point[points[i]].GetChild(0).gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 根据点位放置棋子，调用此函数后还需将棋子交给livePool管理
    /// </summary>
    /// <param name="chess"></param>
    /// <param name="gridPos"></param>
    public void PlaceChess(GameObject chess, Vector2 gridPos)
    {
        chess.transform.position = grid2point[gridPos].position + new Vector3(0, offset, 0);
        grid2chess[gridPos] = chess;
        chess2grid[chess] = gridPos;
        UpdateGridData();
    }
}
